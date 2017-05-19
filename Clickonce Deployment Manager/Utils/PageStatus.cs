namespace ClickOnce_Deployment_Manager_64.Utils
{
    public class PageStatus : IPageStatus
    {
        private PageCompleteStatus _pageCompleteStatus;
        private string _messageText;

        public PageCompleteStatus PageCompleteStatus
        {
            get
            {
                return _pageCompleteStatus;
            }
            set
            {
                _pageCompleteStatus = value;
            }
        }

        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                _messageText = value;
            }
        }
    }
}