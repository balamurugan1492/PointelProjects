/* ===========================================
 * ClickOnceDeployment.Data
 * ===========================================
 * Project     : Clickonce Deployment
 * Created on  : 25-11-2015
 * Author      : Sakthikumar
 * Owner       : Pointel Solution
 * ==========================================
 */

namespace ClickOnceDeployment.Data
{
    using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

    public class DeploymentManifestInfo
    {
        public string ProductName
        {
            get;
            set;
        }

        public string PublisherName
        {
            get;
            set;
        }

        public string ProductVersion
        {
            get;
            set;
        }

        public string ClickonceProductVerion
        {
            get;
            set;
        }

        public bool AutoUpdateEnabled
        {
            get;
            set;
        }

        public UpdateMode AppUpdateMode
        {
            get;
            set;
        }

        public int UpdateInterval
        {
            get;
            set;
        }

        public UpdateUnit UpdateUnit
        {
            get;
            set;
        }

        public string DeploymentURL
        {
            get;
            set;
        }

        //added by balamurugan
        public bool DesktopShortcut
        {
            get;
            set;
        }
    }
}