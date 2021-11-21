using CornControls.CustomControl;
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
    /// Interaction logic for PenaltyView.xaml
    /// </summary>
    public partial class PenaltyView : UserControl
    {
        public PenaltyView()
        {
            InitializeComponent();
        }

        protected void searchChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            if (datagridEx.SearchText == "")
            {
                datagridEx.SetCollectionFilter(null);
            }
            else
            {
                datagridEx.SetCollectionFilter(obj =>
                {
                    PenaltyModel role = obj as PenaltyModel;

                    if (datagridEx.SearchText != "")
                    {
                        var searchText = datagridEx.SearchText;
                        switch (datagridEx.SearchFilter)
                        {
                            case "ID":
                                return role.ID.ToString().Contains(searchText);
                            case "Date":
                                return role.Ngay.ToString("dd/MM/yyyy").Contains(searchText);
                            case "Employee ID":
                                return role.IDNhanVien.Contains(searchText);
                            case "Penalty Type":
                                return role.TenViPham.Contains(searchText);
                            case "Flat deduction":
                                return role.SoTienTru.ToString("N2").Contains(searchText);
                            case "Percentage deduction":
                                return $"{role.SoPhanTramTru.ToString("N2")}%".Contains(searchText);
                        }
                    }

                    return true;
                });
            }
        }
    }
}
