using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EzHRMApp.Core.Converter
{
    public class ScheduleBoolToWorkdayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                var val = (bool)value;
                return val ? "Work" : "Day off";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value.ToString();
            if (val == "Work")
                return true;
            else if (val == "Day off")
                return false;

            return false;
        }
    }
}
