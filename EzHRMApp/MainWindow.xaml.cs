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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using EzHRMApp.Helper;
using WpfScreenHelper;
using CornControls.Window;
using ViewModel;
using CornControls.Themes;

namespace EzHRMApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BlankWindow
    {
        public MainWindow()
        {
            _offSet = new Thickness(-8);
            InitializeComponent();

            SettingViewModel vmInstance = SettingViewModel.Instance;
            if (vmInstance != null && vmInstance.CurrentTheme >= 0)
            {
                ThemeHelper.SelectTheme((ThemeHelper.ThemeColor)vmInstance.CurrentTheme);
            }
        }
    }
}
