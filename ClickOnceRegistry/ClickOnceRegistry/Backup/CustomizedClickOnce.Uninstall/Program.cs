using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CustomizedClickOnce.Common;
using CustomizedClickOnce.Uninstall.Properties;
using System.Security.Principal;
using Microsoft.Win32;
using System.ComponentModel;

namespace CustomizedClickOnce.Uninstall
{
    static class Program
    {
        private static Mutex instanceMutex;

        [STAThread]
        static void Main()
        {
            try
            {
                bool createdNew;
                instanceMutex = new Mutex(true, @"Local\" + Assembly.GetExecutingAssembly().GetType().GUID, out createdNew);
                if (!createdNew)
                {
                    instanceMutex = null;
                    return;
                }

                RemoveFiles();

                var clickOnceHelper = new ClickOnceHelper(Globals.PublisherName, Globals.ProductName);
                clickOnceHelper.Uninstall();

                //Delete all files from publisher folder and folder itself on uninstall
                var publisherFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Globals.PublisherName);
                if (Directory.Exists(publisherFolder))
                    Directory.Delete(publisherFolder, true);
                

                ReleaseMutex();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static void RemoveFiles()
        {
            try
            {
                WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
                string driverName = "vddrvn.dll";

                if (hasAdministrativeRight)
                {
                    MessageBox.Show("Has Admin previlege");
                    try
                    {
                        string icaRegistry;
                        string targetPath;
                        if (System.Environment.Is64BitOperatingSystem)
                        {
                            icaRegistry = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\ICA 3.0";
                            Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\POINTEL");
                            targetPath = @"C:\Program Files (x86)\Citrix\ICA Client";
                        }
                        else
                        {
                            icaRegistry = @"HKEY_LOCAL_MACHINE\SOFTWARE\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\ICA 3.0";
                            Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Citrix\ICA Client\Engine\Configuration\Advanced\Modules\POINTEL");
                            targetPath = @"C:\Program Files\Citrix\ICA Client";

                        }

                        Registry.SetValue(icaRegistry, "VirtualDriverEx", "");

                        if (System.IO.Directory.Exists(targetPath))
                        {
                            string destFile = System.IO.Path.Combine(targetPath, driverName);
                            System.IO.File.Delete(destFile);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error in deleting files : "+ex.Message);
                    }
                }
                else
                {

                    MessageBox.Show("No Admin previlege"); 

                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.Verb = "runas";
                    processInfo.FileName = @"C:\Windows\System32\appwiz.cpl";
                    try
                    {
                        Process.Start(processInfo);
                        //Process.GetCurrentProcess().Kill();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("Error : " + ex.Message);
                        //Do nothing. Probably the user canceled the UAC window
                    }
                    MessageBox.Show("Doesn't have admin previleges");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured : " + ex.Message);
            }
        }

        private static void ReleaseMutex()
        {
            if (instanceMutex == null)
                return;
            instanceMutex.ReleaseMutex();
            instanceMutex.Close();
            instanceMutex = null;
        }
    }

}
