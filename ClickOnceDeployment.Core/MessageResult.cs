
using ClickOnceDeployment.Data;
namespace ClickOnceDeployment.Core
{
    internal class MessageResult : IMessage
    {
        private OperationStatus _operationStatus;

        private string _messageText;

        public OperationStatus OperationStatus
        {
            get
            {
                return _operationStatus;
            }
        }

        public string MessageText
        {
            get
            {
                return _messageText;
            }
        }

        internal  void SetMessage(string message,bool isSuccess=false)
        {
            _messageText = message;
            
            if (!isSuccess)
                _operationStatus = OperationStatus.Failure;
        }

    }
}
