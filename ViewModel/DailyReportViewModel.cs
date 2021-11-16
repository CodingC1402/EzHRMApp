using System;
using System.Collections.ObjectModel;
using System.Text;
using Model;

namespace ViewModel
{
    public class DailyReportViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Daily Reports";

        public ObservableCollection<DailyReportModel> DailyReports { get; set; }

        public DailyReportViewModel()
        {
            DailyReports = DailyReportModel.LoadAll();
        }
    }
}
