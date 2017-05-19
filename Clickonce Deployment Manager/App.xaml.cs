using ClickOnceDeployment.Data;
using System;
using System.Windows;

namespace ClickOnce_Deployment_Manager_64
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Pointel.Logger.Core.ILog logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // MessageBox.Show("app started");
            try
            {
                ConfigureLogger();
                logger.Debug("Try to read configuration.");
                ApplicationDetails.Instance().ReadConfigurations();
                ApplicationDetails.Instance().DestinationPath = ApplicationDetails.Instance().DestinationPath.Replace(" ", String.Empty).TrimEnd('\\') + "V" + ApplicationDetails.Instance().Version + "\\";
                logger.Info("Application started successfully.");
            }
            catch (System.Exception generalException)
            {
                logger.Error("Error occurred as " + generalException.Message);
            }
        }

        private void ConfigureLogger()
        {
            try
            {
                LogInformation.Instance().ReadLogInformation();
                Pointel.Logger.Core.Logger.ConfigureLog4net(
                 LogInformation.Instance().LogMaxRollSize.ToString(), LogInformation.Instance().LogFileSize,
                 LogInformation.Instance().LogRollStyle, LogInformation.Instance().ConversionPattern,
                 LogInformation.Instance().LogFileName,
                 LogInformation.Instance().LogFilterLevel, LogInformation.Instance().LogLevelToFilter,
                 LogInformation.Instance().DatePattern);
                LogInformation.Instance().Dispose();
                LogInformation.DisposeObject();
            }
            catch (System.Exception generalException)
            {
                logger.Error("Error occurred as " + generalException.Message);
            }
        }
    }
}