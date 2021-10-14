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
using System.Windows.Shapes;

namespace EzHRMApp.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : BlankWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            MaxHeight = MinHeight = Height;
            MaxWidth = MinWidth = Width;
        }

        protected void CloseWindow(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
