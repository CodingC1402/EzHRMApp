using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using PropertyChanged;

namespace CornControls.CustomControl
{
    [AddINotifyPropertyChangedInterface]
    public class TimePicker : Control, INotifyPropertyChanged
    {
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }

        private static void OnTimeChangedCallBack(
        DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = sender as TimePicker;
            if (control != null)
            {
                control.OnTimeDateChanged(new RoutedEventArgs());
            }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TimePicker));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register(nameof(NormalColor), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(nameof(HoverColor), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty FocusedColorProperty = DependencyProperty.Register(nameof(FocusedColor), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty FocusedPaddingProperty = DependencyProperty.Register(nameof(FocusedPadding), typeof(Thickness), typeof(TimePicker));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register(nameof(NormalPadding), typeof(Thickness), typeof(TimePicker));

        public static readonly DependencyProperty OutterColorProperty = DependencyProperty.Register(nameof(OutterColor), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty FocusedOutterColorProperty = DependencyProperty.Register(nameof(FocusedOutterColor), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ReadOnlyBackgroundProperty = DependencyProperty.Register(nameof(ReadOnlyBackground), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(DateTime), typeof(TimePicker), new FrameworkPropertyMetadata(DateTime.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTimeChangedCallBack));
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(nameof(IsPressed), typeof(bool), typeof(TimePicker));

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
        public Brush FocusedColor
        {
            get => (Brush)GetValue(FocusedColorProperty);
            set => SetValue(FocusedColorProperty, value);
        }

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

        [Browsable(true), Category("Appearance")]
        public DateTime Time
        {
            get => (DateTime)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public int Hour
        {
            get => Time.Hour;
        }

        [Browsable(true), Category("Appearance")]
        public int Minute
        {
            get => Time.Minute;
        }

        private event RoutedEventHandler _timeChanged;
        public event RoutedEventHandler TimeChanged
        {
            add { _timeChanged += value; }
            remove { _timeChanged -= value; }
        }

        private Popup _popUp;
        private Grid _grid;
        private Border _border;

        private Path _hourUp;
        private Path _hourDown;

        private Path _minuteUp;
        private Path _minuteDown;

        private Timer _cdTimer = new Timer();
        private Timer _repeatTimer = new Timer();

        private Path _currentPath = null;
        private int _reportTimerDefaultInterval = 50;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popUp = (Popup)Template.FindName("PART_popUP", this);
            _grid = (Grid)Template.FindName("PART_grid", this);
            _border = (Border)Template.FindName("PART_border", this);

            _hourUp = (Path)Template.FindName("PART_hourUp", this);
            _hourDown = (Path)Template.FindName("PART_hourDown", this);
            _minuteUp = (Path)Template.FindName("PART_minuteUp", this);
            _minuteDown = (Path)Template.FindName("PART_minuteDown", this);

            _hourUp.MouseDown += PathDown;
            _hourDown.MouseDown += PathDown;

            _minuteUp.MouseDown += PathDown;
            _minuteDown.MouseDown += PathDown;

            _hourUp.MouseUp += PathUp;
            _hourDown.MouseUp += PathUp;

            _minuteUp.MouseUp += PathUp;
            _minuteDown.MouseUp += PathUp;

            _popUp.Opened += (s, e) =>
            {
                Mouse.Capture(this, CaptureMode.SubTree);
            };

            MouseDown += (s, e) =>
            {
                if (_popUp.IsOpen && !_grid.IsMouseOver)
                {
                    ReleaseMouseCapture();
                    _popUp.IsOpen = false;
                }
                else if (!_popUp.IsOpen)
                {
                    _popUp.IsOpen = true;
                }
            };

            _cdTimer.Interval = 500;
            _repeatTimer.Interval = _reportTimerDefaultInterval;

            _cdTimer.Elapsed += (s, e) =>
            {
                _repeatTimer.Start();
                _cdTimer.Stop();
            };
            _repeatTimer.Elapsed += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    DoPathCalculation(_currentPath);
                    if (_repeatTimer.Interval > _reportTimerDefaultInterval / 4)
                    {
                        _repeatTimer.Interval -= 1;
                    }
                });
            };
        }

        protected void PathDown(object sender, MouseButtonEventArgs e)
        {
            Path path = sender as Path;
            path.CaptureMouse();
            _currentPath = path;

            DoPathCalculation(path);
            _cdTimer.Start();
        }

        protected void DoPathCalculation(Path sender)
        {
            if (sender == _hourUp)
            {
                Time = Time.AddHours(1);
            }
            else if (sender == _hourDown)
            {
                Time = Time.AddHours(-1);
            }
            else if (sender == _minuteUp)
            {
                Time = Time.AddMinutes(1);
            }
            else if (sender == _minuteDown)
            {
                Time = Time.AddMinutes(-1);
            }
        }

        protected void PathUp(object sender, MouseButtonEventArgs e)
        {
            Path path = sender as Path;
            path.ReleaseMouseCapture();

            _cdTimer.Stop();
            _repeatTimer.Stop();

            _currentPath = null;
            Mouse.Capture(this, CaptureMode.SubTree);
        }

        protected virtual void OnTimeDateChanged(RoutedEventArgs e)
        {
            if (_timeChanged != null)
                _timeChanged(this, e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsPressed = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            IsPressed = false;
        }

        protected void RaiseTimeChanged()
        {
            RaisePropertyChanged(nameof(Time));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
