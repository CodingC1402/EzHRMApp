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
using Microsoft.Win32;
using Model;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for MonthlyReportsView.xaml
    /// </summary>
    public partial class MonthlyReportsView : UserControl
    {
        public MonthlyReportsView()
        {
            InitializeComponent();
        }

        protected void filterTextChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            datagridEx.SetCollectionFilter(obj =>
            {
                MonthlyReportModel monthlyReport = obj as MonthlyReportModel;

                if (datagridEx.SearchText != "")
                {
                    if (!int.TryParse(datagridEx.SearchText, out int result))
                        return false;

                    switch (datagridEx.SearchFilter)
                    {
                        case "Month":
                            return monthlyReport.Thang == result;
                        case "Year":
                            return monthlyReport.Nam.ToString().Contains(datagridEx.SearchText);
                        case "New employees":
                            return monthlyReport.SoNhanVienMoi == result;
                        case "Resigned employees":
                            return monthlyReport.SoNhanVienThoiViec == result;
                    }
                }

                return true;
            });
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!endDatepicker.SelectedDate.HasValue || !startDatepicker.SelectedDate.HasValue)
            {
                exportBtn.CommandParameter = "date-picker-empty";
                return;
            }
            else if (endDatepicker.SelectedDate.Value < startDatepicker.SelectedDate.Value)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            startDatepicker.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            endDatepicker.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);
        }
    }
}
