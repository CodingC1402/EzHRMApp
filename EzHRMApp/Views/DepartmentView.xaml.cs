using Model;
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

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for DepartmentView.xaml
    /// </summary>
    public partial class DepartmentView : UserControl
    {
        public DepartmentView()
        {
            InitializeComponent();
        }

        protected void showResignedEmployeesChanged(object sender, RoutedEventArgs e)
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
            if (filterTextBox.Text == "" && showResignCheckBox.IsChecked.HasValue && showResignCheckBox.IsChecked.Value)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = obj =>
                {
                    DepartmentModel department = obj as DepartmentModel;
                    if ((!showResignCheckBox.IsChecked.HasValue || !showResignCheckBox.IsChecked.Value) && department.NgayNgungHoatDong.HasValue)
                    {
                        return false;
                    }

                    if (filterTextBox.Text != "")
                    {
                        var searchText = filterTextBox.Text;
                        switch ((filterComboboxs.SelectedItem as DataGridTextColumn).Header)
                        {
                            case "Name":
                                return department.TenPhong.Contains(searchText);
                            case "Founding Date":
                                return department.NgayThanhLap.ToString("dd:MM:yyyy").Contains(searchText);
                            case "Shutdown Date":
                                if (department.NgayNgungHoatDong.HasValue)
                                    return department.NgayNgungHoatDong.Value.ToString("dd:MM:yyyy").Contains(searchText);
                                else
                                    break;
                            case "Department Head":
                                return department.TruongPhong.Contains(searchText);
                        }
                    }

                    return true;
                };
            }
        }
    }
}
