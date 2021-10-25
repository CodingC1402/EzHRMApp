using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CornControls.CustomControl
{
    [AddINotifyPropertyChangedInterface]
    public class DatagridEx : DataGrid
    {
        public static readonly DependencyProperty DisabledOpacityProperty = DependencyProperty.Register(nameof(DisabledOpacity), typeof(double), typeof(DatagridEx), new PropertyMetadata(0.8));

        [Browsable(true), Category("Appearance")]
        public double DisabledOpacity
        {
            get => (double)GetValue(DisabledOpacityProperty);
            set
            {
                value = Math.Clamp(value, 0, 1);
                SetValue(DisabledOpacityProperty, value);
            }
        }

        static DatagridEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatagridEx), new FrameworkPropertyMetadata(typeof(DatagridEx)));
        }
    }
}
