using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class DashboardViewModel : Navigation.ViewModelBase
    {
        public enum TimeSpan
        {
            OneWeek,
            OneMonth,
            OneYear
        }

        private TimeSpan _compilingTimeSpan = TimeSpan.OneMonth;
        private DateTime _viewingDate = DateTime.Today;

        public static DashboardViewModel Instance { get; private set; }
        public override string ViewName => "Dashboard";

        public TimeSpan CompilingTimeSpan
        {
            get => _compilingTimeSpan;
            set
            {
                _compilingTimeSpan = value;
                switch (value)
                {
                    case TimeSpan.OneWeek:
                        TimeSpanText = "a week";
                        break;
                    case TimeSpan.OneMonth:
                        TimeSpanText = "a month";
                        break;
                    case TimeSpan.OneYear:
                        TimeSpanText = "a year";
                        break;
                }
                Compile();
            }
        }
        public DateTime ViewingDate
        {
            get => _viewingDate;
            set
            {
                _viewingDate = value;
                Compile();
            }
        }
        public TimeSpan[] AvailableTimeSpan { get; } = new TimeSpan[] {
            TimeSpan.OneWeek,
            TimeSpan.OneMonth,
            TimeSpan.OneYear
        };

        public DailyReportModel CurrentReport { get; set; }
        public ObservableCollection<DailyReportModel> Reports { get; set; }

        public int BeingLateSum { get; set; }
        public int BeingEarlySum { get; set; }
        public int BeingOnTimeSum { get; set; }

        public int WorkOverTimeSum { get; set; }
        public int CheckOutEarly { get; set; }
        public int CheckOutOnTime { get; set; }

        public string TimeSpanText { get; set; }

        // For the charts
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public DashboardViewModel()
        {
            Instance = this;
            CompilingTimeSpan = TimeSpan.OneWeek;
            XFormatter = val => new DateTime((long)val).ToString("yyyy");
            YFormatter = val => val.ToString("N") + " M";

            Compile();
        }

        protected void Compile()
        {
            BeingEarlySum = BeingLateSum = BeingOnTimeSum = WorkOverTimeSum = CheckOutEarly = CheckOutOnTime = 0;
            Reports = new ObservableCollection<DailyReportModel>();

            int dayToCheck = 0;
            switch (_compilingTimeSpan)
            {
                case TimeSpan.OneWeek:
                    dayToCheck = 7;
                    break;
                case TimeSpan.OneMonth:
                    dayToCheck = 30;
                    break;
                case TimeSpan.OneYear:
                    dayToCheck = 365;
                    break;
            }

            for (int i = dayToCheck - 1; i >= 0; i--)
            {
                var checkingDate = _viewingDate.Date.AddDays(-i);
                var dailyReport = DailyReportModel.GetReportOfDate(checkingDate);

                if (dailyReport != null)
                {
                    BeingEarlySum += dailyReport.SoNVDenSom;
                    BeingLateSum += dailyReport.SoNVDenTre;
                    BeingOnTimeSum += dailyReport.SoNVDenDungGio;

                    WorkOverTimeSum += dailyReport.SoNVLamThemGio;
                    CheckOutEarly += dailyReport.SoNVTanLamSom;
                    CheckOutOnTime += dailyReport.SoNVTanLamDungGio;
                }
                else
                {
                    dailyReport = new DailyReportModel { NgayBaoCao = checkingDate };
                }
                Reports.Add(dailyReport);
            }

            CurrentReport = Reports.Last();
        }
    }
}
