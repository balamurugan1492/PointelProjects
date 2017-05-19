using ClickOnceDeployment.Data.Utilities;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ClickOnceDeployment.Data
{
    public class ApplicationDetails
    {
        private static ApplicationDetails _objApplicationDetails;
        private Pointel.Logger.Core.ILog logger;

        #region Properties

        public bool DecisionDetermined
        {
            get;
            set;
        }

        public string ApplicationName
        {
            get;
            private set;
        }

        public string Version
        {
            get;
            private set;
        }

        public string DestinationPath
        {
            get;
            set;
        }

        public string AppOriginalName
        {
            get;
            private set;
        }

        //modified by balamurugan on 26/04/2016
        //public string RequiredFiles
        //{
        //    get;
        //    private set;
        //}

        public string MainDirFiles
        {
            get;
            private set;
        }

        public string AppFolderName
        {
            get;
            private set;
        }

        #endregion Properties

        #region Constructor Read App.Config

        private ApplicationDetails()
        {
            logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnceDeployment");
        }

        public void ReadConfigurations()
        {
            logger.Debug("Try to read application configuration info.");
            try
            {
                foreach (string keyName in ConfigurationManager.AppSettings.AllKeys)
                {
                    switch (keyName)
                    {
                        case "ApplicationName":
                            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"].ToString();
                            break;

                        case "Version":
                            Version = ConfigurationManager.AppSettings["Version"].ToString();
                            break;

                        case "DestinationPath":
                            DestinationPath = ConfigurationManager.AppSettings["DestinationPath"].ToString();
                            break;

                        case "AppOriginalName":
                            AppOriginalName = ConfigurationManager.AppSettings["AppOriginalName"].ToString();
                            break;
                        //modified by balamurugan on 26/04/2016
                        //case "RequiredFiles":
                        //    RequiredFiles = ConfigurationManager.AppSettings["RequiredFiles"].ToString();
                        //    break;
                        case "MainDirFiles":
                            MainDirFiles = ConfigurationManager.AppSettings["MainDirFiles"].ToString();
                            break;

                        case "AppFolderName":
                            AppFolderName = Environment.CurrentDirectory + @"\" + ConfigurationManager.AppSettings["AppFolderName"].ToString() + @"\";
                            string[] subFoldersPathArray = Directory.GetDirectories(AppFolderName);
                            for (int i = 0; i < subFoldersPathArray.Length; i++)
                            {
                                string[] folderPathArray = subFoldersPathArray[i].Split('\\');
                                CustomFileDetails.Instance().Subfolders.Add(folderPathArray[folderPathArray.Length - 1]);
                            }
                            break;
                    }
                }

                logger.Info("The application configuration info read successfully.");
                logger.Trace("Application information=" + this.ToString());
            }
            catch (Exception generalException)
            {
                logger.Error(generalException.Message);
            }
            logger.Info("Reading Application information completed");
        }

        public override string ToString()
        {
            StringBuilder objSB = new StringBuilder();
            objSB.Append("\nApplicationName=" + ApplicationName.GetValue());
            objSB.Append("\nVersion=" + Version.GetValue());
            objSB.Append("\nDestinationPath=" + DestinationPath.GetValue());
            //objSB.Append("\nRequiredFiles=" + RequiredFiles.GetValue());
            objSB.Append("\nMainDirFiles=" + MainDirFiles.GetValue());
            objSB.Append("\nAppFolderName=" + AppFolderName.GetValue());
            return objSB.ToString();
        }

        #endregion Constructor Read App.Config

        #region SingleInstance

        public static ApplicationDetails Instance()
        {
            if (_objApplicationDetails == null)
                _objApplicationDetails = new ApplicationDetails();

            return _objApplicationDetails;
        }

        #endregion SingleInstance
    }
}