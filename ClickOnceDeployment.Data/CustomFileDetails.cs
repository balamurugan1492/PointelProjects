using System.Collections.Generic;

namespace ClickOnceDeployment.Data
{
    public class CustomFileDetails
    {
        private Pointel.Logger.Core.ILog logger;
        private List<string> _subFolderList = new List<string>();
        public string SelectedFolder;
        public string NewFolderName;
        public bool IsnewFolderAdd = false;

        #region Properties

        public List<string> Subfolders
        {
            get
            {
                return _subFolderList;
            }
            set { _subFolderList = value; }
        }

        #endregion Properties

        #region Constructor

        private CustomFileDetails()
        {
            logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnceDeployment");
        }

        #endregion Constructor

        #region SingleInstance

        private static CustomFileDetails _objCustomFileDetails;

        public static CustomFileDetails Instance()
        {
            if (_objCustomFileDetails == null)
                _objCustomFileDetails = new CustomFileDetails();

            return _objCustomFileDetails;
        }

        #endregion SingleInstance
    }
}