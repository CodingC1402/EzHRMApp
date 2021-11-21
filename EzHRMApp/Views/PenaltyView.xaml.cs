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
                    PenaltyModel penalty = obj as PenaltyModel;

                    if (datagridEx.SearchText != "")
                    {
                        var searchText = datagridEx.SearchText;
                        switch (datagridEx.SearchFilter)
                        {
                            case "ID":
                                return penalty.ID.ToString().Contains(searchText);
                            case "Date":
                                return penalty.Ngay.ToShortDateString().Contains(searchText);
                            case "Employee ID":
                                return penalty.IDNhanVien.Contains(searchText);
                            case "Penalty Type":
                                return penalty.TenViPham.Contains(searchText);
                            case "Flat fine":
                                return penalty.SoTienTru.ToString().Contains(searchText);
                            case "Percent fine":
                                return penalty.SoPhanTramTru.ToString().Contains(searchText);
                        }
                    }

                    return true;
                });
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            // Popup message not work like messagebox || Need changing
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this penalty?", "Warning", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
                deleteBtn.CommandParameter = true;
            else
                deleteBtn.CommandParameter = false;
        }
    }
}
