using ClickOnce_Deployment_Manager_64.Utils;
using ClickOnceDeployment.Core;
using ClickOnceDeployment.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for DestinationFolder.xaml
    /// </summary>
    public partial class DestinationFolder : Page
    {
        private static PageStatus _pageStatus = new PageStatus();

        public DestinationFolder()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtDestinationFolder.Text = ApplicationDetails.Instance().DestinationPath;
        }

        public PageStatus OnNextClick()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtDestinationFolder.Text) && !string.IsNullOrWhiteSpace(txtDestinationFolder.Text))
                {
                    if (txtDestinationFolder.Text.StartsWith(ClickOnceManager.Instance().IISPath, StringComparison.OrdinalIgnoreCase))
                    {
                        string lastletter = txtDestinationFolder.Text.Substring(txtDestinationFolder.Text.Length - 1);
                        if (lastletter != @"\")
                            ApplicationDetails.Instance().DestinationPath = txtDestinationFolder.Text.Replace(" ", String.Empty) + @"\";
                        else
                            ApplicationDetails.Instance().DestinationPath = txtDestinationFolder.Text.Replace(" ", String.Empty);
                        _pageStatus.PageCompleteStatus = PageCompleteStatus.Success;
                        return _pageStatus;
                    }
                    else
                    {
                        _pageStatus.PageCompleteStatus = PageCompleteStatus.Invalid;
                        return _pageStatus;
                    }
                }
                else
                {
                    _pageStatus.PageCompleteStatus = PageCompleteStatus.Empty;
                    return _pageStatus;
                }
            }
            catch (Exception generalException)
            {
                _pageStatus.PageCompleteStatus = PageCompleteStatus.Failure;
                _pageStatus.MessageText = generalException.ToString();
                return _pageStatus;
            }
        }
    }
}