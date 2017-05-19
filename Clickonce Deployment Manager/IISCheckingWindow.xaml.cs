using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Clickonce_Deployment_Manager.Classes;
using System.ComponentModel;
using System.Windows.Threading;
using System.IO;

namespace Clickonce_Deployment_Manager
{
    /// <summary>
    /// Interaction logic for IISCheckingWindow.xaml
    /// </summary>
    public partial class IISCheckingWindow : Window
    {
               
        public IISCheckingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             BackgroundWorker worker = new BackgroundWorker();
             

            try
            {

                    #region code

                worker.DoWork += delegate(object s, DoWorkEventArgs args)
                {

          
                IISManager iisManager=new IISManager();

                if (iisManager.IsIISInstalled())
                {

                    StartWindow.IsIISAvailable = true;
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Window>(CloseWindow), this);

                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Window>(CloseWindow), this);
                    Logger.Error("IIS Server Not Found");
                    this.Hide();
                    MessageBox.Show("IIS Server Not Found");
                    ApplicationDetails.DeploymentFinishedForm.appMessage = "Error occured During Installation ";
                    ApplicationDetails.DeploymentFinishedForm.PublishPage = false;
                    ApplicationDetails.DeploymentFinishedForm.Show();
                }

                };

                     #endregion
            }
            catch (Exception ex)
            {
                Logger.Error("Error at WndProgress Load Event :" + ex.Message);
                this.Hide();
                MessageBox.Show(ex.Message);
                ApplicationDetails.DeploymentFinishedForm.appMessage = "Error occured During Installation :" + ex.Message;
                ApplicationDetails.DeploymentFinishedForm.PublishPage = false;
                ApplicationDetails.DeploymentFinishedForm.Show();
            }

            worker.RunWorkerAsync();
            Logger.Info("WndProgress Load Event : Exit");

           
           
        }
        private static void CloseWindow(Window window)
        {
            try
            {
                StartWindow.IsChecked = true;
                window.Close();
               
            }
            catch (Exception ex)
            {
                Logger.Error("WndProgress.CloseWindow(): " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        
    }
}
