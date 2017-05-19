using ClickOnce_Deployment_Manager_64.Utils;
using ClickOnceDeployment.Core;
using ClickOnceDeployment.Data;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for Progress.xaml
    /// </summary>
    public delegate void ProgressBarPageCompleted(string errorMessage, bool enable);

    public partial class Progress : Page
    {
        private Pointel.Logger.Core.ILog _logger;

        public static event ProgressBarPageCompleted progressBarComplete;

        public Progress()
        {
            InitializeComponent();
            _logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");
        }

        private void WndProgress_DeploymentFinishEvent(OperationStatus status)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    if (status != OperationStatus.Success && progressBarComplete != null)
                    {
                        progressBarComplete("Some problem occurred while deploy the application.", false);
                    }
                    //objDeploymentFinished.ShowFinishWizard("Some problem occurred while deploy the application.", false);
                }
                catch (Exception generalExcption)
                {
                    _logger.Error("Error occurred as " + generalExcption.Message);
                    progressBarComplete("Error occurred as " + generalExcption.Message, false);
                }
            }));
        }

        private void WndProgress_DeploymentStateEvent(string message, int finishedState)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    txtBlkMessage.Text = message;
                    DeployProgress.Value = finishedState;
                }
                catch (Exception generalExcption)
                {
                    _logger.Error("Error occurred as " + generalExcption.Message);
                    progressBarComplete("Error occurred as " + generalExcption.Message, false);
                }
            }));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClickOnceManager.Instance().DeploymentStateEvent += new DeploymentStateHandler(WndProgress_DeploymentStateEvent);
            ClickOnceManager.Instance().DeploymentFinishEvent += new DeploymentFinishHandler(WndProgress_DeploymentFinishEvent);

            Thread deploymentThread = new Thread(() =>
            {
                try
                {
                    IMessage message = ClickOnceManager.Instance().FinishDeployment(GeneralOptions.GetInstance().HostName, GeneralOptions.GetInstance().Port, GeneralOptions.GetInstance().ApplicationName, GeneralOptions.GetInstance().SubVersion);
                    if (message.OperationStatus == OperationStatus.Failure)
                    {
                        try
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                _logger.Error("Error at WndProgress Load Event :" + message.MessageText);
                                MessageBox.ExitMessageBox.ShowMessage(message.MessageText, "Clickonce deployment error", MessageBox.ExitMessageBox.ButtonType.Ok);
                                MessageBox.ExitMessageBox.ShowMessage("The application will be exit.", "Clickonce Deployment Exit Confirmation", MessageBox.ExitMessageBox.ButtonType.Ok);

                                Application.Current.Shutdown();
                            }));
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        progressBarComplete(null, true);
                    }
                }
                catch (Exception generalException)
                {
                    _logger.Error("Error at WndProgress Load Event :" + generalException.Message);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        progressBarComplete("Error occured During Installation :" + generalException.Message, false);
                        //WizardHandler.Instance().ShowSetupFinishedWizard("Error occured During Installation :" + generalException.Message);
                    }));
                }
            });
            deploymentThread.SetApartmentState(ApartmentState.STA);
            deploymentThread.IsBackground = true;
            deploymentThread.Start();
        }
    }
}