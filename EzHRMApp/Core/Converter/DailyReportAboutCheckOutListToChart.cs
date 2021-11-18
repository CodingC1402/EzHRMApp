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
    public class DailyReportAboutCheckOutListToChart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (targetType == typeof(SeriesCollection) && value.GetType() == typeof(ObservableCollection<DailyReportModel>))
            {
                SeriesCollection series;
                var dailyReports = value as ObservableCollection<DailyReportModel>;

                StackedAreaSeries checkOutEarly = new()
                {
                    Title = "Check out early",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>(),
                    Fill = (Brush)Application.Current.FindResource("CancleButtonBrush")
                };
                StackedAreaSeries checkOutOnTime = new()
                {
                    Title = "Check out on time",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>(),
                    Fill = (Brush)Application.Current.FindResource("UpdateButtonBrush")
                };
                StackedAreaSeries workOverTime = new()
                {
                    Title = "Work overtime",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>(),
                    Fill = (Brush)Application.Current.FindResource("ConfirmButtonBrush")
                };

                foreach (var report in dailyReports)
                {
                    workOverTime.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVLamThemGio));
                    checkOutEarly.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVTanLamSom));
                    checkOutOnTime.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVTanLamDungGio));
                }

                series = new()
                {
                    workOverTime,
                    checkOutOnTime,
                    checkOutEarly
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
