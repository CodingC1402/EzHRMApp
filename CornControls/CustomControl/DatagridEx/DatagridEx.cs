using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CornControls.CustomControl
{
    [AddINotifyPropertyChangedInterface]
    public class DatagridEx : DataGrid
    {
        public static readonly DependencyProperty DisabledOpacityProperty = DependencyProperty.Register(nameof(DisabledOpacity), typeof(double), typeof(DatagridEx), new PropertyMetadata(0.8));

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(DatagridEx), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSearchChangedCallBack));
        public static readonly DependencyProperty SearchFilterProperty = DependencyProperty.Register(nameof(SearchFilter), typeof(string), typeof(DatagridEx), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSearchChangedCallBack));
        
        public static readonly DependencyProperty SearchTextboxWidthProperty = DependencyProperty.Register(nameof(SearchTextboxWidth), typeof(double), typeof(DatagridEx), new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSearchChangedCallBack));
        public static readonly DependencyProperty SearchComboboxWidthProperty = DependencyProperty.Register(nameof(SearchComboboxWidth), typeof(double), typeof(DatagridEx), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSearchChangedCallBack));

        private ICollectionView _collectionView;
        private Predicate<object> _filter;
        private ComboBox _combobox;

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

        [Browsable(true), Category("Appearance")]
        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public string SearchFilter
        {
            get => (string)GetValue(SearchFilterProperty);
            set => SetValue(SearchFilterProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public double SearchTextboxWidth
        {
            get => (double)GetValue(SearchTextboxWidthProperty);
            set => SetValue(SearchTextboxWidthProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public double SearchComboboxWidth
        {
            get => (double)GetValue(SearchComboboxWidthProperty);
            set => SetValue(SearchComboboxWidthProperty, value);
        }

        public Predicate<object> GetCollectionFilter()
        {
            return _filter;
        }

        public void SetCollectionFilter(Predicate<object> filter)
        {
            _filter = filter;
            if (_collectionView != null)
            {
                _collectionView.Filter = _filter;
            }
        }

        private event RoutedEventHandler _searchChanged = null;
        public event RoutedEventHandler SearchChanged
        {
            add { _searchChanged += value; }
            remove { _searchChanged -= value; }
        }

        private static void OnSearchChangedCallBack(
        DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DatagridEx control = sender as DatagridEx;
            if (control != null)
            {
                control.OnSearchChanged(new RoutedEventArgs());
            }
        }

        protected virtual void OnSearchChanged(RoutedEventArgs e)
        {
            if (_searchChanged != null)
                _searchChanged(this, e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _combobox = (ComboBox)Template.FindName("filterComboboxs", this);

            _combobox.SelectionChanged += (s, e) =>
            {
                SearchFilter = (_combobox.SelectedItem as DataGridColumn).Header.ToString();
            };
        }

        static DatagridEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatagridEx), new FrameworkPropertyMetadata(typeof(DatagridEx)));
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            _collectionView = CollectionViewSource.GetDefaultView(newValue);
            _collectionView.Filter = _filter;
        }
    }
}
