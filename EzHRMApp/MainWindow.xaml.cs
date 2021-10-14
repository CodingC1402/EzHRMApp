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
using EzHRMApp.Windows;

namespace EzHRMApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BlankWindow
    {
        private RadioButton  _previousTabButton = null;
        private LoginWindow _loginWindow = null;

        public MainWindow()
        {
            _offSet = new Thickness(-8);
            InitializeComponent();
        }

        protected void HomeButtonClicked(object sender, EventArgs e)
        {
            if (_previousTabButton != null)
            {
                _previousTabButton.IsChecked = false;
                _previousTabButton = null;
            }
            homeBtn.Width = 0;
        }

        protected void TabButtonClicked(object sender, EventArgs e)
        {
            homeBtn.Width = 75;
            _previousTabButton = sender as RadioButton;
        }

        protected override void OnWindowStateChanged(RoutedPropertyChangedEventArgs<WindowState> e)
        {
            base.OnWindowStateChanged(e);
            if (e.NewValue == WindowState.Normal)
            {
                maximizeBtn.Content = "◻";
            }
            else
            {
                maximizeBtn.Content = "⧉";
            }
        }

        protected void ShowHelp(object sender, EventArgs e)
        {
            if (_loginWindow == null)
            {
                _loginWindow = new LoginWindow();
                _loginWindow.Owner = this;
                _loginWindow.ShowInTaskbar = false;
            }

            _loginWindow.ShowDialog();
        }
    }
}
