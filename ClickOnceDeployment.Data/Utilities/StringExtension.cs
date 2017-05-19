
namespace ClickOnceDeployment.Data.Utilities
{
    public static class StringExtension
    {
        public static string GetValue(this string value)
        {
            return string.IsNullOrEmpty(value) ? "null" : value;
        }
    }
}
