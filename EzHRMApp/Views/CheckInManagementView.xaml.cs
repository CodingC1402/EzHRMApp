using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Model;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for CheckInManagementView.xaml
    /// </summary>
    public partial class CheckInManagementView : UserControl
    {
        public CheckInManagementView()
        {
            InitializeComponent();
        }

        protected void showTodayOnlyChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected void filterTypeChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected void filterTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(datagridEx.ItemsSource);
            if (filterTextBox.Text == "" && showTodayCheckBox.IsChecked.HasValue && !showTodayCheckBox.IsChecked.Value)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = obj =>
                {
                    CheckInModel checkIn = obj as CheckInModel;
                    if (showTodayCheckBox.IsChecked.HasValue && showTodayCheckBox.IsChecked.Value
                    && !(checkIn.ThoiGianVaoLam.Date == DateTime.Now.Date
                    || (checkIn.ThoiGianVaoLam.Date == DateTime.Now.Date.AddDays(-1) && checkIn.ThoiGianTanLam == null)))
                        return false;

                    if (filterTextBox.Text != "")
                    {
                        var searchText = filterTextBox.Text;
                        switch ((filterComboboxs.SelectedItem as DataGridTextColumn).Header)
                        {
                            case "Check-in time":
                                return checkIn.ThoiGianVaoLam.ToString("dd/MM/yyyy").Contains(searchText);
                            case "Employee ID":
                                return checkIn.IDNhanVien.Contains(searchText);
                            case "Check-out time":
                                return checkIn.ThoiGianTanLam.HasValue 
                                    && checkIn.ThoiGianTanLam.Value.ToString("dd/MM/yyyy").Contains(searchText);
                        }
                    }

                    return true;
                };
            }
        }
    }
}
