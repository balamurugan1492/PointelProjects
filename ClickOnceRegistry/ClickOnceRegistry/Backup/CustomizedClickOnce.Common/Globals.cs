namespace CustomizedClickOnce.Common
{

    //These parameters should be read from some config in real applciation
    //Here they're just hard coded
    public class Globals
    {
        public static string PublisherName
        {
            get { return "Pointel"; }
        }

        public static string ProductName
        {
            get { return "ClickOnce"; }
        }
    }
}
