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

        protected void filterTextChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            if (datagridEx.SearchText == "" && showTodayCheckBox.IsChecked.HasValue && !showTodayCheckBox.IsChecked.Value)
            {
                datagridEx.SetCollectionFilter(null);
            }
            else
            {
                datagridEx.SetCollectionFilter(obj =>
                    {
                        CheckInModel checkIn = obj as CheckInModel;
                        if (showTodayCheckBox.IsChecked.HasValue && showTodayCheckBox.IsChecked.Value
                        && !(checkIn.ThoiGianVaoLam.Date == DateTime.Now.Date
                        || (checkIn.ThoiGianVaoLam.Date == DateTime.Now.Date.AddDays(-1) && checkIn.ThoiGianTanLam == null)))
                            return false;

                        if (datagridEx.SearchText != "")
                        {
                            var searchText = datagridEx.SearchText;
                            switch (datagridEx.SearchFilter)
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
                    }
                );
            }
        }
    }
}
