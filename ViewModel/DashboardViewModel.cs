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
        public override string ViewName => "Dashboard";

        public int ViewingDatIndex {
            get => _viewingDateIndex;
            set
            {
                _viewingDateIndex = value;
                CurrentReport = InWeekReports[_viewingDateIndex];
            }
        }

        public DailyReportModel CurrentReport
        {
            get => _currentReport;
            set => _currentReport = value;
        }

        public ObservableCollection<DateTime> InWeekDates { get; set; }
        public ObservableCollection<DailyReportModel> InWeekReports { get; set; }

        private int _viewingDateIndex = 0;
        private DailyReportModel _currentReport;

        public DashboardViewModel()
        {
            InWeekDates = new ObservableCollection<DateTime>();
            InWeekReports = new ObservableCollection<DailyReportModel>();
            for (int i = 0; i < 7; i++)
            {
                InWeekDates.Add(DateTime.Now.AddDays(-i));
                var dailyReport = DailyReportModel.GetReportOfDate(InWeekDates[i]);
                if (dailyReport != null)
                {
                    InWeekReports.Add(dailyReport);
                }
                else
                {
                    InWeekDates.RemoveAt(InWeekDates.Count - 1);
                    break;
                }
            }
        }
    }
}
