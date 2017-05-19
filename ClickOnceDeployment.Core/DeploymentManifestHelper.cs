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

    internal class DeploymentManifestHelper
    {
        #region Fields Declaration

        private DeployManifest deployManifest;
        private static DeploymentManifestHelper _objDeploymentManifest;

        #endregion Fields Declaration

        #region Property

        public string FilePath
        {
            get;
            private set;
        }

        #endregion Property

        #region Constructor

        private DeploymentManifestHelper()
        {
        }

        #endregion Constructor

        #region Single Instance

        public static DeploymentManifestHelper Instance()
        {
            if (_objDeploymentManifest == null)
                _objDeploymentManifest = new DeploymentManifestHelper();

            return _objDeploymentManifest;
        }

        #endregion Single Instance

        #region Methods

        public MessageResult LoadManifest()
        {
            MessageResult objResult = new MessageResult();
            objResult.SetMessage("The deployment manifest file loaded successfully.", true);

            if (!string.IsNullOrEmpty(ApplicationDetails.Instance().AppFolderName + ApplicationDetails.Instance().AppOriginalName))
            {
                FilePath = ApplicationDetails.Instance().AppFolderName + ApplicationDetails.Instance().AppOriginalName + ".application";

                if (System.IO.File.Exists(FilePath))
                    deployManifest = ManifestReader.ReadManifest(FilePath, true) as DeployManifest;
                else
                    objResult.SetMessage("The application original path is null.");
            }
            else
                objResult.SetMessage("The application original path is null.");

            return objResult;
        }

        public MessageResult UpdateManifest(DeploymentManifestInfo deploymentManifestInfo)
        {
            MessageResult objResult = new MessageResult();
            objResult.SetMessage("The deployment manifest file updated successfully.", true);

            if (deploymentManifestInfo != null)
            {
                if (!string.IsNullOrEmpty(FilePath))
                {
                    if (string.IsNullOrEmpty(deploymentManifestInfo.PublisherName) || string.IsNullOrEmpty(deploymentManifestInfo.ProductVersion)
                        || string.IsNullOrEmpty(deploymentManifestInfo.DeploymentURL) || string.IsNullOrEmpty(deploymentManifestInfo.ProductName))
                        objResult.SetMessage("Required information is missing.");
                    else
                    {
                        deployManifest.CreateDesktopShortcut = deploymentManifestInfo.DesktopShortcut;
                        deployManifest.Publisher = deploymentManifestInfo.PublisherName;
                        deployManifest.AssemblyIdentity.Version = deploymentManifestInfo.ProductVersion;
                        deployManifest.DeploymentUrl = deploymentManifestInfo.DeploymentURL;
                        if (deploymentManifestInfo.AutoUpdateEnabled)
                        {
                            deployManifest.UpdateEnabled = false;
                        }
                        else
                        {
                            deployManifest.UpdateEnabled = true;
                            deployManifest.UpdateMode = UpdateMode.Foreground;
                        }
                        deployManifest.Product = deploymentManifestInfo.ProductName;
                        deployManifest.EntryPoint.AssemblyIdentity.Version = deploymentManifestInfo.ProductVersion;
                        deployManifest.EntryPoint.TargetPath = "Application Files\\" + ApplicationDetails.Instance().AppOriginalName + "_" + deploymentManifestInfo.ProductVersion.Replace('.', '_') + @"\" + ApplicationDetails.Instance().AppOriginalName + ".exe.manifest";
                        deployManifest.AssemblyReferences.FindTargetPath(FilePath);
                        deployManifest.ResolveFiles();
                        deployManifest.UpdateFileInfo();
                        ManifestWriter.WriteManifest(deployManifest, FilePath);
                    }
                }
                else
                    objResult.SetMessage("The deployment file path is null.");
            }
            else
                objResult.SetMessage("The deployment information is null.");

            return objResult;
        }

        public DeploymentManifestInfo GetDeploymentInformation()
        {
            DeploymentManifestInfo objDeploymentManifestInfo = new DeploymentManifestInfo();
            if (deployManifest != null)
            {
                objDeploymentManifestInfo.ProductName = deployManifest.Product;
                objDeploymentManifestInfo.PublisherName = deployManifest.Publisher;
                objDeploymentManifestInfo.ProductVersion = deployManifest.AssemblyIdentity.Version;
            }
            return objDeploymentManifestInfo;
        }
    }

        #endregion Methods
}