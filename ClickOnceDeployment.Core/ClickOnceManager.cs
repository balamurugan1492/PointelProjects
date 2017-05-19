/* ====================================
 * ClickOnceDeployment.Core
 * ====================================
 * Project   : ClickOnce Deployment
 * Created On: 19-11-2015
 * Author    : Sakthikumar
 * Owner     : Pointel Solution
 * ====================================
 */

namespace ClickOnceDeployment.Core
{
    using ClickOnceDeployment.Data;
    using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
    using Pointel.WebServer.Util;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml;

    public delegate void DeploymentStateHandler(string message, int finishedState);

    public delegate void DeploymentFinishHandler(OperationStatus status);

    /// <summary>
    /// The ClickOnceManager provide the gateway to perform the clickonce deployment.
    /// </summary>
    public class ClickOnceManager
    {
        #region Data members

        private static ClickOnceManager _objClickonceManager;
        private DirectoryInfo mainDir = null;
        private string _mainDirectory;

        #endregion Data members

        #region Events

        public event DeploymentStateHandler DeploymentStateEvent;

        public event DeploymentFinishHandler DeploymentFinishEvent;

        #endregion Events

        #region Properties

        public DeploymentManifestInfo DeploymentManifestInfo
        {
            get;
            internal set;
        }

        public string IISPath
        {
            get;
            private set;
        }

        public List<string> FileCollection
        {
            get
            {
                return ApplicationManifestHelper.Instance().GetFileCollection();
            }
        }

        public List<FileReference> FileReference
        {
            get
            {
                return ApplicationManifestHelper.Instance().GetFileReference();
            }
        }

        //modified by balamurugan on 26/04/2016
        public string[] RequiredFiles
        {
            get
            {
                return Directory.GetFiles(Environment.CurrentDirectory + "\\" + ConfigurationManager.AppSettings["AppFolderName"].ToString());
            }
        }

        #endregion Properties

        #region Constructor

        private ClickOnceManager()
        {
        }

        #endregion Constructor

        #region SingleInstance

        public static ClickOnceManager Instance()
        {
            if (_objClickonceManager == null)
                _objClickonceManager = new ClickOnceManager();

            return _objClickonceManager;
        }

        #endregion SingleInstance

        #region Methods

        /// <summary>
        /// Initialize the all clickonce configuration.
        /// </summary>
        public IMessage Initialize()
        {
            MessageResult objResult = new MessageResult();
            try
            {
                WebServerManager objWebServer = new WebServerManager();
                if (!objWebServer.IsIISInstalled())
                    objResult.SetMessage("IIS Server is not available in your machine. Please Install IIS Server First and then start application installtion.");
                else
                {
                    objResult = DeploymentManifestHelper.Instance().LoadManifest();

                    if (objResult.OperationStatus == OperationStatus.Success)
                    {
                        DeploymentManifestInfo = DeploymentManifestHelper.Instance().GetDeploymentInformation();
                        objResult = ApplicationManifestHelper.Instance().LoadManifest();
                    }

                    IISPath = objWebServer.GetIISFolderPath();
                    if (string.IsNullOrEmpty(ApplicationDetails.Instance().DestinationPath))
                    {
                        ApplicationDetails.Instance().DestinationPath = ClickOnceManager.Instance().IISPath + @"\" + ApplicationDetails.Instance().ApplicationName + "V" + ApplicationDetails.Instance().Version;
                    }
                }

                if (objResult.OperationStatus == OperationStatus.Success)
                    objResult.SetMessage("Clickonce manager initialized successfully.", true);
            }
            catch (Exception generalException)
            {
                objResult.SetMessage(generalException.Message);
            }
            return objResult as IMessage;
        }

        public IMessage UpdateAttribute(ClickOnceAttribute attributeName, string data)
        {
            MessageResult objResult = new MessageResult();
            try
            {
                switch (attributeName)
                {
                    default:
                        objResult.SetMessage("Unknown attribute.");
                        break;
                }
                if (objResult.OperationStatus == OperationStatus.Success)
                    objResult.SetMessage("The deployment data updated successfully.", true);
            }
            catch (Exception generalException)
            {
                objResult.SetMessage(generalException.Message);
            }

            return objResult as IMessage;
        }

        public void ChangeUpdateMode(UpdateMode updateMode)
        {
            DeploymentManifestInfo.AppUpdateMode = updateMode;
        }

        public void ChangeUpdatUnit(UpdateUnit updateUnit)
        {
            DeploymentManifestInfo.UpdateUnit = updateUnit;
        }

        public IMessage FinishDeployment(string hostName, string portNo, string applicationName, string subVersion)
        {
            MessageResult objResult = new MessageResult();
            try
            {
                UpdateConfigFile(hostName, portNo, applicationName, subVersion);
                DeploymentStateEvent.Invoke("Updating application manifest... ", 1);
                ApplicationManifestHelper.Instance().UpdateManifest(DeploymentManifestInfo.ProductVersion);
                DeploymentStateEvent.Invoke("Application manifest updated successfully...", 10);
                DeploymentStateEvent.Invoke("Updating application manifest... ", 10);
                DeploymentManifestHelper.Instance().UpdateManifest(DeploymentManifestInfo);
                DeploymentStateEvent.Invoke("Deployment manifest updated successfully...", 20);
                CreateDestinationDirectory();
                CopyFiles();
                GenerateSetupExe();
                Helper.HtmlHelper objHtmlHelper = new Helper.HtmlHelper();
                //imagepath
                //ApplicationDetails.Instance().ApplicationName+"_"+DeploymentManifestInfo.ProductVersion.Replace(".","_")
                objHtmlHelper.GenerateHtmlPage(_mainDirectory, ApplicationDetails.Instance().ApplicationName, DeploymentManifestInfo.ClickonceProductVerion
                    , DeploymentManifestInfo.PublisherName, "", DeploymentManifestInfo.DeploymentURL,
                    DeploymentManifestInfo.DeploymentURL.Substring(0, DeploymentManifestInfo.DeploymentURL.LastIndexOf('/') + 1) + "setup.exe");

                if (objResult.OperationStatus == OperationStatus.Success)
                    objResult.SetMessage("Clickonce deployment manager deployed application successfully.", true);
            }
            catch (Exception generalException)
            {
                objResult.SetMessage(generalException.Message);
            }

            return objResult as IMessage;
        }

        public IMessage AddNewFile(string filePath)
        {
            return ApplicationManifestHelper.Instance().AddFile(filePath);
        }

        public IMessage RemoveFile(string filePath)
        {
            return ApplicationManifestHelper.Instance().RemoveFile(filePath);
        }

        private string _subDirName;

        private void CopyFiles()
        {
            if (!string.IsNullOrEmpty(ApplicationDetails.Instance().MainDirFiles))
            {
                var files = ApplicationDetails.Instance().MainDirFiles.Split(',');
                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        if (File.Exists(ApplicationDetails.Instance().AppFolderName + file))
                        {
                            DeploymentStateEvent.Invoke("Coping " + file + " ... ", 40);
                            if (file.ToLower().Contains("dotnetfx"))
                            {
                                if (!Directory.Exists(_mainDirectory + @"dotnetfx45\"))  // if it doesn't exist, create
                                    Directory.CreateDirectory(_mainDirectory + @"dotnetfx45\");
                                File.Copy(ApplicationDetails.Instance().AppFolderName + file, _mainDirectory + @"dotnetfx45\" + file, true);
                            }
                            else
                                File.Copy(ApplicationDetails.Instance().AppFolderName + file, _mainDirectory + @"\" + file, true);
                        }
                        else if (file.ToLower().Contains(".application"))
                        {
                            File.Copy(ApplicationDetails.Instance().AppFolderName + file, _mainDirectory + @"\" + file, true);
                        }
                    }
                }
            }

            //modified by balamurugan on 26/10/2016
            if (RequiredFiles.Length != 0 && RequiredFiles != null)
            {
                foreach (string file in RequiredFiles)
                {
                    string[] fileName = file.Split('\\');
                    //|| fileName[fileName.Length - 1] != "setup.exe"
                    if (!fileName[fileName.Length - 1].ToLower().Contains("dotnetfx"))
                    {
                        if (File.Exists(file))
                        {
                            DeploymentStateEvent.Invoke("Coping " + file + " ... ", 60);
                            File.Copy(file, _subDirName + @"\" + Path.GetFileName(file), true);
                        }
                    }
                }
            }

            // Code to copy the files referenced in the application.

            foreach (string fileName in FileCollection)
            {
                if (File.Exists(ApplicationDetails.Instance().AppFolderName + fileName))
                {
                    if (fileName.Contains(@"\"))
                    {
                        string temp = _subDirName + "\\" + fileName;
                        if (!Directory.Exists(temp.Substring(0, temp.LastIndexOf(@"\"))))
                        {
                            Directory.CreateDirectory(temp.Substring(0, temp.LastIndexOf(@"\")));
                            DeploymentStateEvent.Invoke("Coping " + fileName + " ... ", 70);
                        }
                        File.Copy(ApplicationDetails.Instance().AppFolderName + fileName, temp, true);
                    }
                    else
                    {
                        DeploymentStateEvent.Invoke("Coping " + fileName + " ... ", 70);
                        // File.Copy(ApplicationDetails.Instance().AppFolderName + fileName, _subDirName + "\\" + fileName + ".deploy", true);
                    }
                }
            }

            DeploymentFinishEvent.Invoke(OperationStatus.Success);
        }

        private void CreateDestinationDirectory()
        {
            string version = DeploymentManifestInfo.ProductVersion.Replace('.', '_');
            mainDir = new DirectoryInfo(ApplicationDetails.Instance().DestinationPath);
            _mainDirectory = mainDir.FullName;
            DeploymentStateEvent.Invoke("Creating Directory " + mainDir.FullName + " ... ", 25);
            mainDir.Create();

            if (mainDir.Exists == true)
            {
                DeploymentStateEvent.Invoke("Creating Directory Application Files ... ", 30);
                DirectoryInfo SubDir = mainDir.CreateSubdirectory(@"Application Files");
                //DeploymentStateEvent.Invoke("Creating Directory " + DeploymentManifestInfo.ProductName + "_" + version + " ... ", 35);
                //DirectoryInfo SubDir1 = SubDir.CreateSubdirectory(DeploymentManifestInfo.ProductName + "_" + version);
                DeploymentStateEvent.Invoke("Creating Directory " + ApplicationDetails.Instance().AppOriginalName + "_" + version + " ... ", 35);
                DirectoryInfo SubDir1 = SubDir.CreateSubdirectory(ApplicationDetails.Instance().AppOriginalName + "_" + version);
                _subDirName = SubDir1.FullName;
            }
        }

        #region Generate SetupFile

        private void GenerateSetupExe()
        {
            try
            {
                if (File.Exists(mainDir.FullName + @"\setup.exe"))
                {
                    string url = DeploymentManifestInfo.DeploymentURL.Substring(0, DeploymentManifestInfo.DeploymentURL.LastIndexOf("/") + 1);
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    string path = Directory.GetDirectoryRoot(mainDir.FullName);
                    path = path.Substring(0, path.LastIndexOf(":") + 1);
                    p.Start();
                    p.StandardInput.WriteLine(path);
                    p.StandardInput.WriteLine(@"cd " + mainDir.FullName);
                    p.StandardInput.WriteLine("setup.exe -url=" + url);
                    p.Close();
                    p.Dispose();
                }
            }
            catch (Exception generalException)
            {
                DeploymentStateEvent.Invoke("Error Occurred while modifying setup.exe :" + generalException.ToString(), 30);
            }
        }

        #endregion Generate SetupFile

        #region Update Configuration Details

        //added by balamurugan
        //for update the config file
        private void UpdateConfigFile(string hostName, string portNo, string applicationName, string subVersion)
        {
            try
            {
                string configURL = string.Empty;
                if (!string.IsNullOrEmpty(hostName))
                {
                    if (!string.IsNullOrEmpty(portNo))
                    {
                        configURL = "tcp://" + hostName + ":" + portNo + "/" + applicationName;
                    }
                    else if (!string.IsNullOrEmpty(applicationName))
                    {
                        configURL = "tcp://" + hostName + "/" + applicationName;
                    }
                    else
                    {
                        configURL = "tcp://" + hostName + "/";
                    }
                }
                else
                {
                    configURL = "";
                }
                string fileName = ApplicationDetails.Instance().AppFolderName + ApplicationDetails.Instance().AppOriginalName + ".exe.config";
                if (File.Exists(fileName))
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(fileName);
                        XmlNode appsetting = doc.ChildNodes[1].ChildNodes.Cast<XmlNode>().Where(x => x.Name.Equals("appSettings")).Single<XmlNode>();
                        foreach (XmlNode node in appsetting.ChildNodes.Cast<XmlNode>().Where(x => x.Attributes != null && x.Attributes.Count == 2))
                        {
                            try
                            {
                                //For Existing node
                                if (node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.Equals("key")).Single<XmlAttribute>().Value.ToLower().Equals("login.url"))
                                {
                                    node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.Equals("value")).Single<XmlAttribute>().Value = configURL;
                                }

                                if (node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.Equals("key")).Single<XmlAttribute>().Value.ToLower().Equals("application.version"))
                                {
                                    node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.Equals("value")).Single<XmlAttribute>().Value = ApplicationDetails.Instance().Version + "." + subVersion;
                                }

                                if (node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.Equals("key")).Single<XmlAttribute>().Value.ToLower().Equals("auto.update"))
                                {
                                    node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.Equals("value")).Single<XmlAttribute>().Value = ClickOnceManager.Instance().DeploymentManifestInfo.AutoUpdateEnabled.ToString();
                                }
                            }
                            catch (Exception generalException)
                            {
                                DeploymentStateEvent.Invoke("UpdateConfigFile : Error Occurred while updating configuration file : " + generalException.ToString(), 30);
                            }
                        }
                        doc.Save(fileName);
                    }
                    catch (Exception generalException)
                    {
                        DeploymentStateEvent.Invoke("Error Occurred while reading Config file : " + generalException.ToString(), 30);
                    }
                }
                else
                {
                    DeploymentStateEvent.Invoke("Configuration File Cound not be updated :" + ApplicationDetails.Instance().AppOriginalName + ".exe.config file Not Found", 30);
                }
            }
            catch (Exception generalException)
            {
                DeploymentStateEvent.Invoke("UpdateConfigFile : Error Occurred while updating configuration file : " + generalException.ToString(), 30);
            }
        }

        #endregion Update Configuration Details

        #endregion Methods
    }
}