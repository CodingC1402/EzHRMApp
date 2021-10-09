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
    public class RadioButtonEx : ButtonEx
    {
        public static readonly DependencyProperty SelectedPaddingProperty = DependencyProperty.Register("SelectedPadding", typeof(Thickness), typeof(RadioButtonEx));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register("NormalPadding", typeof(Thickness), typeof(RadioButtonEx));

        public static readonly DependencyProperty OutterColorProperty = DependencyProperty.Register("OutterColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(RadioButtonEx), new PropertyMetadata(Brushes.Gray));

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
        public Brush SelectedColor
        {
            get => (Brush)GetValue(SelectedColorProperty);
            set => SetValue(PathProperty, value);
        }

        static RadioButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonEx), new FrameworkPropertyMetadata(typeof(RadioButtonEx)));
        }
    }
}
