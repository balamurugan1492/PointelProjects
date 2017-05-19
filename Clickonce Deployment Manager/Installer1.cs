using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Xml;

namespace ClickOnce_Deployment_Manager_64
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        protected override void OnAfterInstall(System.Collections.IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            if (this.Context.Parameters["targetdir"] != null && !string.IsNullOrEmpty(this.Context.Parameters["targetdir"]))
            {
                string FilePath = this.Context.Parameters["targetdir"] + @"Files\Agent.Interaction.Desktop.exe.config";

                XmlDocument configDoc = new XmlDocument();
                try
                {
                    configDoc.Load(FilePath);
                    if (configDoc.ChildNodes.Cast<XmlNode>().Any(x => x.Name == "configuration"))
                    {
                        XmlNode configNode = configDoc.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "configuration").Single<XmlNode>();
                        if (configNode.Cast<XmlNode>().Any(x => x.Name == "system.net"))
                        {
                            XmlNode netNode = configNode.Cast<XmlNode>().Where(x => x.Name == "system.net").Single<XmlNode>();
                            string Ipaddress = string.Empty;
                            var host = Dns.GetHostEntry(Dns.GetHostName());
                            foreach (var ip in host.AddressList)
                            {
                                if (ip.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    Ipaddress = ip.ToString();
                                    break;
                                }
                            }
                            //check for default proxy in system.net
                            if (netNode.Cast<XmlNode>().Any(x => x.Name == "defaultProxy"))
                            {
                                XmlNode defaultProxyNode = netNode.Cast<XmlNode>().Where(x => x.Name == "defaultProxy").Single<XmlNode>();
                                if (defaultProxyNode.ChildNodes.Cast<XmlNode>().Any(x => x.Name == "bypasslist"))
                                {
                                    XmlNode bypassnode = defaultProxyNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "bypasslist").Single<XmlNode>();
                                    XmlNode addNode = configDoc.CreateNode(XmlNodeType.Element, "add", null);
                                    XmlAttribute addAttbute = configDoc.CreateAttribute("address");
                                    addAttbute.Value = Ipaddress != null ? Ipaddress : "0";
                                    addNode.Attributes.Append(addAttbute);
                                    bypassnode.AppendChild(addNode);
                                    configDoc.Save(FilePath);
                                }
                                else
                                {
                                    XmlNode byPassNode = configDoc.CreateNode(XmlNodeType.Element, "bypasslist", null);
                                    XmlNode addNode = configDoc.CreateNode(XmlNodeType.Element, "add", null);
                                    XmlAttribute addAttbute = configDoc.CreateAttribute("address");
                                    addAttbute.Value = Ipaddress != null ? Ipaddress : "0";
                                    addNode.Attributes.Append(addAttbute);
                                    byPassNode.AppendChild(addNode);
                                    defaultProxyNode.AppendChild(byPassNode);
                                    configDoc.Save(FilePath);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Xml Exception" + ex.ToString());
                }
            }
        }
    }
}