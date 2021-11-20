using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EzHRMApp.Core.Converter
{
    public class IntToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(int))
            {
                return ((int)value).ToString();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(int) && value.GetType() == typeof(string))
            {
                int result = 0;
                int.TryParse((string)value, out result);
                return result;
            }

            return 0;
        }
    }
}
