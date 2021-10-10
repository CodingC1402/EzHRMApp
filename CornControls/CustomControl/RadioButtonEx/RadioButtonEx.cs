using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace CornControls.CustomControl
{
    [AddINotifyPropertyChangedInterface]
    public class RadioButtonEx : RadioButton
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RadioButtonEx));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register("NormalColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register("HoverColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty PressedColorProperty = DependencyProperty.Register("PressedColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(Geometry), typeof(RadioButtonEx));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(double), typeof(RadioButtonEx));
        public static readonly DependencyProperty IconPadProperty = DependencyProperty.Register("IconPad", typeof(GridLength), typeof(RadioButtonEx));

        public static readonly DependencyProperty SelectedPaddingProperty = DependencyProperty.Register("SelectedPadding", typeof(Thickness), typeof(RadioButtonEx));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register("NormalPadding", typeof(Thickness), typeof(RadioButtonEx));

        public static readonly DependencyProperty OutterColorProperty = DependencyProperty.Register("OutterColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty SelectedOutterColorProperty = DependencyProperty.Register("SelectedOutterColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));

        [Browsable(true), Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
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
        public Brush PressedColor
        {
            get => (Brush)GetValue(PressedColorProperty);
            set => SetValue(PressedColorProperty, value);
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
        public Thickness SelectedPadding
        {
            get => (Thickness)GetValue(SelectedPaddingProperty);
            set => SetValue(SelectedPaddingProperty, value);
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
        public Brush SelectedOutterColor
        {
            get => (Brush)GetValue(SelectedOutterColorProperty);
            set => SetValue(SelectedOutterColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush SelectedColor
        {
            get => (Brush)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        static RadioButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonEx), new FrameworkPropertyMetadata(typeof(RadioButtonEx)));
        }
    }
}
