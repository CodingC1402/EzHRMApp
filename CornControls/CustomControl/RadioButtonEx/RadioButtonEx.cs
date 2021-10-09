using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.CompilerServices;

namespace CornControls.CustomControl
{
    public class RadioButtonEx : RadioButton, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SelectedPaddingProperty = DependencyProperty.Register("SelectedPadding", typeof(Thickness), typeof(RadioButtonEx));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register("NormalPadding", typeof(Thickness), typeof(RadioButtonEx));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register("NormalColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register("HoverColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty PressedColorProperty = DependencyProperty.Register("PressedColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(Geometry), typeof(RadioButtonEx));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(double), typeof(RadioButtonEx));
        public static readonly DependencyProperty IconPadProperty = DependencyProperty.Register("IconPad", typeof(GridLength), typeof(RadioButtonEx));

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        [Browsable(true), Category("Appearance")]
        public Thickness SelectedPadding
        {
            get => (Thickness)GetValue(SelectedPaddingProperty);
            set
            {
                SetValue(SelectedPaddingProperty, value);
                RaisePropertyChanged();
            }
        }
        [Browsable(true), Category("Appearance")]
        public Thickness NormalPadding
        {
            get => (Thickness)GetValue(NormalPaddingProperty);
            set
            {
                SetValue(NormalPaddingProperty, value);
                RaisePropertyChanged();
            }
        }

        [Browsable(true), Category("Appearance")]
        public Brush NormalColor
        {
            get => (Brush)GetValue(NormalColorProperty);
            set
            {
                SetValue(NormalColorProperty, value);
                RaisePropertyChanged();
            }
        }
        [Browsable(true), Category("Appearance")]
        public Brush HoverColor
        {
            get => (Brush)GetValue(HoverColorProperty);
            set
            {
                SetValue(HoverColorProperty, value);
                RaisePropertyChanged();
            }
        }
        [Browsable(true), Category("Appearance")]
        public Brush PressedColor
        {
            get => (Brush)GetValue(PressedColorProperty);
            set
            {
                SetValue(PressedColorProperty, value);
                RaisePropertyChanged();
            }
        }

        [Browsable(true), Category("Appearance")]
        public Brush SelectedColor
        {
            get => (Brush)GetValue(SelectedColorProperty);
            set
            {
                SetValue(SelectedColorProperty, value);
                RaisePropertyChanged();
            }
        }

        [Browsable(true), Category("Appearance")]
        public Geometry Path
        {
            get => (Geometry)GetValue(PathProperty);
            set
            {
                SetValue(PathProperty, value);
                RaisePropertyChanged();
            }
        }

        [Browsable(true), Category("Appearance")]
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set
            {
                SetValue(IconSizeProperty, value);
                RaisePropertyChanged();
            }
        }
        [Browsable(true), Category("Appearance")]
        public GridLength IconPad
        {
            get => (GridLength)GetValue(IconPadProperty);
            set
            {
                SetValue(IconPadProperty, value);
                RaisePropertyChanged();
            }
        }

        static RadioButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonEx), new FrameworkPropertyMetadata(typeof(RadioButtonEx)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }
    }
}
