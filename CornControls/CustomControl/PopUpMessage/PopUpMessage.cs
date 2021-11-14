using PropertyChanged;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Data;
using System.Timers;

namespace CornControls.CustomControl
{
    [TemplatePart(Name = "PART_background", Type = typeof(Border)),
    TemplatePart(Name = "PART_innerPanel", Type = typeof(Grid)), AddINotifyPropertyChangedInterface]
    public class PopUpMessage : Control
    {
        public enum ButtonStyleEnum
        {
            CancleButton = 1,
            ConfirmButton = 2,
            YesButton = 4,
            NoButton = 8
        }

        public enum Result
        {
            Ok,
            Cancled,
            Error
        }

        private static PopUpMessage _instance;
        public static PopUpMessage Instance => _instance ??= new PopUpMessage();

        public static bool ShowErrorMessage(string title, string message)
        {
            Instance.Margin = new Thickness(7);
            Instance.IconPath = (Geometry)Application.Current.FindResource("errorIcon");
            Instance.IconHeight = 24;
            Instance.IconWidth = 24;
            Instance.Title = title;
            Instance.Message = message;
            Instance.ButtonStyle = PopUpMessage.ButtonStyleEnum.ConfirmButton;
            Instance.IsOpened = true;
            return true;
        }

        public static bool ShowMessage()
        {
            return Instance.IsOpened = true;
        }

        public static void CloseMessage()
        {
            Instance.IsOpened = false;
        }

        protected static AnimationTimeline CreateAnimation(double from, double to, double time, double amplitude, EasingMode mode, EventHandler whenDone = null)
        {
            IEasingFunction ease = new BackEase { Amplitude = amplitude, EasingMode = mode };
            Duration duration = new Duration(TimeSpan.FromSeconds(time / 2));
            DoubleAnimation anim = new DoubleAnimation(from, to, duration) { EasingFunction = ease };
            if (whenDone != null)
            {
                anim.Completed += whenDone;
            }

            anim.Freeze();
            return anim;
        }

        static PopUpMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopUpMessage), new FrameworkPropertyMetadata(typeof(PopUpMessage)));
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(PopUpMessage), new PropertyMetadata("This is a templated message"));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(PopUpMessage), new PropertyMetadata("Title"));
        public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(nameof(CanClose), typeof(bool), typeof(PopUpMessage), new PropertyMetadata(true));
        public static readonly DependencyProperty MessageResultProperty = DependencyProperty.Register(nameof(MessageResult), typeof(Result), typeof(PopUpMessage), new PropertyMetadata(Result.Ok));
        public static readonly DependencyProperty CurrentWindowProperty = DependencyProperty.Register(nameof(CurrentWindow), typeof(System.Windows.Window), typeof(PopUpMessage), new PropertyMetadata(null));

        public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register(nameof(IsOpened), typeof(bool), typeof(PopUpMessage), new PropertyMetadata(false));

        public static readonly DependencyProperty InAnimTimeProperty = DependencyProperty.Register(nameof(InAnimTime), typeof(double), typeof(PopUpMessage), new PropertyMetadata(0.75));
        public static readonly DependencyProperty OutAnimTimeProperty = DependencyProperty.Register(nameof(OutAnimTime), typeof(double), typeof(PopUpMessage), new PropertyMetadata(0.75));
        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(nameof(BackgroundOpacity), typeof(double), typeof(PopUpMessage), new PropertyMetadata(0.3));

        public static readonly DependencyProperty PanelHeightProperty = DependencyProperty.Register(nameof(PanelHeight), typeof(double), typeof(PopUpMessage), new PropertyMetadata(300.0));
        public static readonly DependencyProperty MidPanelWidthProperty = DependencyProperty.Register(nameof(MidPanelWidth), typeof(GridLength), typeof(PopUpMessage), new PropertyMetadata(new GridLength(600, GridUnitType.Pixel)));

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(nameof(ButtonStyle), typeof(ButtonStyleEnum), typeof(PopUpMessage), new PropertyMetadata(ButtonStyleEnum.ConfirmButton | ButtonStyleEnum.CancleButton));

        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(PopUpMessage), new PropertyMetadata(50.0));
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(PopUpMessage), new PropertyMetadata(50.0));
        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register(nameof(IconPath), typeof(Geometry), typeof(PopUpMessage));
        
        public PopUpMessage()
        {
            Timer timer = new Timer();
            timer.Interval = 250;
            timer.Elapsed += (s, e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.IsOpened = ViewModel.Helper.PopUpMessage.Instance.IsOpened;
                    this.Message = ViewModel.Helper.PopUpMessage.Instance.Message;
                });
            };
            timer.Start();
            // Fucking stupid ass solution ~~

            //DataContext = ViewModel.Helper.PopUpMessage.Instance;
            //Binding bindingIsOpen = new Binding(nameof(ViewModel.Helper.PopUpMessage.IsOpened));
            //Binding bindingMessage = new Binding(nameof(ViewModel.Helper.PopUpMessage.Message));
            //bindingIsOpen.Source = ViewModel.Helper.PopUpMessage.Instance;
            //bindingMessage.Source = ViewModel.Helper.PopUpMessage.Instance;
            //SetBinding(IsOpenedProperty, bindingIsOpen);
            //SetBinding(MessageProperty, bindingMessage);
        }

        protected bool Show()
        {
            if (IsOpened)
            {
                CloseMessage();
                return false;
            }

            Instance.Dispatcher.Invoke(() =>
            {
                CanClose = true;
                SetToCurrentMainWindow();

                AnimationTimeline inBackgroundAnim = CreateAnimation(0, Instance.BackgroundOpacity, Instance.InAnimTime, 0, EasingMode.EaseOut);
                AnimationTimeline inPanelAnim = CreateAnimation(0, Instance.PanelHeight, Instance.InAnimTime, 0, EasingMode.EaseOut);

                if (Instance._border != null)
                {
                    Instance._border.Opacity = 0;
                    Instance._border.BeginAnimation(OpacityProperty, inBackgroundAnim);
                }

                if (Instance._innerPanel != null)
                {
                    Instance._innerPanel.Height = 0;
                    Instance._innerPanel.BeginAnimation(HeightProperty, inPanelAnim);
                }

                Instance.Visibility = Visibility.Visible;
            }, DispatcherPriority.Background);

            return true;
        }

        protected void Close()
        {
            if (!CanClose || !IsOpened)
            {
                return;
            }

            Instance.Dispatcher.Invoke(() =>
            {
                AnimationTimeline outBackgroundAnim = CreateAnimation(BackgroundOpacity, 0, OutAnimTime, 0, EasingMode.EaseOut);
                AnimationTimeline outPanelAnim = CreateAnimation(PanelHeight, 0, OutAnimTime, 0, EasingMode.EaseOut, (s, e) => {
                    Visibility = Visibility.Hidden;
                    IsOpened = false;
                    CanClose = false;
                    RaiseMessageClosedEvent();
                });

                _border?.BeginAnimation(OpacityProperty, outBackgroundAnim);
                _innerPanel?.BeginAnimation(HeightProperty, outPanelAnim);
            }, DispatcherPriority.Background);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set
            {
                SetValue(MessageProperty, value);
                ViewModel.Helper.PopUpMessage.Instance.Message = value;
            }
        }
        public bool CanClose
        {
            get => (bool)GetValue(CanCloseProperty);
            set => SetValue(CanCloseProperty, value);
        }
        public Result MessageResult
        {
            get => (Result)GetValue(MessageResultProperty);
            set => SetValue(MessageResultProperty, value);
        }
        public System.Windows.Window CurrentWindow
        {
            get => (System.Windows.Window)GetValue(CurrentWindowProperty);
            private set => SetValue(CurrentWindowProperty, value);
        }

        public bool IsOpened
        {
            get => (bool)GetValue(IsOpenedProperty);
            set
            {
                if (value)
                    Show();
                else
                    Close();
                SetValue(IsOpenedProperty, value);
                ViewModel.Helper.PopUpMessage.Instance.IsOpened = value;
            }
        }

        public double OutAnimTime
        {
            get => (double)GetValue(OutAnimTimeProperty);
            set => SetValue(OutAnimTimeProperty, value);
        }
        public double InAnimTime
        {
            get => (double)GetValue(InAnimTimeProperty);
            set => SetValue(InAnimTimeProperty, value);
        }

        public double BackgroundOpacity
        {
            get => (double)GetValue(BackgroundOpacityProperty);
            set
            {
                double clampedValue = Math.Clamp(value, 0, 1);
                SetValue(BackgroundOpacityProperty, clampedValue);
            }
        }

        public double PanelHeight
        {
            get => (double)GetValue(PanelHeightProperty);
            set => SetValue(PanelHeightProperty, value);
        }
        public GridLength MidPanelWidth
        {
            get => (GridLength)GetValue(MidPanelWidthProperty);
            set => SetValue(MidPanelWidthProperty, value);
        }

        public ButtonStyleEnum ButtonStyle
        {
            get => (ButtonStyleEnum)GetValue(ButtonStyleProperty);
            set
            {
                SetValue(ButtonStyleProperty, value);
                CancelCommand.RaiseCanExecuteChangeEvent();
                YesCommand.RaiseCanExecuteChangeEvent();
                ConfirmCommand.RaiseCanExecuteChangeEvent();
                NoCommand.RaiseCanExecuteChangeEvent();
            }
        }

        public double IconHeight
        {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }
        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }
        public Geometry IconPath
        {
            get => (Geometry)GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }

        public static readonly RoutedEvent MessageClosedEvent = EventManager.RegisterRoutedEvent(nameof(MessageClosed), RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(PopUpMessage));

        public event RoutedEventHandler MessageClosed
        {
            add { AddHandler(MessageClosedEvent, value); }
            remove { RemoveHandler(MessageClosedEvent, value); }
        }

        private Border _border;
        private Grid _innerPanel;

        public void SetToCurrentMainWindow()
        {
            if (CurrentWindow != null && CurrentWindow != Application.Current.MainWindow)
            {
                Grid oldRootContent = CurrentWindow.Content as Grid;
                oldRootContent?.Children.Remove(this);
            }

            CurrentWindow = Application.Current.MainWindow;
            Visibility = Visibility.Hidden;

            Grid rootContent = CurrentWindow.Content as Grid;
            if (rootContent == null)
            {
                rootContent = new Grid();
                object content = CurrentWindow.Content;

                CurrentWindow.Content = null;
                _ = rootContent.Children.Add(content as UIElement);
                CurrentWindow.Content = rootContent;
            }

            if (!rootContent.Children.Contains(this))
            {
                _ = rootContent.Children.Add(this);
            }
        }

        protected virtual void RaiseMessageClosedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(MessageClosedEvent);
            RaiseEvent(args);
            OnMessageClosed(Instance);
        }

        protected virtual void OnMessageClosed(PopUpMessage instances)
        {}

        public override void OnApplyTemplate()
        {
            _border = Template.FindName("PART_background", this) as Border;
            _innerPanel = Template.FindName("PART_innerPanel", this) as Grid;

            base.OnApplyTemplate();
        }

        private RelayCommand<object> _confirmCommand;
        public RelayCommand<object> ConfirmCommand => _confirmCommand ??= new RelayCommand<object>(param => {
            MessageResult = Result.Ok;
            IsOpened = false;
        }, param => {
            return (ButtonStyle & ButtonStyleEnum.ConfirmButton) > 0;
        });
        private RelayCommand<object> _cancelCommand;
        public RelayCommand<object> CancelCommand => _cancelCommand ??= new RelayCommand<object>(param => {
            MessageResult = Result.Cancled;
            IsOpened = false;
        }, param => {
            return (ButtonStyle & ButtonStyleEnum.CancleButton) > 0;
        });

        private RelayCommand<object> _yesCommand;
        public RelayCommand<object> YesCommand => _yesCommand ??= new RelayCommand<object>(param => {
            MessageResult = Result.Ok;
            IsOpened = false;
        }, param => {
            return (ButtonStyle & ButtonStyleEnum.YesButton) > 0;
        });
        private RelayCommand<object> _noCommand;
        public RelayCommand<object> NoCommand => _noCommand ??= new RelayCommand<object>(param => {
            MessageResult = Result.Cancled;
            IsOpened = false;
        }, param => {
            return (ButtonStyle & ButtonStyleEnum.NoButton) > 0;
        });
    }
}
