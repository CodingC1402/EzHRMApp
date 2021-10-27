using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CornControls.CustomControl
{
    public class BaseControl : Control
    {
        public static readonly DependencyProperty IsCapslockedProperty = DependencyProperty.Register(nameof(IsCapslocked), typeof(bool), typeof(BaseControl));

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(Geometry), typeof(BaseControl));
        public static new readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register(nameof(IsFocused), typeof(bool), typeof(BaseControl));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(BaseControl));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register(nameof(NormalColor), typeof(Brush), typeof(BaseControl), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(nameof(HoverColor), typeof(Brush), typeof(BaseControl), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty FocusedColorProperty = DependencyProperty.Register(nameof(FocusedColor), typeof(Brush), typeof(BaseControl), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(BaseControl));
        public static readonly DependencyProperty IconPadProperty = DependencyProperty.Register(nameof(IconPad), typeof(GridLength), typeof(BaseControl));

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
    }
}
