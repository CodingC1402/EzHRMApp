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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControlWindow
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnGetWindowParent()
        {
            base.OnGetWindowParent();
            var wnd = OwnerWindow;
            wnd.MaxHeight = wnd.MinHeight = wnd.Height = 510;
            wnd.MaxWidth = wnd.MinWidth = wnd.Width = 370;
        }

        protected void CloseWindow(object sender, EventArgs e)
        {
            OwnerWindow.Close();
        }
    }
}
