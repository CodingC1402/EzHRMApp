using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CornControls.CustomControl
{
    public class ToggleButton : ButtonEx
    {
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(nameof(Selected), typeof(bool), typeof(ToggleButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedChangedCallback));
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(Brush), typeof(ToggleButton), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty SelectedHoverColorProperty = DependencyProperty.Register(nameof(SelectedHoverColor), typeof(Brush), typeof(ButtonEx), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty SelectedPressedColorProperty = DependencyProperty.Register(nameof(SelectedPressedColor), typeof(Brush), typeof(ButtonEx), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register(nameof(SelectedPath), typeof(Geometry), typeof(ButtonEx));

        public static void SelectedChangedCallback (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ToggleButton control = sender as ToggleButton;
            if (control != null)
            {
                control.OnSelectedChanged(new EventArgs());
            }
        }

        static ToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButton), new FrameworkPropertyMetadata(typeof(ToggleButton)));
        }

        [Browsable(true), Category("Appearance")]
        public Brush SelectedHoverColor
        {
            get => (Brush)GetValue(SelectedHoverColorProperty);
            set => SetValue(SelectedHoverColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush SelectedPressedColor
        {
            get => (Brush)GetValue(SelectedPressedColorProperty);
            set => SetValue(SelectedPressedColorProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Geometry SelectedPath
        {
            get => (Geometry)GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }

        public Brush SelectedColor
        {
            get => (Brush)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        private event EventHandler _selectedChanged;
        public event EventHandler SelectedChanged
        {
            add { _selectedChanged += value; }
            remove { _selectedChanged -= value; }
        }

        protected virtual void OnSelectedChanged(EventArgs e)
        {
            if (_selectedChanged != null)
                _selectedChanged(this, e);
        }

        protected override void OnClick()
        {
            base.OnClick();
            BindingExpression be = BindingOperations.GetBindingExpression(this, SelectedProperty);
            if (be == null)
                Selected = !Selected;
        }
    }
}
