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
using System.Windows.Data;

namespace EzHRMApp.Core.Converter
{
    public class DailyReportAboutCheckOutListToChart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(SeriesCollection) && value.GetType() == typeof(ObservableCollection<DailyReportModel>))
            {
                SeriesCollection series;
                var dailyReports = value as ObservableCollection<DailyReportModel>;

                StackedAreaSeries workOverTime = new()
                {
                    Title = "Work overtime",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>()
                };
                StackedAreaSeries checkOutEarly = new()
                {
                    Title = "Check out early",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>()
                };
                StackedAreaSeries checkOutOnTime = new()
                {
                    Title = "Check out on time",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>()
                };

                foreach (var report in dailyReports)
                {
                    workOverTime.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVLamThemGio));
                    checkOutEarly.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVTanLamSom));
                    checkOutOnTime.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVTanLamDungGio));
                }

                series = new()
                {
                    checkOutEarly,
                    checkOutOnTime,
                    workOverTime
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
