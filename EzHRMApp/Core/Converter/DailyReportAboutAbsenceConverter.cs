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
    class DailyReportAboutAbsenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (targetType == typeof(SeriesCollection) && value.GetType() == typeof(ObservableCollection<DailyReportModel>))
            {
                SeriesCollection series;
                var dailyReports = value as ObservableCollection<DailyReportModel>;

                StackedAreaSeries absence = new()
                {
                    Title = "Absence",
                    LineSmoothness = 0,
                    Values = new ChartValues<DateTimePoint>(),
                    Fill = (Brush)Application.Current.FindResource("CancleButtonHoverBrush")
                };
                //StackedAreaSeries showUp = new()
                //{
                //    Title = "Showed up",
                //    LineSmoothness = 0,
                //    Values = new ChartValues<DateTimePoint>(),
                //    Fill = (Brush)Application.Current.FindResource("ConfirmButtonHoverBrush")
                //};

                foreach (var report in dailyReports)
                {
                    if (report != null)
                    {
                        absence.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVVangMat));
                        //showUp.Values.Add(new DateTimePoint(report.NgayBaoCao, report.SoNVDenSom + report.SoNVDenDungGio + report.SoNVDenTre));
                    }
                }

                series = new()
                {
                    //showUp,
                    absence,
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
