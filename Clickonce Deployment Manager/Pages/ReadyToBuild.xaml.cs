using ClickOnceDeployment.Core;
using ClickOnceDeployment.Data;
using System;
using System.Windows.Controls;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for ReadToBuild.xaml
    /// </summary>
    public partial class ReadyToBuild : Page
    {
        private Pointel.Logger.Core.ILog _logger;

        public ReadyToBuild()
        {
            InitializeComponent();
            _logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _logger.Info("ReadyToBuild.Page_Loaded()");
                string path = @"Application Files\" + ApplicationDetails.Instance().ApplicationName + "_" + ClickOnceManager.Instance().DeploymentManifestInfo.ProductVersion.Replace('.', '_') + @"\";
                LSBFileList.Items.Add("The application package '" + ApplicationDetails.Instance().ApplicationName + "' version " + ApplicationDetails.Instance().Version + " will be deployed.");
                LSBFileList.Items.Add("");
                LSBFileList.Items.Add("The deployment URL used to install this package is:");

                if (ClickOnceManager.Instance().DeploymentManifestInfo.DeploymentURL.Contains("/"))
                {
                    LSBFileList.Items.Add(ClickOnceManager.Instance().DeploymentManifestInfo.DeploymentURL.Substring(0, ClickOnceManager.Instance().DeploymentManifestInfo.DeploymentURL.LastIndexOf("/") + 1) + "install.html");
                }

                LSBFileList.Items.Add("");
                LSBFileList.Items.Add("The following files will be copied on the deployment folder is:");
                LSBFileList.Items.Add(ApplicationDetails.Instance().DestinationPath.Substring(0, ApplicationDetails.Instance().DestinationPath.LastIndexOf(@"\")));
                LSBFileList.Items.Add("");

                foreach (string fileName in ClickOnceManager.Instance().FileCollection)
                    LSBFileList.Items.Add(fileName);
            }
            catch (Exception generalException)
            {
                _logger.Error("ReadyToBuild.Page_Loaded(): " + generalException.Message);
            }
        }
    }
}