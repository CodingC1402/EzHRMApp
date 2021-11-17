using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Navigation;

namespace ViewModel
{
    public class ReportsViewModel : NavigationViewModel
    {
        public override string ViewName => "Reports";

        public ReportsViewModel()
        {
            ToMonthlyReports = new NavigationCommand<MonthlyReportViewModel>(new MonthlyReportViewModel(), this, 0);
            ViewModels.Add(ToMonthlyReports.ViewModel);

            ToDailyReports = new NavigationCommand<DailyReportViewModel>(new DailyReportViewModel(), this, 0);
            ViewModels.Add(ToDailyReports.ViewModel);

            CurrentViewModel = ToDailyReports.ViewModel;
        }

        public NavigationCommand<MonthlyReportViewModel> ToMonthlyReports { get; set; }
        public NavigationCommand<DailyReportViewModel> ToDailyReports { get; set; }
    }
}
