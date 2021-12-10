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
            //else
            //{
            //    datagridEx.SetCollectionFilter(obj =>
            //    {
            //        DepartmentModel department = obj as DepartmentModel;
            //        if ((!showResignCheckBox.IsChecked.HasValue || !showResignCheckBox.IsChecked.Value) && department.NgayNgungHoatDong.HasValue)
            //        {
            //            return false;
            //        }

            //        if (datagridEx.SearchText != "")
            //        {
            //            var searchText = datagridEx.SearchText;
            //            switch (datagridEx.SearchFilter)
            //            {
            //                case "Name":
            //                    return department.TenPhong.Contains(searchText);
            //                case "Founding Date":
            //                    return department.NgayThanhLap.ToString("dd:MM:yyyy").Contains(searchText);
            //                case "Shutdown Date":
            //                    if (department.NgayNgungHoatDong.HasValue)
            //                        return department.NgayNgungHoatDong.Value.ToString("dd:MM:yyyy").Contains(searchText);
            //                    else
            //                        break;
            //                case "Department Head":
            //                    return department.TruongPhong.Contains(searchText);
            //            }
            //        }

            //        return true;
            //    });
            //}
        }
    }
}
