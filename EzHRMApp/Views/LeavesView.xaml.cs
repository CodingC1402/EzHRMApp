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
    /// Interaction logic for LeavesView.xaml
    /// </summary>
    public partial class LeavesView : UserControl
    {
        public LeavesView()
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
                    LeaveModel role = obj as LeaveModel;
                    if (datagridEx.SearchText != "")
                    {
                        var searchText = datagridEx.SearchText;
                        switch (datagridEx.SearchFilter)
                        {
                            case "Employee ID":
                                return role.IDNhanVien.Contains(searchText);
                            case "Date":
                                return role.NgayBatDauNghi.ToString("dd/MM/yyyy").Contains(searchText);
                            case "Amount of days":
                                return role.SoNgayNghi.ToString().Contains(searchText);
                            case "Reasons":
                                return role.LyDoNghi.ToString().Contains(searchText);
                        }
                    }

                    return true;
                });
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PercentValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        // Clamp value
        private void TextBoxEx_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            float value = 0;
            bool isFloat = float.TryParse(textBox.Text, out value);
            if (!isFloat)
            {
                textBox.Text = "0";
            }
            else
            {
                if (value > 100)
                {
                    textBox.Text = "100";
                }
                else if (value < 0)
                {
                    textBox.Text = "0";
                }
            }
        }
    }
}
