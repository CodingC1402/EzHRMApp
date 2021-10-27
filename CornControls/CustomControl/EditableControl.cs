using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CornControls.CustomControl
{
    public class EditableControl : BaseControl
    {
        public static readonly DependencyProperty FocusedPaddingProperty = DependencyProperty.Register(nameof(FocusedPadding), typeof(Thickness), typeof(EditableControl));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register(nameof(NormalPadding), typeof(Thickness), typeof(EditableControl));

        public static readonly DependencyProperty OutterColorProperty = DependencyProperty.Register(nameof(OutterColor), typeof(Brush), typeof(EditableControl), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty FocusedOutterColorProperty = DependencyProperty.Register(nameof(FocusedOutterColor), typeof(Brush), typeof(EditableControl), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ReadOnlyBackgroundProperty = DependencyProperty.Register(nameof(ReadOnlyBackground), typeof(Brush), typeof(EditableControl), new PropertyMetadata(Brushes.Gray));

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
        public Brush ReadOnlyBackground
        {
            get => (Brush)GetValue(ReadOnlyBackgroundProperty);
            set => SetValue(ReadOnlyBackgroundProperty, value);
        }
    }
}
