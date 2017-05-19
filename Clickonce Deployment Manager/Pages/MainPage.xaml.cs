using ClickOnceDeployment.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Pointel.Logger.Core.ILog _logger;

        public MainPage()
        {
            InitializeComponent();
            _logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WelcomeMessage.Text = "Welcome to " + ApplicationDetails.Instance().ApplicationName.Replace('.', ' ') + " Deployment Manager, Version "
                    + ApplicationDetails.Instance().Version;
                this.GuideMessage.Text = "This Deployment Manager will guide you through the steps required to deploy " + ApplicationDetails.Instance().ApplicationName.Replace('.', ' ')
                    + " on your web server  as a ClickOnce package.";
                //Setup_Title.Text = ApplicationDetails.Instance().ApplicationName + " Deployment Manager";
            }
            catch (Exception generalException)
            {
                _logger.Error("Error at StartForm Window Load Event :" + generalException.ToString());
            }
        }
    }
}