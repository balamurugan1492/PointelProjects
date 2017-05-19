
namespace ClickOnceDeployment.Data
{
    public enum OperationStatus
    {
        Success,
        Failure
    }

    public interface IMessage
    {
        OperationStatus OperationStatus
        {
            get;
        }

        string MessageText
        {
            get;
        }
    }
}
