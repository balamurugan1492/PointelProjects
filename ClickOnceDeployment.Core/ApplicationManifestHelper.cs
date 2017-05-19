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
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class ApplicationManifestHelper
    {
        #region Data members

        private ApplicationManifest _applicationManifest;
        private static ApplicationManifestHelper _objDeploymentManifest;

        #endregion Data members

        #region Properties

        public string FilePath
        {
            get;
            private set;
        }

        #endregion Properties

        #region Constructor

        private ApplicationManifestHelper()
        {
        }

        #endregion Constructor

        #region SingleInstance

        public static ApplicationManifestHelper Instance()
        {
            if (_objDeploymentManifest == null)
                _objDeploymentManifest = new ApplicationManifestHelper();

            return _objDeploymentManifest;
        }

        #endregion SingleInstance

        #region Methods

        public MessageResult LoadManifest()
        {
            MessageResult objResult = new MessageResult();
            objResult.SetMessage("The deployment manifest file loaded successfully.", true);

            if (!string.IsNullOrEmpty(ApplicationDetails.Instance().AppFolderName + ApplicationDetails.Instance().AppOriginalName))
            {
                FilePath = ApplicationDetails.Instance().AppFolderName + ApplicationDetails.Instance().AppOriginalName + ".exe.manifest";

                if (System.IO.File.Exists(FilePath))
                    _applicationManifest = ManifestReader.ReadManifest(FilePath, true) as ApplicationManifest;
                else
                    objResult.SetMessage("The application original path is null.");
            }
            else
                objResult.SetMessage("The application original path is null.");

            return objResult;
        }

        public MessageResult UpdateManifest(string fileVersion)
        {
            MessageResult objResult = new MessageResult();
            objResult.SetMessage("The application manifest file updated successfully.", true);

            if (!string.IsNullOrEmpty(FilePath))
            {
                if (string.IsNullOrEmpty(fileVersion))
                    objResult.SetMessage("file version is null.");
                else
                {
                    _applicationManifest.AssemblyIdentity.Version = fileVersion;
                    _applicationManifest.ResolveFiles();
                    _applicationManifest.UpdateFileInfo();
                    ManifestWriter.WriteManifest(_applicationManifest, FilePath);
                }
            }
            else
                objResult.SetMessage("The deployment file path is null.");

            return objResult;
        }

        internal MessageResult AddFile(string filePath)
        {
            MessageResult objResult = new MessageResult();
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    objResult.SetMessage("The file path is Null or Empty");
                else
                {
                    string temp = Path.GetDirectoryName(filePath);
                    if (temp.Length > ApplicationDetails.Instance().AppFolderName.Length - 1)
                    {
                        temp = filePath.Substring(ApplicationDetails.Instance().AppFolderName.Length);
                        if (temp.Contains(@"\"))
                        {
                            if (!IsFileExist(temp))
                            {
                                FileReference _fileReference = new Microsoft.Build.Tasks.Deployment.ManifestUtilities.FileReference(filePath);
                                _fileReference.TargetPath = temp;
                                _fileReference.Group = "CustomFile";
                                _fileReference.ResolvedPath = temp;
                                _applicationManifest.FileReferences.Add(_fileReference);
                            }
                            else
                            {
                                objResult.SetMessage("The filePath already added into File references", true);
                            }
                        }
                    }
                    else
                    {
                        if (!IsFileExist(Path.GetFileName(filePath)))
                        {
                            FileReference _fileReference = new Microsoft.Build.Tasks.Deployment.ManifestUtilities.FileReference(filePath);
                            _fileReference.TargetPath = Path.GetFileName(filePath);
                            _fileReference.Group = "CustomFile";
                            _fileReference.ResolvedPath = Path.GetFileName(filePath);
                            _applicationManifest.FileReferences.Add(_fileReference);
                        }
                    }
                }
            }
            catch (Exception generalException)
            {
                objResult.SetMessage(generalException.Message);
            }
            if (string.IsNullOrEmpty(objResult.MessageText))
                objResult.SetMessage("The file added successfully.", true);

            return objResult;
        }

        public MessageResult RemoveFile(string fileName)
        {
            MessageResult objResult = new MessageResult();
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    objResult.SetMessage("The file name is Null or Empty");
                else if (IsFileExist(fileName))
                {
                    // code to remove file.
                    if (_applicationManifest.FileReferences.FindTargetPath(fileName) != null)
                        _applicationManifest.FileReferences.Remove(_applicationManifest.FileReferences.FindTargetPath(fileName));
                    //else if (_applicationManifest.AssemblyReferences.FindTargetPath(fileName) != null)
                    //    _applicationManifest.AssemblyReferences.Remove(_applicationManifest.AssemblyReferences.FindTargetPath(fileName));
                    //else
                    //    objResult.SetMessage("The file remove operation getting failed.");
                }
            }
            catch (Exception generalException)
            {
                objResult.SetMessage(generalException.Message);
            }

            if (string.IsNullOrEmpty(objResult.MessageText))
                objResult.SetMessage("The file removed successfully.", true);

            return objResult;
        }

        public void SetFileOptionState(string fileName, bool isOptional)
        {
        }

        public void SetFileDataState(string fileName, bool isOptional)
        {
        }

        public List<string> GetFileCollection()
        {
            List<string> lstFile = new List<string>();
            if (_applicationManifest != null)
            {
                foreach (FileReference file in _applicationManifest.FileReferences)
                    lstFile.Add(file.TargetPath);
                foreach (AssemblyReference assembly in _applicationManifest.AssemblyReferences)
                    lstFile.Add(assembly.TargetPath);
            }
            return lstFile;
        }

        public List<FileReference> GetFileReference()
        {
            List<FileReference> lstFile = new List<FileReference>();
            if (_applicationManifest != null)
            {
                foreach (FileReference file in _applicationManifest.FileReferences)
                    if (file.Group == "CustomFile")
                        lstFile.Add(file);
            }
            return lstFile;
        }

        public bool IsFileExist(string filePath)
        {
            return (_applicationManifest.FileReferences != null &&
                _applicationManifest.FileReferences.FindTargetPath(filePath) != null
                && _applicationManifest.FileReferences.FindTargetPath(filePath) != null);
        }

        #endregion Methods
    }
}