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
    /// Interaction logic for RoleManagement.xaml
    /// </summary>
    public partial class RoleManagementView : UserControl
    {
        public RoleManagementView()
        {
            InitializeComponent();
        }


        protected void searchChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            if (datagridEx.SearchText == "" && showResignCheckBox.IsChecked.HasValue && showResignCheckBox.IsChecked.Value)
            {
                datagridEx.SetCollectionFilter(null);
            }
            else
            {
                datagridEx.SetCollectionFilter(obj =>
                {
                    RoleModel role = obj as RoleModel;
                    if ((!showResignCheckBox.IsChecked.HasValue || !showResignCheckBox.IsChecked.Value) && role.DaXoa == 1)
                    {
                        return false;
                    }

                    if (datagridEx.SearchText != "")
                    {
                        var searchText = datagridEx.SearchText;
                        switch (datagridEx.SearchFilter)
                        {
                            case "Name":
                                return role.TenChucVu.Contains(searchText);
                            case "Salary by":
                                return role.CachTinhLuong.Contains(searchText);
                            case "Hourly Wage":
                                return role.TienLuongMoiGio.ToString().Contains(searchText);
                            case "Monthly Wage":
                                return role.TienLuongMoiThang.ToString().Contains(searchText);
                            case "Overtime %":
                                return role.PhanTramLuongNgoaiGio.ToString().Contains(searchText);
                        }
                    }

                    return true;
                });
            }
        }
    }
}
