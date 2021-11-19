using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EzHRMApp.Core.Converter
{
    public class MonthlyReportToChart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (targetType == typeof(SeriesCollection) && value.GetType() == typeof(ObservableCollection<MonthlyReportModel>))
            {
                SeriesCollection series;
                var monthlyReports = value as ObservableCollection<MonthlyReportModel>;

                ColumnSeries newEmployees = new()
                {
                    Title = "New employees",
                    Values = new ChartValues<int>(),
                    Fill = (Brush)Application.Current.FindResource("ConfirmButtonHoverBrush")
                };
                ColumnSeries resignedEmployees = new()
                {
                    Title = "Resigned employees",
                    Values = new ChartValues<int>(),
                    Fill = (Brush)Application.Current.FindResource("CancleButtonHoverBrush")
                };

                foreach (var report in monthlyReports)
                {
                    newEmployees.Values.Add(report.SoNhanVienMoi);
                    resignedEmployees.Values.Add(report.SoNhanVienThoiViec);
                }

                series = new()
                {
                    newEmployees,
                    resignedEmployees
                };

                return series;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
