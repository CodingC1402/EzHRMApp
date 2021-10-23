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
using PropertyChanged;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for StaffsView.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class StaffsView : UserControl
    {
        public static readonly DependencyProperty IDWidthProperty = DependencyProperty.Register(nameof(IDWidth), typeof(GridLength), typeof(StaffsView));
        public static readonly DependencyProperty SurnameWidthProperty = DependencyProperty.Register(nameof(SurnameWidth), typeof(GridLength), typeof(StaffsView));
        public static readonly DependencyProperty NameWidthProperty = DependencyProperty.Register(nameof(NameWidth), typeof(GridLength), typeof(StaffsView));
        public static readonly DependencyProperty EmailWidthProperty = DependencyProperty.Register(nameof(EmailWidth), typeof(GridLength), typeof(StaffsView));
        public static readonly DependencyProperty PhoneNumberWidthProperty = DependencyProperty.Register(nameof(PhoneNumberWidth), typeof(GridLength), typeof(StaffsView));
        public static readonly DependencyProperty StartWorkingWidthProperty = DependencyProperty.Register(nameof(StartWorkingWidth), typeof(GridLength), typeof(StaffsView));

        public GridLength IDWidth {
            get => (GridLength)GetValue(IDWidthProperty);
            set => SetValue(IDWidthProperty, value);
        }
        public GridLength SurnameWidth {
            get => (GridLength)GetValue(SurnameWidthProperty);
            set => SetValue(SurnameWidthProperty, value);
        }
        public GridLength NameWidth {
            get => (GridLength)GetValue(NameWidthProperty);
            set => SetValue(NameWidthProperty, value);
        }
        public GridLength EmailWidth {
            get => (GridLength)GetValue(EmailWidthProperty);
            set => SetValue(EmailWidthProperty, value);
        }
        public GridLength PhoneNumberWidth {
            get => (GridLength)GetValue(PhoneNumberWidthProperty);
            set => SetValue(PhoneNumberWidthProperty, value);
        }
        public GridLength StartWorkingWidth {
            get => (GridLength)GetValue(StartWorkingWidthProperty);
            set => SetValue(StartWorkingWidthProperty, value);
        }

        public StaffsView()
        {
            InitializeComponent();
        }
    }
}
