using System;
using System.IO;
using System.Windows.Data;

namespace ClickOnce_Deployment_Manager_64.Classes
{
    internal class DisplayValueConverter : IValueConverter
    {
        private string temp;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString().Contains(@"\"))
            {
                temp = value.ToString();
                temp = Path.GetFileName(temp);
                return temp;
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}