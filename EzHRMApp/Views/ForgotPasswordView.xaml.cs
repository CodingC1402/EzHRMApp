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
using ViewModel;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for ForgotPasswordView.xaml
    /// </summary>
    public partial class ForgotPasswordView : UserControlWindow
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
        }

        protected override void OnGetWindowParent()
        {
            base.OnGetWindowParent();

            var width = 700;
            var height = 400;
            OwnerWindow.ResizeMode = ResizeMode.NoResize;
            SetWindowSize(width, height, width, height, width, height);
        }

        protected void CloseWindow(object sender, EventArgs e)
        {
            OwnerWindow.Close();
        }

        private void ButtonEx_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordViewModel.Instance.ConfirmPassword = confirmPassBox.Password;
            ForgotPasswordViewModel.Instance.NewPassword = newPassBox.Password;
        }
    }
}
