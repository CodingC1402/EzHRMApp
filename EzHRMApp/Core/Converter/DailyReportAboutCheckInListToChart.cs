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
    public class DailyReportAboutCheckInListToChart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(SeriesCollection) && value.GetType() == typeof(ObservableCollection<DailyReportModel>))
            {
                SeriesCollection series;
                var dailyReports = value as ObservableCollection<DailyReportModel>;

                StackedAreaSeries checkInEarly = new()
                {
                    Title = "Check in early",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>(),
                    Fill = (Brush)Application.Current.FindResource("ConfirmButtonBrush")
                };
                StackedAreaSeries checkInOnTime = new()
                {
                    Title = "Check in on time",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>(),
                    Fill = (Brush)Application.Current.FindResource("UpdateButtonBrush")
                };
                StackedAreaSeries checkInLate = new()
                {
                    Title = "Check in late",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>(),
                    Fill = (Brush)Application.Current.FindResource("CancleButtonBrush")
                };

                foreach (var report in dailyReports)
                {
                    if (report != null)
                    {
                        checkInLate.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVDenTre));
                        checkInOnTime.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVDenDungGio));
                        checkInEarly.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVDenSom));
                    }
                }

                series = new()
                {
                    checkInEarly,
                    checkInOnTime,
                    checkInLate,
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
