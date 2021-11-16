using System;
using System.Collections.ObjectModel;
using System.Text;
using Model;

namespace ViewModel
{
    public class MonthlyReportViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Monthly Reports";

        public ObservableCollection<MonthlyReportModel> MonthlyReports { get; set; }

        public MonthlyReportViewModel()
        {
            MonthlyReports = MonthlyReportModel.LoadAll();
        }
    }
}
