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
    /// Interaction logic for DailyReportsView.xaml
    /// </summary>
    public partial class DailyReportsView : UserControl
    {
        public DailyReportsView()
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
                DailyReportModel dailyReport = obj as DailyReportModel;

                if (datagridEx.SearchText != "")
                {
                    var searchText = datagridEx.SearchText;
                    if (datagridEx.SearchFilter == "Report date")
                        return dailyReport.NgayBaoCao.ToString("dd/MM/yyyy").Contains(searchText);

                    if (!int.TryParse(searchText, out int result))
                        return false;

                    switch (datagridEx.SearchFilter)
                    {
                        case "Came early":
                            return dailyReport.SoNVDenSom == result;
                        case "Came on time":
                            return dailyReport.SoNVDenDungGio == result;
                        case "Came late":
                            return dailyReport.SoNVDenTre == result;
                        case "Left early":
                            return dailyReport.SoNVTanLamSom == result;
                        case "Left on time":
                            return dailyReport.SoNVTanLamDungGio == result;
                        case "Worked overtime":
                            return dailyReport.SoNVLamThemGio == result;
                        case "Absent":
                            return dailyReport.SoNVVangMat == result;
                    }
                }

                return true;
            });
        }

        private void ButtonEx_Click(object sender, RoutedEventArgs e)
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
    }
}
