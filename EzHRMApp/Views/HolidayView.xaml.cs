using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for HolidayView.xaml
    /// </summary>
    public partial class HolidayView : UserControl
    {
        public HolidayView()
        {
            InitializeComponent();
        }

        protected void searchChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            //if (datagridEx.SearchText == "" && showResignCheckBox.IsChecked.HasValue && showResignCheckBox.IsChecked.Value)
            //{
            //    datagridEx.SetCollectionFilter(null);
            //}

            datagridEx.SetCollectionFilter(obj =>
            {
                HolidayModel department = obj as HolidayModel;
                //if ((!showResignCheckBox.IsChecked.HasValue || !showResignCheckBox.IsChecked.Value) && department.NgayNgungHoatDong.HasValue)
                //{
                //    return false;
                //}

                if (datagridEx.SearchText != "")
                {
                    var searchText = datagridEx.SearchText;
                    switch (datagridEx.SearchFilter)
                    {
                        case "Name":
                            return department.TenDipNghiLe.Contains(searchText);
                        case "Founding Date":
                            return department.Ngay.ToString().Contains(searchText);
                        case "Shutdown Date":
                            return department.Thang.ToString().Contains(searchText);
                        case "Department Head":
                            return department.SoNgayNghi.ToString().Contains(searchText);
                    }
                }

                return true;
            });
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
