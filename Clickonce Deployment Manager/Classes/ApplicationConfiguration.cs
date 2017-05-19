using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Clickonce_Deployment_Manager.Classes
{
   public class ApplicationConfiguration
    {
       private XmlDocument _xmlDocument = null;

       public void LoadXML(string path)
       {
           if (string.IsNullOrEmpty(path))
           {
               throw new Exception("File path is null or empty");
           }
           try
           {
               _xmlDocument = new XmlDocument();
               _xmlDocument.Load(path);
           }
           catch (Exception ex)
           {
               Logger.Error("ApplicationConfiguration.LoadXML() : " + ex.Message);
               MessageBox.Show(ex.Message);

           }
       }

       public string GetValue(string tagName)
       {
           if (string.IsNullOrEmpty(tagName))
           {
               throw new Exception("Tag name is null or empty");
           }
           try
           {
               XmlNodeList element = _xmlDocument.GetElementsByTagName(tagName);
               if (element != null)
               {
                   if (element.Count > 0)
                   {
                       return  element[0].InnerText;
                   }

               }
           }
           catch (Exception ex)
           {
               Logger.Error("ApplicationConfiguration.LoadXML() : " + ex.Message);
               MessageBox.Show(ex.Message);
           }

           throw new Exception("The tag " + tagName + " is not found");
       }

       public void SetValue(string tagName,string _value)
       {
           if (string.IsNullOrEmpty(tagName))
           {
               throw new Exception("Tag name is null or empty");
           }
           try
           {
               XmlNodeList element = _xmlDocument.GetElementsByTagName(tagName);
               if (element != null)
               {
                   if (element.Count > 0)
                   {
                       element[0].InnerText = _value;
                   }

               }
               else
               {
                   throw new Exception("The tag " + tagName + " is not found");
               }
           }
           catch (Exception ex)
           {
               Logger.Error("ApplicationConfiguration.LoadXML() : " + ex.Message);
               MessageBox.Show(ex.Message);
           }

           
       }

       public void SaveConfiguration(string path)
       {
            if (string.IsNullOrEmpty(path))
           {
               throw new Exception("File path is null or empty");
           }
           try
           {
               _xmlDocument.Save(path);
           }
           catch (Exception ex)
           {
               Logger.Error("SaveConfiguration" + ex.Message);
               MessageBox.Show(ex.Message);
           }
       }
       
    }
}
