using ClickOnce_Deployment_Manager_64.Utils;
using ClickOnceDeployment.Core;
using ClickOnceDeployment.Data;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for PackageInfo.xaml
    /// </summary>
    public partial class PackageInfo : Page
    {
        private Pointel.Logger.Core.ILog _logger;
        private static PageStatus _pageStatus = new PageStatus();

        public PackageInfo()
        {
            InitializeComponent();
            _logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");
        }

        private void txtSubVersion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //regex that allows numeric input only
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
                e.Handled = true;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.Title = ApplicationDetails.Instance().ApplicationName + " Installer";
                txtApplicationName.Text = ClickOnceManager.Instance().DeploymentManifestInfo.ProductName;
                txtPublisher.Text = ClickOnceManager.Instance().DeploymentManifestInfo.PublisherName;
                txtVersion.Text = ApplicationDetails.Instance().Version;
                if (string.IsNullOrEmpty(GeneralOptions.GetInstance().SubVersion))
                    txtSubVersion.Text = this.GetSubVersion(ApplicationDetails.Instance().DestinationPath);
                else
                    txtSubVersion.Text = GeneralOptions.GetInstance().SubVersion;
                txtBaseURL.Text = this.GetBaseURL();
                ChbAddCustomFiles.IsChecked = GeneralOptions.GetInstance().IsCustomFileChecked;
                ChbDesktopShort.IsChecked = GeneralOptions.GetInstance().IsCreateDesktopChecked;
                if (ClickOnceManager.Instance().DeploymentManifestInfo.AutoUpdateEnabled)
                    AutoRadioButton.IsChecked = true;
                else
                    ManualRadioButton.IsChecked = true;
            }
            catch (Exception generalException)
            {
                _logger.Error("WndPackageInfo Window Load Event:" + generalException.Message);
            }
        }

        private string GetSubVersion(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    string[] files = System.IO.Directory.GetFiles(path, "*.application", System.IO.SearchOption.TopDirectoryOnly);
                    if (files.Length > 0)
                    {
                        _logger.Info("Checking for Previous versions of Manifest on Destination Path");
                        foreach (string file in files)
                        {
                            if (file.Contains(ApplicationDetails.Instance().AppOriginalName))
                            {
                                _logger.Info("Previous version of the same applciation found ");
                                DeployManifest dep = ManifestReader.ReadManifest(file, true) as DeployManifest;
                                _logger.Info("Previous Version :" + dep.AssemblyIdentity.Version);
                                string version = dep.AssemblyIdentity.Version;

                                int digit = Convert.ToInt32(version.Substring(version.LastIndexOf('.') + 1));
                                digit++;
                                return digit.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception generalException)
            {
                _logger.Error("Error Occurred while Getting Deployment SubVersion :" + generalException.ToString());
            }
            return "0";
        }

        private string GetBaseURL()
        {
            try
            {
                string Ipaddress = string.Empty;
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _logger.Info("Internetwork Ipaddress" + ip.ToString());
                        Ipaddress = ip.ToString();
                        break;
                    }
                }
                if ((ClickOnceManager.Instance().IISPath + "\\").Length <= ApplicationDetails.Instance().DestinationPath.Length)
                {
                    int i = ApplicationDetails.Instance().DestinationPath.Length - (ClickOnceManager.Instance().IISPath.Length + 1);
                    if (i > 0)
                    {
                        string temp = "http://" + Ipaddress + "/" + ApplicationDetails.Instance().DestinationPath.Substring(ClickOnceManager.Instance().IISPath.Length + 1);
                        temp = temp.Replace('\\', '/');
                        if (temp.Substring(temp.Length - 1) == "/")
                        {
                            return temp + ApplicationDetails.Instance().AppOriginalName + ".application";
                        }
                        else
                        {
                            return temp + "/" + ApplicationDetails.Instance().AppOriginalName + ".application";
                        }
                    }
                    else
                    {
                        //Dns.GetHostAddresses(Environment.MachineName).ToString()
                        return "http://" + Ipaddress + @"/" + ApplicationDetails.Instance().AppOriginalName + ".application";
                    }
                }
            }
            catch (Exception generalException)
            {
                _logger.Error("Error Occurred while Getting BaseURL :" + generalException.ToString());
            }
            return "";
        }

        public PageStatus OnNextClick()
        {
            try
            {
                if (txtApplicationName.Text.Length > 2)
                {
                    //validate mandatory fields
                    if ((!string.IsNullOrEmpty(txtApplicationName.Text.Trim())) && (!string.IsNullOrEmpty(txtPublisher.Text.Trim())) && (!string.IsNullOrEmpty(txtSubVersion.Text.Trim())) && (!string.IsNullOrEmpty(txtBaseURL.Text.Trim())))
                    {
                        int n;
                        if (int.TryParse(txtSubVersion.Text, out n))
                        {
                            _logger.Info("Application Name :" + txtApplicationName.Text);
                            ClickOnceManager.Instance().DeploymentManifestInfo.ProductName = txtApplicationName.Text;
                            ClickOnceManager.Instance().DeploymentManifestInfo.ClickonceProductVerion = txtVersion.Text + "." + txtSubVersion.Text;
                            string[] arr = txtVersion.Text.Split('.');

                            string ver = arr[0] + arr[1] + "." + arr[2] + "." + arr[3] + "." + txtSubVersion.Text;

                            _logger.Info("Product Version in Assembly:" + ver);
                            ClickOnceManager.Instance().DeploymentManifestInfo.ProductVersion = ver;

                            _logger.Info("Deployment URL :" + txtBaseURL.Text);
                            ClickOnceManager.Instance().DeploymentManifestInfo.DeploymentURL = txtBaseURL.Text;
                            _logger.Info("Publisher Name :" + txtPublisher.Text);
                            ClickOnceManager.Instance().DeploymentManifestInfo.PublisherName = txtPublisher.Text;
                            if (ChbDesktopShort.IsChecked == true)
                            {
                                _logger.Info("Create desktop shortcut is checked");
                                GeneralOptions.GetInstance().IsCreateDesktopChecked = true;
                                ClickOnceManager.Instance().DeploymentManifestInfo.DesktopShortcut = true;
                            }
                            else
                            {
                                _logger.Info("Create desktop shortcut is not checked");
                                ClickOnceManager.Instance().DeploymentManifestInfo.DesktopShortcut = false;
                                GeneralOptions.GetInstance().IsCreateDesktopChecked = false;
                            }
                            _pageStatus.PageCompleteStatus = PageCompleteStatus.Success;
                            if (ChbAddCustomFiles.IsChecked == true)
                            {
                                GeneralOptions.GetInstance().IsCustomFileChecked = true;
                            }
                            else
                            {
                                GeneralOptions.GetInstance().IsCustomFileChecked = false;
                            }
                        }
                        else
                        {
                            txtSubVersion.Text = "";
                            _pageStatus.PageCompleteStatus = PageCompleteStatus.Invalid;
                            _pageStatus.MessageText = "Subversion should be a integer number.";
                        }
                    }
                    else
                    {
                        _pageStatus.PageCompleteStatus = PageCompleteStatus.Empty;
                        _pageStatus.MessageText = "The mandatory fields should not be empty.";
                    }
                }
                else
                {
                    _pageStatus.MessageText = "Application Name Should be atleast 3 characters.";
                    _pageStatus.PageCompleteStatus = PageCompleteStatus.Invalid;
                }
                GeneralOptions.GetInstance().SubVersion = txtSubVersion.Text;
            }
            catch (Exception generalException)
            {
                _logger.Error("PackageInfo OnNext_Click :" + generalException.Message);
            }
            return _pageStatus;
        }

        #region ManualRadioButton_Checked

        private void ManualRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ClickOnceManager.Instance().DeploymentManifestInfo.AutoUpdateEnabled = false;
        }

        #endregion ManualRadioButton_Checked

        #region AutoRadioButton_Checked

        private void AutoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ClickOnceManager.Instance().DeploymentManifestInfo.AutoUpdateEnabled = true;
        }

        #endregion AutoRadioButton_Checked

        private void ChbAddCustomFiles_Checked(object sender, RoutedEventArgs e)
        {
            GeneralOptions.GetInstance().IsCustomFileChecked = true;
        }

        private void ChbDesktopShort_Checked(object sender, RoutedEventArgs e)
        {
            GeneralOptions.GetInstance().IsCreateDesktopChecked = true;
        }

        private void ChbAddCustomFiles_Unchecked(object sender, RoutedEventArgs e)
        {
            GeneralOptions.GetInstance().IsCustomFileChecked = false;
        }

        private void ChbDesktopShort_Unchecked(object sender, RoutedEventArgs e)
        {
            GeneralOptions.GetInstance().IsCreateDesktopChecked = false;
        }
    }
}