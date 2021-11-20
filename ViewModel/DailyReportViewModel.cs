using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Model;
using ViewModel.Helper;
using CsvHelper;
using System.IO;

namespace ViewModel
{
    public class DailyReportViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Daily Reports";

        public ObservableCollection<DailyReportModel> DailyReports { get; set; }
        public List<string> TimespanCollection { get; private set; } = new List<string>() { "Week", "Month", "Year" };
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DailyReportViewModel()
        {
            DailyReports = DailyReportModel.LoadAll();
            StartDate = EndDate = DateTime.Now;
        }

        private void ExportToCSV(object param)
        {
            var msg = param.ToString();
            if (msg == "date-picker-empty")
            {
                ErrorString = "Please select both start and end dates";
                HaveError = true;
                return;
            }
            if (msg == "date-picker-end-date-earlier")
            {
                ErrorString = "End date has to be greater than start date";
                HaveError = true;
                return;
            }

            var reports = DailyReportModel.GetDailyReportsInTimeSpan(StartDate, EndDate);
            using (var streamWriter = new StreamWriter(msg))
            using (var writer = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture))
            {
                writer.WriteRecords(reports);
            }
        }

        private RelayCommand<object> _exportCSVCommand;
        public RelayCommand<object> ExportCSVCommand => _exportCSVCommand ??= new RelayCommand<object>(ExportToCSV);
    }
}
