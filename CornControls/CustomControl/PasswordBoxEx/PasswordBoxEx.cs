using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CornControls.CustomControl
{
    [AddINotifyPropertyChangedInterface]
    public class PasswordBoxEx : Control, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(Geometry), typeof(PasswordBoxEx));
        public static new readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register(nameof(IsFocused), typeof(bool), typeof(PasswordBoxEx));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PasswordBoxEx));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register(nameof(NormalColor), typeof(Brush), typeof(PasswordBoxEx), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(nameof(HoverColor), typeof(Brush), typeof(PasswordBoxEx), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty FocusedColorProperty = DependencyProperty.Register(nameof(FocusedColor), typeof(Brush), typeof(PasswordBoxEx), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(PasswordBoxEx));
        public static readonly DependencyProperty IconPadProperty = DependencyProperty.Register(nameof(IconPad), typeof(GridLength), typeof(PasswordBoxEx));

        public static readonly DependencyProperty FocusedPaddingProperty = DependencyProperty.Register(nameof(FocusedPadding), typeof(Thickness), typeof(PasswordBoxEx));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register(nameof(NormalPadding), typeof(Thickness), typeof(PasswordBoxEx));

        public static readonly DependencyProperty OutterColorProperty = DependencyProperty.Register(nameof(OutterColor), typeof(Brush), typeof(PasswordBoxEx), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty FocusedOutterColorProperty = DependencyProperty.Register(nameof(FocusedOutterColor), typeof(Brush), typeof(PasswordBoxEx), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty SelectionTextBrushProperty = DependencyProperty.Register(nameof(SelectionTextBrush), typeof(Brush), typeof(PasswordBoxEx), new PropertyMetadata(Brushes.Gray));

        [Browsable(true), Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public new bool IsFocused
        {
            get => (bool)GetValue(IsFocusedProperty);
            private set => SetValue(IsFocusedProperty, value);
        }
        public Brush NormalColor
        {
            get => (Brush)GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush HoverColor
        {
            get => (Brush)GetValue(HoverColorProperty);
            set => SetValue(HoverColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush FocusedColor
        {
            get => (Brush)GetValue(FocusedColorProperty);
            set => SetValue(FocusedColorProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Geometry Path
        {
            get => (Geometry)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public GridLength IconPad
        {
            get => (GridLength)GetValue(IconPadProperty);
            set => SetValue(IconPadProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Thickness FocusedPadding
        {
            get => (Thickness)GetValue(FocusedPaddingProperty);
            set => SetValue(FocusedPaddingProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Thickness NormalPadding
        {
            get => (Thickness)GetValue(NormalPaddingProperty);
            set => SetValue(NormalPaddingProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Brush OutterColor
        {
            get => (Brush)GetValue(OutterColorProperty);
            set => SetValue(OutterColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush FocusedOutterColor
        {
            get => (Brush)GetValue(FocusedOutterColorProperty);
            set => SetValue(FocusedOutterColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush SelectionTextBrush
        {
            get => (Brush)GetValue(SelectionTextBrushProperty);
            set => SetValue(SelectionTextBrushProperty, value);
        }

        private PasswordBox contentPasswordBox = null;
        [Browsable(true), Category("Appearance")]
        public SecureString SecturedPassword
        {
            get
            {
                if (contentPasswordBox != null)
                    return contentPasswordBox.SecurePassword;
                else
                    return null;
            }
        }

        [Browsable(true), Category("Appearance")]
        private string _password = "";
        public string Password
        {
            get
            {
                if (contentPasswordBox != null)
                    return contentPasswordBox.Password;
                else
                    return "";
            }
            set
            {
                if (contentPasswordBox != null)
                    contentPasswordBox.Password = value;
                else
                    _password = value;

                RaisePropertyChanged(nameof(SecturedPassword));
            }
        }

        [Browsable(true), Category("Appearance")]
        private char _passwordChar = '●';

        public event PropertyChangedEventHandler PropertyChanged;

        public char PasswordChar
        {
            get
            {
                if (contentPasswordBox != null)
                    return contentPasswordBox.PasswordChar;
                else
                    return _passwordChar;
            }
            set
            {
                if (contentPasswordBox != null)
                    contentPasswordBox.PasswordChar = value;
                else
                    _passwordChar = value;
            }
        }

        static PasswordBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBoxEx), new FrameworkPropertyMetadata(typeof(PasswordBoxEx)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            contentPasswordBox = Template.FindName(nameof(contentPasswordBox), this) as PasswordBox;
            contentPasswordBox.LostFocus += (s, e) =>
            {
                IsFocused = false;
            };
            contentPasswordBox.GotFocus += (s, e) =>
            {
                IsFocused = true;
            };
            contentPasswordBox.PasswordChanged += (s, e) =>
            {
                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(SecturedPassword));
            };

            contentPasswordBox.Password = _password;
            contentPasswordBox.PasswordChar = _passwordChar;
            RaisePropertyChanged(nameof(SecturedPassword));

            _password = "╭∩╮(︶︿︶)╭∩╮";
            _passwordChar = '*';
        }

        protected void RaisePropertyChanged([CallerMemberName]string propName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
