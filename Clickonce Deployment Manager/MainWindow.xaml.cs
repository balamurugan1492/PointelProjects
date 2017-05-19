using ClickOnce_Deployment_Manager_64.Pages;
using ClickOnce_Deployment_Manager_64.Utils;
using ClickOnceDeployment.Core;
using ClickOnceDeployment.Data;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ClickOnce_Deployment_Manager_64
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Pointel.Logger.Core.ILog _logger;
        private string _pageName;

        public MainWindow()
        {
            InitializeComponent();
            _logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");
            Progress.progressBarComplete += GeneralOptions_progressBarComplete;
        }

        private void GeneralOptions_progressBarComplete(string errorMessage, bool enable)
        {
            Application.Current.Dispatcher.Invoke(() =>
               {
                   if (errorMessage == null)
                   {
                       mainFrame.Content = new DeploymentFinish("The Manifest File ('" + ApplicationDetails.Instance().AppOriginalName
                           + ".exe.manifest')" + " has been validated successfully.\n\nThe Manifest File ('" + ApplicationDetails.Instance().AppOriginalName
                           + ".application')" + " has been validated successfully.", enable);
                       _logger.Info("DeploymentFinish Page is Opened...");
                       this.BtnBack.Visibility = Visibility.Collapsed;
                       this.BtnNext.Visibility = Visibility.Visible;
                       this.BtnNext.Content = "Finish";
                   }
                   else
                   {
                       mainFrame.Content = new DeploymentFinish(errorMessage, enable);
                       _logger.Info("DeploymentFinish Page is Opened...");
                       _logger.Error(errorMessage);
                       this.BtnBack.Visibility = Visibility.Collapsed;
                       this.BtnNext.Visibility = Visibility.Visible;
                       this.BtnNext.Content = "Finish";
                   }
               });
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("MainWindow-Back Button is clicked..");
            try
            {
                switch (_pageName)
                {
                    case "DestinationFolder":
                        {
                            mainFrame.Content = new MainPage();
                            _logger.Info("MainPage is Opened...");
                            this.BtnBack.Visibility = Visibility.Collapsed;
                        }
                        break;

                    case "PackageInfo":
                        {
                            mainFrame.Content = new DestinationFolder();
                            _logger.Info("DestinationFolder Page is Opened...");
                        }
                        break;

                    case "CustomFiles":
                        {
                            mainFrame.Content = new PackageInfo();
                            _logger.Info("PackageInfo Page is Opened...");
                        }
                        break;

                    case "ClientConfiguration":
                        {
                            (mainFrame.Content as ClientConfiguration).OnBackClick();
                            if (GeneralOptions.GetInstance().IsCustomFileChecked)
                            {
                                mainFrame.Content = new CustomFiles();
                                _logger.Info("CustomFiles Page is Opened...");
                            }
                            else
                            {
                                mainFrame.Content = new PackageInfo();
                                _logger.Info("PackageInfo Page is Opened...");
                            }
                        }
                        break;

                    case "ReadyToBuild":
                        {
                            mainFrame.Content = new ClientConfiguration();
                            _logger.Info("ClientConfiguration Page is Opened...");
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception generalException)
            {
                _logger.Error("StartForm NextClick Event :" + generalException.ToString());
                MessageBox.ExitMessageBox.ShowMessage(generalException.Message, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                mainFrame.Content = new DeploymentFinish("Error occured During Installation :" + generalException.Message, false);
                _logger.Info("DeploymentFinish Page is Opened...");
                _logger.Error(generalException.Message);
                this.BtnBack.Visibility = Visibility.Collapsed;
                this.BtnNext.Content = "Finish";
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("MainWindow.Next() Button is clicked..");
            try
            {
                if ((BtnNext.Content as string) != "Finish")
                {
                    switch (_pageName)
                    {
                        case "MainPage":
                            IMessage result = ClickOnceManager.Instance().Initialize();
                            if (result.OperationStatus == OperationStatus.Success)
                            {
                                mainFrame.Content = new DestinationFolder();
                                _logger.Info("DestinationFolder is Opened...");
                                this.BtnBack.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                mainFrame.Content = new DeploymentFinish(result.MessageText, false);
                                _logger.Info("DeploymentFinish Page is Opened...");
                                _logger.Error(result.MessageText);
                                this.BtnBack.Visibility = Visibility.Collapsed;
                                this.BtnNext.Content = "Finish";
                            }
                            break;

                        case "DestinationFolder":
                            {
                                IPageStatus pageStatus = (mainFrame.Content as DestinationFolder).OnNextClick();
                                switch (pageStatus.PageCompleteStatus)
                                {
                                    case PageCompleteStatus.Success:
                                        mainFrame.Content = new PackageInfo();
                                        _logger.Info("PackageInfo Page is Opened...");
                                        break;

                                    case PageCompleteStatus.Invalid:
                                        MessageBox.ExitMessageBox.ShowMessage("Destination directory must be inside IIS Folder location. (" + ClickOnceManager.Instance().IISPath + ")", "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        break;

                                    case PageCompleteStatus.Empty:
                                        MessageBox.ExitMessageBox.ShowMessage("Please Enter Destination Folder Path.", "Field required", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        break;

                                    case PageCompleteStatus.Failure:
                                        MessageBox.ExitMessageBox.ShowMessage(pageStatus.MessageText, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        mainFrame.Content = new DeploymentFinish("Error occured During Installation :" + pageStatus.MessageText, false);
                                        _logger.Info("DeploymentFinish Page is Opened...");
                                        _logger.Error(pageStatus.MessageText);
                                        this.BtnBack.Visibility = Visibility.Collapsed;
                                        this.BtnNext.Content = "Finish";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        case "PackageInfo":
                            {
                                IPageStatus pageStatus = (mainFrame.Content as PackageInfo).OnNextClick();
                                switch (pageStatus.PageCompleteStatus)
                                {
                                    case PageCompleteStatus.Success:
                                        if (GeneralOptions.GetInstance().IsCustomFileChecked)
                                        {
                                            mainFrame.Content = new CustomFiles();
                                            _logger.Info("CustomFiles Page is Opened..");
                                        }
                                        else
                                            mainFrame.Content = new ClientConfiguration();
                                        _logger.Info("ClientConfiguration Page is Opened..");
                                        break;

                                    case PageCompleteStatus.Invalid:
                                        MessageBox.ExitMessageBox.ShowMessage(pageStatus.MessageText, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        break;

                                    case PageCompleteStatus.Empty:
                                        MessageBox.ExitMessageBox.ShowMessage(pageStatus.MessageText, "Field required", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        break;

                                    case PageCompleteStatus.Failure:
                                        MessageBox.ExitMessageBox.ShowMessage(pageStatus.MessageText, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        mainFrame.Content = new DeploymentFinish("Error occured During Installation :" + pageStatus.MessageText, false);
                                        _logger.Info("DeploymentFinish Page is Opened...");
                                        _logger.Error(pageStatus.MessageText);
                                        this.BtnBack.Visibility = Visibility.Collapsed;
                                        this.BtnNext.Content = "Finish";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        case "CustomFiles":
                            {
                                mainFrame.Content = new ClientConfiguration();
                                _logger.Info("ClientConfiguration Page is Opened..");
                            }
                            break;

                        case "ClientConfiguration":
                            {
                                IPageStatus pageStatus = (mainFrame.Content as ClientConfiguration).OnNextClick();
                                switch (pageStatus.PageCompleteStatus)
                                {
                                    case PageCompleteStatus.Success:
                                        mainFrame.Content = new ReadyToBuild();
                                        _logger.Info("ReadyToBuild Page is Opened..");
                                        break;

                                    case PageCompleteStatus.Empty:
                                        MessageBox.ExitMessageBox.ShowMessage(pageStatus.MessageText, "Field required", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        break;

                                    case PageCompleteStatus.Failure:
                                        MessageBox.ExitMessageBox.ShowMessage(pageStatus.MessageText, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                                        mainFrame.Content = new DeploymentFinish("Error occured During Installation :" + pageStatus.MessageText, false);
                                        _logger.Info("DeploymentFinish Page is Opened...");
                                        _logger.Error(pageStatus.MessageText);
                                        this.BtnBack.Visibility = Visibility.Collapsed;
                                        this.BtnNext.Content = "Finish";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        case "ReadyToBuild":
                            {
                                mainFrame.Content = new Progress();
                                _logger.Info("Progress Page is Opened..");
                                this.BtnBack.Visibility = Visibility.Collapsed;
                                this.BtnNext.Visibility = Visibility.Collapsed;
                                this.LblExitButton.Visibility = Visibility.Collapsed;
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    //handle finish deployment
                    try
                    {
                        //code to code the wizard and view published page
                        if (GeneralOptions.GetInstance().IsShowHtmlPage)
                        {
                            string webPagepath1 = ClickOnceManager.Instance().DeploymentManifestInfo.DeploymentURL.Substring(0,
                                ClickOnceManager.Instance().DeploymentManifestInfo.DeploymentURL.LastIndexOf('/')) + "/install.html";
                            System.Diagnostics.Process.Start(webPagepath1);
                        }
                        _logger.Debug("Deployment Completed Successfully");
                        _logger.Debug("Application Exit");
                        _logger.Debug("-------------End--------------");
                        Application.Current.Shutdown();
                    }
                    catch (Exception generalException)
                    {
                        _logger.Error("Finished_Click() : " + generalException.Message);
                        MessageBox.ExitMessageBox.ShowMessage(generalException.Message, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                        mainFrame.Content = new DeploymentFinish("Error occured During Installation :" + generalException.Message, false);
                        _logger.Info("DeploymentFinish Page is Opened...");
                        _logger.Error(generalException.Message);
                        this.BtnBack.Visibility = Visibility.Collapsed;
                        this.BtnNext.Content = "Finish";
                    }
                }
            }
            catch (Exception generalException)
            {
                _logger.Error("StartForm NextClick Event :" + generalException.ToString());
                MessageBox.ExitMessageBox.ShowMessage(generalException.ToString(), "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                mainFrame.Content = new DeploymentFinish("Error occured During Installation :" + generalException.ToString(), false);
                _logger.Error(generalException.Message);
                _logger.Info("DeploymentFinish Page is Opened...");
                this.BtnBack.Visibility = Visibility.Collapsed;
                this.BtnNext.Visibility = Visibility.Visible;
                this.BtnNext.Content = "Finish";
            }
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            imagClose.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Custom Close selected.png",
                   UriKind.Absolute));
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            imagClose.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Custom Close.png",
                   UriKind.Absolute));
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.ExitMessageBox.ShowMessage("Are you sure that you want to exit " + ApplicationDetails.Instance().ApplicationName.Replace('.', ' ') + " Deployment Manager ?", "Clickonce Deployment Exit Confirmation", MessageBox.ExitMessageBox.ButtonType.YesorNo))
                Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainFrame.Content == null)
                    mainFrame.Content = new MainPage();
            }
            catch (Exception ex)
            {
                _logger.Error("Error Occured On Windows Loaded :" + ex.ToString());
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.ExitMessageBox.ShowMessage("Are you sure that you want to exit " + ApplicationDetails.Instance().ApplicationName + " Deployment Manager ?", "Clickonce Deployment Exit Confirmation", MessageBox.ExitMessageBox.ButtonType.YesorNo))
                Application.Current.Shutdown();
        }

        private void mainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            _pageName = e.Content.GetType().Name;
        }
    }
}