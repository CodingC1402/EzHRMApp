using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
            Instance.RaiseMessageClosedEvent();
            Instance.CurrentWindow = null;
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(PopUpMessage), new PropertyMetadata("Message"));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(PopUpMessage), new PropertyMetadata("Title"));
        public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(nameof(CanClose), typeof(bool), typeof(PopUpMessage), new PropertyMetadata(true));
        public static readonly DependencyProperty MessageResultProperty = DependencyProperty.Register(nameof(MessageResult), typeof(Result), typeof(PopUpMessage), new PropertyMetadata(Result.Ok));
        public static readonly DependencyProperty CurrentWindowProperty = DependencyProperty.Register(nameof(CurrentWindow), typeof(System.Windows.Window), typeof(PopUpMessage), new PropertyMetadata(null));
       
        public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register(nameof(IsOpened), typeof(bool), typeof(PopUpMessage), new PropertyMetadata(false));

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

        public static readonly RoutedEvent MessageClosedEvent = EventManager.RegisterRoutedEvent(nameof(MessageClosed), RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(PopUpMessage));

        public event RoutedEventHandler MessageClosed
        {
            add { AddHandler(MessageClosedEvent, value); }
            remove { RemoveHandler(MessageClosedEvent, value); }
        }

        protected virtual void RaiseMessageClosedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(MessageClosedEvent);
            RaiseEvent(args);
            OnMessageClosed(Instance);
        }

        protected virtual void OnMessageClosed(PopUpMessage instances)
        {}
    }
}
