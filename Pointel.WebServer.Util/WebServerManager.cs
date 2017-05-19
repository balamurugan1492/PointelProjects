/* ==============================================
 * Pointel.WebServer.Util
 * ==============================================
 * Project   : Clickonce Deployment
 * Created on: 25-11-2015
 * Author    : Sakthikumar
 * Owner     : Pointel solution
 * =============================================
 */

using Microsoft.Win32;
using System;

namespace Pointel.WebServer.Util
{
    #region Enum Webserver

    public enum WebServer { IIS, Tomcat }

    #endregion Enum Webserver

    public class WebServerManager : System.IDisposable
    {
        #region Data Member

        private WebServer _webServer;

        #endregion Data Member

        #region Property

        public WebServer WorkingWebServer
        {
            get
            {
                return _webServer;
            }
        }

        #endregion Property

        #region Methods

        public bool IsIISInstalled()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\InetStp");
                if (key != null)
                    return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public string GetIISFolderPath()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\InetStp");
                if (key != null)
                    return key.GetValue("PathWWWRoot").ToString();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public void Dispose()
        {
        }

        #endregion Methods
    }
}