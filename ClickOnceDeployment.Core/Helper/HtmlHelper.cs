using System.IO;
using System.Text;

namespace ClickOnceDeployment.Core.Helper
{
    public class HtmlHelper
    {
        public void GenerateHtmlPage(string filePath, string applicationName, string version, string companyName, string imagePath,
            string deploymentURl, string setupURL)
        {
            string content = @"<html><head><link rel=""shortcut icon"" type=""image/x-icon"" href=""Application Files/" + imagePath + @"/pointel_logo.png" + @""" /><title>" + applicationName + @"</title></head> <style type=""text/css"">
            *{
            margin: 0;
            }
            html, body {
            height: 100%;
            }
            .wrapper {
            min-height: 95.5%;
            height: auto !important;
            height: 100%;
            margin: 0 auto -2em 0;
            }

            .footer
            {
	            background-color:#EBEBEB;
                text-align:center;
                height: 2em;
                font-size:12pt;
	            vertical-align:middle;
            }
            a
            {
	                color:#996600;
            }
            table
            {
	            margin:30px;
            }
            td:first-child
            {
	            font-weight:bold;
	            padding:10px;
	            color:#003300;
            }
            ul
            {
	            font-weight:normal;
	            color:#000000;
	            margin:5px 10px 0px 10px;
            }
            .content
            {
                background-color: #46a1bc;
                height: 80px;
                border-bottom: solid 2px #F3EDE0;
                color: #ffffff;
                font-family: Arial, Helvetica, sans-serif;
                font-size: 2em;
               font-weight: 300;
               overflow: hidden;
            }
                </style>
            </head>
            <body>
            <div class=""wrapper"">
                <div class=""content"">

	            <p  style=""padding-left:32%;padding-top:15px;vertical-align:middle;"">Agent Interaction Desktop 	</p>
	            </div>

                <table  style=""font-family:Arial, Helvetica, sans-serif;"">
                <tr>
                <td>Name : </td>
                <td>" + applicationName.Replace('.', ' ') + @"</td>
                </tr>

                <tr>
                <td>Version : </td>
                <td>" + version + @"</td>
                </tr>

                <tr>
                <td>Publisher : </td>
                <td>" + companyName + @"</td>
                </tr>
                <tr>
                <td colspan=""2"">
                <br />
                The following prerequisites are required:<br />
            </br>
                <ul >
                <li> Microsoft .NET Framework 4.5 (x86 and x64) </li>
                </br>
               <!--<li> Windows Installer 3.1 </li>
               </br>-->
                </ul>
                </td>
                </tr>
                </table>
                <div style=""margin:20px;font-family:Arial, Helvetica, sans-serif;"">
                    If these components are already installed, you can <a href='" + deploymentURl + @"'>launch</a>   the application now. Otherwise, click the button below to install the prerequisites and run the application.
                    <br /><br/>
<a href=" + setupURL + @" style=""padding:5px 10px; text-decoration:none; border:solid 1px #000000;background-color:#EBEBEB"">Install</a>
                </div>
                </div>
                <div class=""footer"" style=""font-family:Arial, Helvetica, sans-serif;"">
                    <a href=""http://msdn.microsoft.com/library/t71a733d.aspx"">ClickOnce and .Net Framework Resources</a>
                </div>

            </body>
            </html>";
            //in content div to image
            // <img src=""Application Files/" + imagePath + @"/pointel_logo.png"" alt=""logo"" align=""left"" style=""width:50px;height:50px;padding-left:10px; padding-top:11px;""/>
            // <a href=" +setupURL+ @" style=""padding:5px 10px; text-decoration:none; border:solid 1px #000000;background-color:#EBEBEB"">Install</a>
            using (BinaryWriter bw = new BinaryWriter(File.Create(filePath + @"install.html")))
            {
                byte[] con = ASCIIEncoding.ASCII.GetBytes(content);
                bw.Write(con, 0, con.Length);
            }
        }

        public void ShowHtmlPage(string filePath)
        {
            System.Diagnostics.Process.Start(filePath);
        }
    }
}