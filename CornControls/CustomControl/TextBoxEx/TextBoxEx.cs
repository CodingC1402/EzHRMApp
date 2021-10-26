using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CornControls.CustomControl
{
    [AddINotifyPropertyChangedInterface]
    public class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty IsCapslockedProperty = DependencyProperty.Register(nameof(IsCapslocked), typeof(bool), typeof(TextBoxEx));

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(Geometry), typeof(TextBoxEx));
        public static new readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register(nameof(IsFocused), typeof(bool), typeof(TextBoxEx));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TextBoxEx));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register(nameof(NormalColor), typeof(Brush), typeof(TextBoxEx), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(nameof(HoverColor), typeof(Brush), typeof(TextBoxEx), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty FocusedColorProperty = DependencyProperty.Register(nameof(FocusedColor), typeof(Brush), typeof(TextBoxEx), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(TextBoxEx));
        public static readonly DependencyProperty IconPadProperty = DependencyProperty.Register(nameof(IconPad), typeof(GridLength), typeof(TextBoxEx));

        public static readonly DependencyProperty FocusedPaddingProperty = DependencyProperty.Register(nameof(FocusedPadding), typeof(Thickness), typeof(TextBoxEx));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register(nameof(NormalPadding), typeof(Thickness), typeof(TextBoxEx));

        public static readonly DependencyProperty OutterColorProperty = DependencyProperty.Register(nameof(OutterColor), typeof(Brush), typeof(TextBoxEx), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty FocusedOutterColorProperty = DependencyProperty.Register(nameof(FocusedOutterColor), typeof(Brush), typeof(TextBoxEx), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ReadOnlyBackgroundProperty = DependencyProperty.Register(nameof(ReadOnlyBackground), typeof(Brush), typeof(TextBoxEx), new PropertyMetadata(Brushes.Gray));

        [Browsable(true), Category("Appearance")]
        public bool IsCapslocked
        {
            get => (bool)GetValue(IsCapslockedProperty);
            set => SetValue(IsCapslockedProperty, value);
        }

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

        private TextBox contentTextBox = null;
        [Browsable(true), Category("Appearance")]
        public new string SelectedText
        {
            get => contentTextBox.SelectedText;
        }

        [Browsable(true), Category("Appearance")]
        public Brush ReadOnlyBackground
        {
            get => (Brush)GetValue(ReadOnlyBackgroundProperty);
            set => SetValue(ReadOnlyBackgroundProperty, value);
        }

        static TextBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEx), new FrameworkPropertyMetadata(typeof(TextBoxEx)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            contentTextBox = Template.FindName(nameof(contentTextBox), this) as TextBox;
            contentTextBox.LostFocus += (s, e) =>
            {
                IsFocused = false;
            };
            contentTextBox.GotFocus += (s, e) =>
            {
                IsFocused = true;
            };

            contentTextBox.PreviewKeyDown += (s, e) =>
            {
                if (Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled)
                {
                    IsCapslocked = true;
                }
                else
                {
                    IsCapslocked = false;
                }
            };
        }
    }
}
