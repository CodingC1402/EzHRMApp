using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using CsvHelper;
using Model;
using ViewModel.Helper;

namespace ViewModel
{
    public class MonthlyReportViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Monthly Reports";

        public ObservableCollection<MonthlyReportModel> MonthlyReports { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public MonthlyReportViewModel()
        {
            MonthlyReports = MonthlyReportModel.LoadAll();
            StartDate = EndDate = DateTime.Now;
        }

        private void ExportToCSV(object param)
        {
            if (param == null)
                return;

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

            var reports = MonthlyReportModel.GetReportsBetweenDates(StartDate, EndDate);
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
