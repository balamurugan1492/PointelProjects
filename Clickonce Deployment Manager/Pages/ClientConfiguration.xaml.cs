using ClickOnce_Deployment_Manager_64.Utils;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for ClientConfiguration.xaml
    /// </summary>
    public partial class ClientConfiguration : Page
    {
        private Pointel.Logger.Core.ILog _logger;
        private static PageStatus _pageStatus = new PageStatus();

        public ClientConfiguration()
        {
            InitializeComponent();
            _logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");
        }

        #region PortNumber_PreviewTextInput

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtApplicationName.Text = GeneralOptions.GetInstance().ApplicationName;
            txtHostName.Text = GeneralOptions.GetInstance().HostName;
            txtPortNumber.Text = GeneralOptions.GetInstance().Port;
        }

        private void txtPortNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that allows numeric input only
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        #endregion PortNumber_PreviewTextInput

        //#region ManualRadioButton_Checked

        //private void ManualRadioButton_Checked(object sender, RoutedEventArgs e)
        //{
        //    ClickOnceManager.Instance().DeploymentManifestInfo.AutoUpdateEnabled = false;
        //}

        //#endregion ManualRadioButton_Checked

        //#region AutoRadioButton_Checked

        //private void AutoRadioButton_Checked(object sender, RoutedEventArgs e)
        //{
        //    ClickOnceManager.Instance().DeploymentManifestInfo.AutoUpdateEnabled = true;
        //}

        //#endregion AutoRadioButton_Checked

        public PageStatus OnNextClick()
        {
            try
            {
                _logger.Info("BtnNext_Click : Checking Configuration details....");
                if (!string.IsNullOrWhiteSpace(txtHostName.Text) || !string.IsNullOrWhiteSpace(txtPortNumber.Text) || !string.IsNullOrWhiteSpace(txtApplicationName.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txtHostName.Text))
                    {
                        GeneralOptions.GetInstance().ApplicationName = txtApplicationName.Text;
                        GeneralOptions.GetInstance().HostName = txtHostName.Text;
                        GeneralOptions.GetInstance().Port = txtPortNumber.Text;
                        _pageStatus.PageCompleteStatus = PageCompleteStatus.Success;
                    }
                    else if (!string.IsNullOrWhiteSpace(txtPortNumber.Text) && string.IsNullOrWhiteSpace(txtHostName.Text))
                    {
                        _pageStatus.PageCompleteStatus = PageCompleteStatus.Empty;
                        _pageStatus.MessageText = "Please provide Hostname and Port detail, Otherwise it will not be used at Login.Are you sure that you want to proceed next?";
                        txtApplicationName.Focus();
                    }
                    else if (!string.IsNullOrWhiteSpace(txtApplicationName.Text) && string.IsNullOrWhiteSpace(txtHostName.Text) && string.IsNullOrWhiteSpace(txtPortNumber.Text))
                    {
                        _pageStatus.PageCompleteStatus = PageCompleteStatus.Empty;
                        _pageStatus.MessageText = "Please provide Hostname and Port detail, Otherwise it will not be used at Login.Are you sure that you want to proceed next?";

                        txtApplicationName.Focus();
                    }
                    //else if (!string.IsNullOrWhiteSpace(txtPortNumber.Text) || !string.IsNullOrEmpty(txtHostName.Text))
                    //{
                    //    if (CustomMessageBox.COMessageBox.ShowMessage("Please provide Hostname and Port detail, Otherwise it will not be used at Login.Are you sure that you want to proceed next?", "Field required", CustomMessageBox.COMessageBox.ButtonType.Ok))
                    //    {
                    //        txtApplicationName.Focus();
                    //    }
                    //    else
                    //    {
                    //        ApplicationName = txtApplicationName.Text;
                    //        Host = txtHostName.Text;
                    //        Port = txtPortNumber.Text;
                    //        readyToBuild.ClientConfiguration = this;
                    //        this.Hide();
                    //        readyToBuild.Show();
                    //    }
                    //}
                }
                else
                {
                    _pageStatus.PageCompleteStatus = PageCompleteStatus.Success;
                }
            }
            catch (Exception generalException)
            {
                _logger.Error("ClientConfiguration OnNext Click : " + generalException.ToString());
                _pageStatus.PageCompleteStatus = PageCompleteStatus.Failure;
                _pageStatus.MessageText = generalException.ToString();
            }
            return _pageStatus;
        }

        public void OnBackClick()
        {
            GeneralOptions.GetInstance().ApplicationName = txtApplicationName.Text;
            GeneralOptions.GetInstance().HostName = txtHostName.Text;
            GeneralOptions.GetInstance().Port = txtPortNumber.Text;
        }
    }
}