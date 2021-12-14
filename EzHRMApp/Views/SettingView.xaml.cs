using CornControls.Themes;
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
using ViewModel;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();

            SettingViewModel vmInstance = SettingViewModel.Instance;
            if (vmInstance == null)
                return;

            vmInstance.OnColorChanged.AddListener(ColorIndexChanged);
        }

        private void ColorIndexChanged(int index)
        {
            ThemeHelper.SelectTheme((ThemeHelper.ThemeColor)index);
        }
    }
}
