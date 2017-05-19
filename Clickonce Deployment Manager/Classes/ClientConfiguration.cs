using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Clickonce_Deployment_Manager.Classes
{
    public static class ClientConfiguration
    {
        private static XmlDocument _xmlDoc = null;

        public static string ApplicationName
        {
            get;
            set;
        }
        public static string Host
        {
            get;
            set;
        }
        public static int Port
        {
            get;
            set;
        }

        public static void ReadClientConfiguration(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new Exception("The path name is Null or Empty");
                }
                _xmlDoc = new XmlDocument();
                Logger.Info("Try to Read Configuration setting");
                _xmlDoc.Load(path);
                if (_xmlDoc.ChildNodes.Count > 0)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error(" ClientConfiguration.ReadClientConfiguration : " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
