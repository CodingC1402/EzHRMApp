using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace CornControls.CustomControl
{
    public class PopUpMessage : UserControl
    {
        public enum Result
        {
            Ok,
            Cancled,
            Error
        }

        private static PopUpMessage _instance = null;
        public static PopUpMessage Instance
        {
            get => _instance ?? (_instance = new PopUpMessage());
        }

        public static Result ShowMessage()
        {
            if (Instance.IsOpened == true)
            {
                CloseMessage(Result.Error);
            }

            Instance.IsOpened = true;
            Instance.CurrentWindow = Application.Current.MainWindow;
            Instance.CanClose = true;

            Grid rootContent = Instance.CurrentWindow.Content as Grid;
            if (rootContent == null)
            {
                rootContent = new Grid();
                object content = Instance.CurrentWindow.Content;

                Instance.CurrentWindow.Content = null;
                rootContent.Children.Add(content as UIElement);
                Instance.CurrentWindow.Content = rootContent;
            }
            rootContent.Children.Add(Instance);

            return Result.Ok;
        }

        public static void CloseMessage(Result result)
        {
            if (!Instance.CanClose)
                return;

            Grid rootContent = Instance.CurrentWindow.Content as Grid;
            rootContent.Children.Remove(Instance);

            Instance.MessageResult = Result.Ok;
            Instance.IsOpened = false;
            Instance.RaiseMessageClosedEvent();
            Instance.CurrentWindow = null;
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

        public static readonly DependencyProperty InAnimTimeProperty = DependencyProperty.Register(nameof(InAnimTime), typeof(double), typeof(PopUpMessage), new PropertyMetadata(1.0));
        public static readonly DependencyProperty OutAnimTimeProperty = DependencyProperty.Register(nameof(OutAnimTime), typeof(double), typeof(PopUpMessage), new PropertyMetadata(1.0));
       
        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(nameof(BackgroundOpacity), typeof(double), typeof(PopUpMessage), new PropertyMetadata(0.5));

        public static readonly DependencyProperty PanelHeightProperty = DependencyProperty.Register(nameof(PanelHeight), typeof(double), typeof(PopUpMessage), new PropertyMetadata(300.0));
        public static readonly DependencyProperty MidPanelWidthProperty = DependencyProperty.Register(nameof(MidPanelWidth), typeof(GridLength), typeof(PopUpMessage), new PropertyMetadata(new GridLength(600, GridUnitType.Pixel)));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
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
            set => SetValue(CurrentWindowProperty, value);
        }

        public bool IsOpened
        {
            get => (bool)GetValue(IsOpenedProperty);
            set => SetValue(IsOpenedProperty, value);
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
                var clampedValue = Math.Clamp(value, 0, 1);
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
        public static readonly RoutedEvent MessageClosedEvent = EventManager.RegisterRoutedEvent(nameof(MessageClosed), RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(PopUpMessage));

        public event RoutedEventHandler MessageClosed
        {
            add { AddHandler(MessageClosedEvent, value); }
            remove { RemoveHandler(MessageClosedEvent, value); }
        }

        private Border _border;
        private Grid _innerPanel;

        protected virtual void RaiseMessageClosedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(MessageClosedEvent);
            RaiseEvent(args);
            OnMessageClosed(Instance);
        }

        protected virtual void OnMessageClosed(PopUpMessage instances)
        {}

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
            _border = newTemplate.FindName("PART_background", this) as Border;
            _innerPanel = newTemplate.FindName("PART_innerPanel", this) as Grid;
        }

        protected virtual AnimationTimeline CreateAnimation(double from, double to, double time, double amplitude, EasingMode mode, EventHandler whenDone = null)
        {
            IEasingFunction ease = new BackEase { Amplitude = amplitude, EasingMode = mode };
            var duration = new Duration(TimeSpan.FromSeconds(time / 2));
            var anim = new DoubleAnimation(from, to, duration) { EasingFunction = ease };
            if (whenDone != null)
                anim.Completed += whenDone;
            anim.Freeze();
            return anim;
        }
    }
}
