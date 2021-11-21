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
    /// Interaction logic for PenaltyTypeView.xaml
    /// </summary>
    public partial class PenaltyTypeView : UserControl
    {
        public PenaltyTypeView()
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
                    PenaltyTypeModel penaltyType = obj as PenaltyTypeModel;

                    if (datagridEx.SearchText != "")
                    {
                        var searchText = datagridEx.SearchText;
                        switch (datagridEx.SearchFilter)
                        {
                            case "Name":
                                return penaltyType.TenViPham.Contains(searchText);
                            case "Flat fine":
                                return penaltyType.TruLuongTrucTiep.ToString().Contains(searchText);
                            case "Percent fine":
                                return penaltyType.TruLuongTheoPhanTram.ToString().Contains(searchText);
                        }
                    }

                    return true;
                });
            }
        }
    }
}
