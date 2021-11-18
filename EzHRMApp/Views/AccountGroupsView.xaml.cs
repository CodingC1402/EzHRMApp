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
using Model;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for AccountGroupsView.xaml
    /// </summary>
    public partial class AccountGroupsView : UserControl
    {
        public AccountGroupsView()
        {
            InitializeComponent();
        }

        protected void searchChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            datagridEx.SetCollectionFilter(obj =>
            {
                if (String.IsNullOrWhiteSpace(datagridEx.SearchText))
                    return true;

                var ag = obj as AccountGroupModel;
                return ag.TenNhomTaiKhoan.Contains(datagridEx.SearchText);
            });
        }
    }
}
