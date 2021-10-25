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

            var width = 350;
            var height = 510;
            OwnerWindow.ResizeMode = ResizeMode.NoResize;
            SetWindowSize(width, height, width, height, width, height);
        }

        protected void CloseWindow(object sender, EventArgs e)
        {
            OwnerWindow.Close();
        }

        private void EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                signInBtn.Command?.Execute(signInBtn.CommandParameter);
            }
        }
    }
}
