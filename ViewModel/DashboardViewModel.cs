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
                if (value.Year != _viewingDate.Year)
                {
                    CompileMonthlyReport(value.Year);
                }
                _viewingDate = value;
                Compile();
            }
        }
        public DateTime EarliestDate { get; set; }
        public TimeSpan[] AvailableTimeSpan { get; } = new TimeSpan[] {
            TimeSpan.OneWeek,
            TimeSpan.OneMonth,
            TimeSpan.OneYear
        };

        public DailyReportModel CurrentReport { get; set; }
        public ObservableCollection<DailyReportModel> Reports { get; set; }
        public ObservableCollection<MonthlyReportModel> MonthlyReports { get; set; }

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

        public string[] XAxisLabels { get; set; } = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        public Func<double, string> MonthlyXFormatter { get; set; }

        public DashboardViewModel()
        {
            Instance = this;
            CompilingTimeSpan = TimeSpan.OneWeek;

            XFormatter = val => {
                string result = "";
                switch (_compilingTimeSpan)
                {
                    case TimeSpan.OneWeek:
                        result = new DateTime((long)val).ToString("d/M/yyyy");
                        break;
                    case TimeSpan.OneMonth:
                        result = new DateTime((long)val).ToString("d/M/yyyy");
                        break;
                    case TimeSpan.OneYear:
                        result = new DateTime ((long)val).ToString("M/yyyy");
                        break;
                }

                return result;
            };
            YFormatter = val => val.ToString();
            MonthlyXFormatter = val =>
            {
                return $"{val} {_viewingDate.Year}";
            };

            Compile();
            CompileMonthlyReport(_viewingDate.Year);
        }

        protected void Compile()
        {
            BeingEarlySum = BeingLateSum = BeingOnTimeSum = WorkOverTimeSum = CheckOutEarly = CheckOutOnTime = 0;
            ObservableCollection<DailyReportModel> newReports = null;

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
            EarliestDate = _viewingDate.AddDays(-dayToCheck + 1);

            var queriedReports = DailyReportModel.GetDailyReportsInTimeSpan(EarliestDate, _viewingDate);

            int queriedIndex = 0;
            int lateSum = 0, earlySum = 0, onTimeSum = 0, overTimeSum = 0, outEarly = 0, outOnTime = 0;

            if (queriedReports.Count > 0)
            {
                newReports = new ObservableCollection<DailyReportModel>();
                for (int i = dayToCheck - 1; i >= 0; i--)
                {
                    DateTime checkingDate = _viewingDate.AddDays(-i);

                    if (queriedIndex < queriedReports.Count && checkingDate == queriedReports[queriedIndex].NgayBaoCao)
                    {
                        var checkingReport = queriedReports[queriedIndex];
                        newReports.Add(checkingReport);

                        lateSum += checkingReport.SoNVDenTre;
                        earlySum += checkingReport.SoNVDenSom;
                        onTimeSum += checkingReport.SoNVDenDungGio;

                        overTimeSum += checkingReport.SoNVLamThemGio;
                        outEarly += checkingReport.SoNVTanLamSom;
                        outOnTime += checkingReport.SoNVTanLamDungGio;

                        queriedIndex++;
                    }
                    else
                    {
                        newReports.Add(new DailyReportModel { NgayBaoCao = checkingDate });
                    }
                }
            }

            BeingLateSum = lateSum;
            BeingEarlySum = earlySum;
            BeingOnTimeSum = onTimeSum;

            WorkOverTimeSum = overTimeSum;
            CheckOutEarly = outEarly;
            CheckOutOnTime = outOnTime;

            // This is to remove small detail in yearly reports, like remove 5 day from the report to reduce lags
            if (_compilingTimeSpan == TimeSpan.OneYear)
            {
                var filteringReports = newReports;
                newReports = new ObservableCollection<DailyReportModel>();
                for (int i = 0; i < dayToCheck; i++)
                {
                    if (newReports.Count == 0 || !newReports.Last().CompareData(filteringReports[i]))
                    {
                        newReports.Add(filteringReports[i]);
                    }
                }
            }

            Reports = newReports;
            CurrentReport = Reports?.Last();
        }

        protected void CompileMonthlyReport(int year)
        {
            var result = MonthlyReportModel.GetAllReportOfYear(year);
            if (result != null && result.Count > 0)
            {
                var fillingInArray = new ObservableCollection<MonthlyReportModel>();

                int resultIndex = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (resultIndex < result.Count && result[resultIndex].Thang == i + 1)
                    {
                        fillingInArray.Add(result[resultIndex]);
                        resultIndex++;
                    }
                    else
                    {
                        fillingInArray.Add(new MonthlyReportModel { Thang = i + 1, Nam = year });
                    }
                }
                MonthlyReports = fillingInArray;
            }
            else
                MonthlyReports = null;
        }
    }
}
