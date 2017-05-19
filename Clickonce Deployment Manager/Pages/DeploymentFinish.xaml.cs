using ClickOnce_Deployment_Manager_64.Utils;
using System.Windows;
using System.Windows.Controls;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for DeploymentFinish.xaml
    /// </summary>
    public partial class DeploymentFinish : Page
    {
        public DeploymentFinish(string msg, bool enable)
        {
            InitializeComponent();
            ChbShowPublishePage.IsEnabled = enable;
            ChbShowPublishePage.IsChecked = enable;
            txtblkMessage.Text = msg;
        }

        private void ChbShowPublishePage_Unchecked(object sender, RoutedEventArgs e)
        {
            GeneralOptions.GetInstance().IsShowHtmlPage = false;
        }

        private void ChbShowPublishePage_Checked(object sender, RoutedEventArgs e)
        {
            GeneralOptions.GetInstance().IsShowHtmlPage = true;
        }
    }
}