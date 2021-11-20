using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Model;
using ViewModel.Helper;

namespace ViewModel
{
    public class DailyReportViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Daily Reports";

        public ObservableCollection<DailyReportModel> DailyReports { get; set; }
        public List<string> TimespanCollection { get; private set; } = new List<string>() { "Week", "Month", "Year" };

        public DailyReportViewModel()
        {
            DailyReports = DailyReportModel.LoadAll();
        }

        private void ExportToCSV(object param)
        {

        }

        private RelayCommand<object> _exportCSVCommand;
        public RelayCommand<object> ExportCSVCommand => _exportCSVCommand ??= new RelayCommand<object>(ExportToCSV);
    }
}
