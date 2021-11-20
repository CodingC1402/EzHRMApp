using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ViewModel.Navigation;
using Model;
using System.Collections.ObjectModel;
using ViewModel.Helper;
using CsvHelper;
using System.IO;

namespace ViewModel
{
    public class EmployeePayrollManagementViewModel : CRUDViewModelBase
    {
        public override string ViewName => "Employee Payroll";

        public ObservableCollection<SalaryModel> Salaries { get; set; }
        public bool ExportAllEmployees { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private string _lastSelectedID;
        public string SelectedEmployeeID
        {
            get => ExportAllEmployees ? _lastSelectedID : _selectedSalary?.IDNhanVien;
            set { }
        }

        private SalaryModel _selectedSalary;
        public SalaryModel SelectedSalary
        {
            get => _selectedSalary;
            set
            {
                _selectedSalary = value;
                if (!ExportAllEmployees) _lastSelectedID = _selectedSalary.IDNhanVien;
            }
        }

        public EmployeePayrollManagementViewModel()
        {
            Salaries = SalaryModel.LoadAll();
            StartDate = EndDate = DateTime.Now.Date;
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

            List<SalaryModel> salaries;
            if (ExportAllEmployees)
            {
                salaries = SalaryModel.GetAllBetween(StartDate, EndDate);
            }
            else salaries = SalaryModel.GetByEmployeeAllBetween(SelectedSalary.IDNhanVien, StartDate, EndDate);

            using (var streamWriter = new StreamWriter(msg))
            using (var writer = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture))
            {
                writer.WriteRecords(salaries);
            }
        }

        private RelayCommand<string> _exportCSVCommand;
        public RelayCommand<string> ExportCSVCommand => _exportCSVCommand ??= new RelayCommand<string>(ExportToCSV);
    }
}
