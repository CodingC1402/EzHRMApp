using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EzHRMApp.Core.Converter
{
    public class FloatToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(float))
            {
                return ((float)value).ToString();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float) && value.GetType() == typeof(string))
            {
                float result = 0;
                float.TryParse((string)value, out result);
                return result;
            }

            return 0;
        }
    }
}
