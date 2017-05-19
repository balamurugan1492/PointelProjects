using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using CustomizedClickOnce.Common;

namespace ClickOnceRegistry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateRegistry();
        }


        void UpdateRegistry()
        {
            try
            {
                WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
                string keyName = ConfigurationManager.AppSettings.Get("keyName");                
                string driverName = ConfigurationManager.AppSettings.Get("virtualDriver");

                if (hasAdministrativeRight)
                {
                    string icaRegistry;
                    string modulesRegistry;
                    string targetPath;
                    if(System.Environment.Is64BitOperatingSystem)
                    {
                        icaRegistry = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\ICA 3.0";
                        modulesRegistry = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\" + keyName;
                        targetPath = @"C:\Program Files (x86)\Citrix\ICA Client";
                    }
                    else
                    {
                        icaRegistry = @"HKEY_LOCAL_MACHINE\SOFTWARE\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\ICA 3.0";
                        modulesRegistry = @"HKEY_LOCAL_MACHINE\SOFTWARE\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\" + keyName;
                        targetPath = @"C:\Program Files\Citrix\ICA Client";

                    }

                    Registry.SetValue(icaRegistry, "VirtualDriverEx", keyName);
                    //MessageBox.Show("Registry Updated");
                    Registry.SetValue(modulesRegistry, "DriverNameWin32", driverName);

                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }

                    string sourceFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), driverName);
                    string destFile = System.IO.Path.Combine(targetPath, driverName);
                    System.IO.File.Copy(sourceFile, destFile, true);                  

                   
                }
                else if (MessageBox.Show("Are want start the application in admin right?", "Confirm", MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.Verb = "runas";
                    processInfo.FileName = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ClickOnceRegistry.exe";
                    try
                    {
                        Process.Start(processInfo);
                        Process.GetCurrentProcess().Kill();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("Error : " + ex.Message);
                        //Do nothing. Probably the user canceled the UAC window
                    }
                }      
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured : " + ex.Message);
            }
        }
    }
}
