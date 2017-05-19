namespace ClickOnce_Deployment_Manager_64.Utils
{
    internal class GeneralOptions
    {
        public bool IsCustomFileChecked
        {
            get;
            set;
        }

        public bool IsShowHtmlPage
        {
            get;
            set;
        }

        public bool IsCreateDesktopChecked
        {
            get;
            set;
        }

        public string ApplicationName
        {
            get;
            set;
        }

        public string HostName
        {
            get;
            set;
        }

        public string Port
        {
            get;
            set;
        }

        public string SubVersion
        {
            get;
            set;
        }

        #region SingleInstance

        private static GeneralOptions _objGeneralOptions;

        public static GeneralOptions GetInstance()
        {
            if (_objGeneralOptions == null)
                _objGeneralOptions = new GeneralOptions();

            return _objGeneralOptions;
        }

        #endregion SingleInstance
    }
}