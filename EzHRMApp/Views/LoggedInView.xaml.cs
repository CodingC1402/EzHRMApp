using CornControls.Window;
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
    /// Interaction logic for LoggedInView.xaml
    /// </summary>
    public partial class LoggedInView : UserControlWindow
    {
        private RadioButton _previousTabButton = null;

        public LoggedInView()
        {
            InitializeComponent();
        }

        protected override void OnGetWindowParent()
        {
            base.OnGetWindowParent();
            var wnd = OwnerWindow;
            wnd.MaxWidth = double.PositiveInfinity;
            wnd.MaxHeight = double.PositiveInfinity;

            wnd.MinHeight = 610; 
            wnd.Height = 610;
            wnd.MinWidth = 1000; 
            wnd.Width = 1000;

            OwnerWindow.WindowStateChanged += (s, e) =>
            {
                if (e.NewValue == WindowState.Normal)
                {
                    maximizeBtn.Content = "◻";
                }
                else
                {
                    maximizeBtn.Content = "⧉";
                }
            };
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

        protected void ShowHelp(object sender, EventArgs e)
        {

        }
    }
}
