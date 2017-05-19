using System;
using System.Configuration;

namespace ClickOnceDeployment.Data
{
    public class LogInformation : System.IDisposable
    {
        #region DataMembers

        private static LogInformation _objLogInformation;

        #endregion DataMembers

        #region Properties

        public string LogFileName { get; private set; }
        public int LogMaxRollSize { get; private set; }
        public string LogFileSize { get; private set; }
        public string LogRollStyle { get; private set; }
        public string ConversionPattern { get; private set; }
        public string LogFilterLevel { get; private set; }
        public string LogLevelToFilter { get; private set; }
        public string DatePattern { get; private set; }

        #endregion Properties

        #region Constructor

        private LogInformation()
        {
        }

        #endregion Constructor

        #region Single Instance

        public static LogInformation Instance()
        {
            if (_objLogInformation == null)
                _objLogInformation = new LogInformation();

            return _objLogInformation;
        }

        #endregion Single Instance

        #region Methods

        public static void DisposeObject()
        {
            _objLogInformation = null;
        }

        public void ReadLogInformation()
        {
            try
            {
                if (ConfigurationManager.AppSettings != null)
                {
                    foreach (string keyName in ConfigurationManager.AppSettings.AllKeys)
                    {
                        switch (keyName)
                        {
                            case "LogFileName":
                                LogFileName = ConfigurationManager.AppSettings[keyName].ToString();
                                break;

                            case "ConversionPattern":
                                ConversionPattern = ConfigurationManager.AppSettings[keyName].ToString();
                                break;

                            case "DatePattern":
                                DatePattern = ConfigurationManager.AppSettings[keyName].ToString();
                                break;

                            case "LogFilterLevel":
                                LogFilterLevel = ConfigurationManager.AppSettings[keyName].ToString();
                                break;

                            case "LogLevelToFilter":
                                LogLevelToFilter = ConfigurationManager.AppSettings[keyName].ToString();
                                break;

                            case "LogFileSize":
                                LogFileSize = ConfigurationManager.AppSettings[keyName].ToString();
                                break;

                            case "LogMaxRollSize":
                                int logMaxRoolSize;

                                if (!int.TryParse(ConfigurationManager.AppSettings[keyName].ToString(), out logMaxRoolSize))
                                    logMaxRoolSize = 10;
                                LogMaxRollSize = logMaxRoolSize;
                                break;

                            case "LogRollStyle":
                                LogRollStyle = ConfigurationManager.AppSettings[keyName].ToString();
                                break;
                        }
                    }
                }
                //else
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            LogFileName = LogFileSize = LogRollStyle = ConversionPattern = LogFileName = LogFilterLevel = LogLevelToFilter = DatePattern = null;
        }

        #endregion Methods
    }
}