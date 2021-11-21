using Microsoft.Win32;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for EmployeePayrollManagementView.xaml
    /// </summary>
    public partial class EmployeePayrollManagementView : UserControl
    {
        public EmployeePayrollManagementView()
        {
            InitializeComponent();
        }

        private void DatagridEx_SearchChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            datagridEx.SetCollectionFilter(obj =>
            {
                var salary = obj as SalaryModel;

                if (datagridEx.SearchText != "")
                {
                    var searchText = datagridEx.SearchText;
                    if (datagridEx.SearchFilter == "Pay date")
                        return salary.NgayTinhLuong.ToString("dd/MM/yyyy").Contains(searchText);

                    if (datagridEx.SearchFilter == "Employee ID")
                        return salary.IDNhanVien.Contains(searchText);

                    if (!float.TryParse(searchText, out float result))
                        return false;

                    switch (datagridEx.SearchFilter)
                    {
                        case "Base salary":
                            return salary.TienLuong == result;
                        case "Deduction":
                            return salary.TienTruLuong == result;
                        case "Bonus":
                            return salary.TienThuong == result;
                        case "Total salary":
                            return salary.TongTienLuong == result;
                    }
                }

                return true;
            });
        }

        private void allEmployeesCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            idTxt.IsEnabled = false;
        }

        private void allEmployeesCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            idTxt.IsEnabled = true;
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!endDatepicker.SelectedDate.HasValue || !startDatepicker.SelectedDate.HasValue)
            {
                exportBtn.CommandParameter = "date-picker-empty";
                return;
            }
            if (endDatepicker.SelectedDate.Value < startDatepicker.SelectedDate.Value)
            {
                exportBtn.CommandParameter = "date-picker-end-date-earlier";
                return;
            }

            var save = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".csv",
                Filter = "CSV file (.csv)|*.csv"
            };

            var result = save.ShowDialog();
            if (result == true)
            {
                exportBtn.CommandParameter = save.FileName;
            }
        }
    }
}
