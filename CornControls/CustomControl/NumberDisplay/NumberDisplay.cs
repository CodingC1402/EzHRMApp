using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CornControls.CustomControl
{
    public class NumberDisplay : Control
    {
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(NumberDisplay));
        public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(nameof(IconBrush), typeof(Brush), typeof(NumberDisplay), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register(nameof(IconPath), typeof(Geometry), typeof(NumberDisplay));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(NumberDisplay));
       
        public static readonly DependencyProperty NumberFontSizeProperty = DependencyProperty.Register(nameof(NumberFontSize), typeof(double), typeof(NumberDisplay));
        public static readonly DependencyProperty NumberMarginProperty = DependencyProperty.Register(nameof(NumberMargin), typeof(Thickness), typeof(NumberDisplay));
        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(nameof(TextMargin), typeof(Thickness), typeof(NumberDisplay));

        public static readonly DependencyProperty NumberTextProperty = DependencyProperty.Register(nameof(NumberText), typeof(string), typeof(NumberDisplay), new PropertyMetadata(""));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(NumberDisplay), new PropertyMetadata(""));

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public Brush IconBrush
        {
            get => (Brush)GetValue(IconBrushProperty);
            set => SetValue(IconBrushProperty, value);
        }
        public Geometry IconPath
        {
            get => (Geometry)GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public double NumberFontSize
        {
            get => (double)GetValue(NumberFontSizeProperty);
            set => SetValue(NumberFontSizeProperty, value);
        }
        public Thickness NumberMargin
        {
            get => (Thickness)GetValue(NumberMarginProperty);
            set => SetValue(NumberMarginProperty, value);
        }
        public Thickness TextMargin
        {
            get => (Thickness)GetValue(TextMarginProperty);
            set => SetValue(TextMarginProperty, value);
        }

        public string NumberText
        {
            get => (string)GetValue(NumberTextProperty);
            set => SetValue(NumberTextProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        static NumberDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberDisplay), new FrameworkPropertyMetadata(typeof(NumberDisplay)));
        }
        
        public NumberDisplay()
        {
        }
    }
}
