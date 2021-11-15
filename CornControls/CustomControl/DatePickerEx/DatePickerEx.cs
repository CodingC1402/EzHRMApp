using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PropertyChanged;

namespace CornControls.CustomControl
{
    [AddINotifyPropertyChangedInterface]
    public class DatePickerEx : TextBoxEx, INotifyPropertyChanged
    {
        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.Register(nameof(DateFormat), typeof(string), typeof(DatePickerEx), new PropertyMetadata("dd/MM/yyyy"));
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(DatePickerEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedChangedCallBack));
        public static readonly DependencyProperty CalendarIconPathProperty = DependencyProperty.Register(nameof(CalendarIconPath), typeof(Geometry), typeof(DatePickerEx));
        public static readonly DependencyProperty CalendarIconWidthProperty = DependencyProperty.Register(nameof(CalendarIconWidth), typeof(GridLength), typeof(DatePickerEx));
        public static readonly DependencyProperty CalendarIconBrushProperty = DependencyProperty.Register(nameof(CalendarIconBrush), typeof(Brush), typeof(DatePickerEx));
        public static readonly DependencyProperty CalendarIconSizeProperty = DependencyProperty.Register(nameof(CalendarIconSize), typeof(double), typeof(DatePickerEx));

        public static readonly DependencyProperty ButtonHoverBrushProperty = DependencyProperty.Register(nameof(ButtonHoverBrush), typeof(Brush), typeof(DatePickerEx));
        public static readonly DependencyProperty ButtonNormalBrushProperty = DependencyProperty.Register(nameof(ButtonNormalBrush), typeof(Brush), typeof(DatePickerEx));
        public static readonly DependencyProperty ButtonPressedBrushProperty = DependencyProperty.Register(nameof(ButtonPressedBrush), typeof(Brush), typeof(DatePickerEx));
        
        protected Calendar _calendar = null;
        protected Popup _popup = null;
        protected ButtonEx _calenderBtn = null;

        [Browsable(true), Category("Appearance")]
        public string DateFormat
        {
            get => (string)GetValue(DateFormatProperty);
            set => SetValue(DateFormatProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Geometry CalendarIconPath
        {
            get => (Geometry)GetValue(CalendarIconPathProperty);
            set => SetValue(CalendarIconPathProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public GridLength CalendarIconWidth
        {
            get => (GridLength)GetValue(CalendarIconWidthProperty);
            set => SetValue(CalendarIconWidthProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush CalendarIconBrush
        {
            get => (Brush)GetValue(CalendarIconBrushProperty);
            set => SetValue(CalendarIconBrushProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public double CalendarIconSize
        {
            get => (double)GetValue(CalendarIconSizeProperty);
            set => SetValue(CalendarIconSizeProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Brush ButtonHoverBrush
        {
            get => (Brush)GetValue(ButtonHoverBrushProperty);
            set => SetValue(ButtonHoverBrushProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush ButtonNormalBrush
        {
            get => (Brush)GetValue(ButtonNormalBrushProperty);
            set => SetValue(ButtonNormalBrushProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush ButtonPressedBrush
        {
            get => (Brush)GetValue(ButtonPressedBrushProperty);
            set => SetValue(ButtonPressedBrushProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private event EventHandler _selectedDateChanged = null;
        public event EventHandler SelectedDateChanged
        {
            add { _selectedDateChanged += value; }
            remove { _selectedDateChanged -= value; }
        }

        private RelayCommand<object> _openCalendar;
        public RelayCommand<object> OpenCalendar => _openCalendar ??= new RelayCommand<object>(param => {
            _popup.IsOpen = !_popup.IsOpen;
            if (_popup.IsOpen)
            {
                //_popup.Focus();
                Mouse.Capture(_calendar, CaptureMode.SubTree);
                _calendar.Focus();
            }
        }, param =>
        {
            return IsEnabled;
        });

        private RelayCommand<object> _clearCalendar;
        public RelayCommand<object> ClearCalendar => _clearCalendar ??= new RelayCommand<object>(param => {
            SelectedDate = null;
            ClearCalendar.RaiseCanExecuteChangeEvent();
        }, param =>
        {
            return SelectedDate.HasValue && IsEnabled;
        });

        private static void OnSelectedChangedCallBack(
        DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DatePickerEx control = sender as DatePickerEx;
            if (control != null)
            {
                control.OnSelectedDateChanged(new EventArgs());
            }
        }
        protected virtual void OnSelectedDateChanged(EventArgs e)
        {
            UpdateText();
            if (_selectedDateChanged != null)
                _selectedDateChanged(this, new EventArgs());
        }

        static DatePickerEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePickerEx), new FrameworkPropertyMetadata(typeof(DatePickerEx)));
        }

        public DatePickerEx()
        {
            IsReadOnly = true;
            IsEnabledChanged += (s, e) =>
            {
                OpenCalendar.RaiseCanExecuteChangeEvent();
                ClearCalendar.RaiseCanExecuteChangeEvent();
                if (_calenderBtn != null)
                    _calenderBtn.Visibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _calenderBtn = Template.FindName("PART_calendarButton", this) as ButtonEx;
            _calenderBtn.Visibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;

            _popup = Template.FindName("PART_popup", this) as Popup;
            _popup.Focusable = false;

            _calendar = Template.FindName("PART_calendar", this) as Calendar;
            _calendar.SelectionMode = CalendarSelectionMode.SingleDate;
            _calendar.MouseDown += (s, e) =>
            {
                _calendar.ReleaseMouseCapture();
                _popup.IsOpen = false;
            };
            _calendar.Focusable = true;
        }

        protected virtual void UpdateText()
        {
            if (SelectedDate.HasValue)
            {
                Text = SelectedDate.Value.ToString(DateFormat);
                ClearCalendar.RaiseCanExecuteChangeEvent();
            }
            else Text = "";
        }
    }
}