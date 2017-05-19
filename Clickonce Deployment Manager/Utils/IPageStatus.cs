namespace ClickOnce_Deployment_Manager_64.Utils
{
    public interface IPageStatus
    {
        PageCompleteStatus PageCompleteStatus
        {
            get;
            set;
        }

        string MessageText
        {
            get;
            set;
        }
    }

    public enum PageCompleteStatus
    {
        Success,
        Failure,
        Empty,
        Invalid
    }
}