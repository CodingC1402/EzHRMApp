using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using WpfScreenHelper;

namespace CornControls.Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class BlankWindow : System.Windows.Window
    {
        protected HwndSource _hwnd = null;
        public HwndSource Hwnd { get => _hwnd; }

        protected Screen _currentScreen;
        protected Thickness _offSet = new Thickness(-3, -3, -3, -3);

        protected Rect _restoreBound = new Rect();

        public new WindowState WindowState
        {
            get => base.WindowState;
            set
            {
                switch (value)
                {
                    case WindowState.Normal:
                        SendMessage(_hwnd.Handle, (uint)Message.WM_SYSCOMMAND, (IntPtr)0xF120, (IntPtr)0);
                        break;
                    case WindowState.Minimized:
                        SendMessage(_hwnd.Handle, (uint)Message.WM_SYSCOMMAND, (IntPtr)0xF020, (IntPtr)0);
                        break;
                    case WindowState.Maximized:
                        SendMessage(_hwnd.Handle, (uint)Message.WM_SYSCOMMAND, (IntPtr)0xF030, (IntPtr)0);
                        break;
                }
            }
        }

        protected enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        protected static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        #region Events
        public static readonly RoutedEvent WindowStateChangedEvent = EventManager.RegisterRoutedEvent(nameof(WindowStateChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<WindowState>), typeof(BlankWindow));
        public event RoutedPropertyChangedEventHandler<WindowState> WindowStateChanged
        {
            add { AddHandler(WindowStateChangedEvent, value); }
            remove { RemoveHandler(WindowStateChangedEvent, value); }
        }
        #endregion

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _restoreBound.X = Left;
            _restoreBound.Y = Top;
            _restoreBound.Width = Width;
            _restoreBound.Height = Height;

            _hwnd = (HwndSource)PresentationSource.FromVisual(this);
            _hwnd.AddHook(WndProc);
        }

        protected virtual void OnTopMouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ReleaseMouseCapture();
                SendMessage(_hwnd.Handle, 161, (IntPtr)2, (IntPtr)0);
            }
        }

        protected virtual void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(_hwnd.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        protected virtual void WindowResize_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = (Rectangle)sender;
            if (rectangle == null) return;

            if (WindowState == WindowState.Maximized) return;

            switch (rectangle.Name)
            {
                case "WindowResizeTop":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "WindowResizeBottom":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "WindowResizeLeft":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "WindowResizeRight":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "WindowResizeTopLeft":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "WindowResizeTopRight":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "WindowResizeBottomLeft":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "WindowResizeBottomRight":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
            }
        }

        protected virtual void WindowResize_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = (Rectangle)sender;
            if (rectangle == null) return;

            if (WindowState == WindowState.Maximized) return;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (rectangle.Name)
            {
                case "WindowResizeTop":
                    Cursor = Cursors.SizeNS;
                    break;
                case "WindowResizeBottom":
                    Cursor = Cursors.SizeNS;
                    break;
                case "WindowResizeLeft":
                    Cursor = Cursors.SizeWE;
                    break;
                case "WindowResizeRight":
                    Cursor = Cursors.SizeWE;
                    break;
                case "WindowResizeTopLeft":
                    Cursor = Cursors.SizeNWSE;
                    break;
                case "WindowResizeTopRight":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "WindowResizeBottomLeft":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "WindowResizeBottomRight":
                    Cursor = Cursors.SizeNWSE;
                    break;
            }
        }

        protected virtual void WindowResize_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        protected virtual void Window_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                Cursor = Cursors.Arrow;
        }

        protected virtual void OnMinimizeClicked(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected virtual void OnMaximizeClicked(object sender, EventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        protected virtual void OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        protected virtual void OnWindowStateChanged(RoutedPropertyChangedEventArgs<WindowState> e)
        {
            RaiseEvent(e);
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((Message)msg)
            {
                case Message.WM_SIZE:
                    int intWParm = wParam.ToInt32();
                    if (intWParm == 2)
                    {
                        if (WindowState == WindowState.Normal)
                        {
                            _restoreBound.X = Left;
                            _restoreBound.Y = Top;
                            _restoreBound.Width = Width;
                            _restoreBound.Height = Height;

                            var rect = _currentScreen.WorkingArea;
                            Top = rect.Y + _offSet.Top;
                            Left = rect.X + _offSet.Left;
                            Height = rect.Height - (_offSet.Bottom + _offSet.Top);
                            Width = rect.Width - (_offSet.Left + _offSet.Right);
                        }

                        OnWindowStateChanged(new RoutedPropertyChangedEventArgs<WindowState>(WindowState, WindowState.Maximized, WindowStateChangedEvent));
                    }
                    else if (intWParm == 1)
                    {
                        OnWindowStateChanged(new RoutedPropertyChangedEventArgs<WindowState>(WindowState, WindowState.Minimized, WindowStateChangedEvent));
                    }
                    else if (intWParm == 0)
                    {
                        OnWindowStateChanged(new RoutedPropertyChangedEventArgs<WindowState>(WindowState, WindowState.Normal, WindowStateChangedEvent));
                    }
                    break;
                case Message.WM_MOVE:
                    _currentScreen = Screen.FromHandle(hwnd);
                    break;
                case Message.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }

        protected void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            if (WindowState != WindowState.Minimized)
                return;

            // Get the MINMAXINFO structure from memory location given by lParam
            NativeMethods.MINMAXINFO mmi =
                (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(NativeMethods.MINMAXINFO));

            var workRect = _currentScreen.WorkingArea;
            var rect = _currentScreen.Bounds;

            mmi.MaxSize.X = (int)(workRect.Width - (_offSet.Left + _offSet.Right));
            mmi.MaxSize.Y = (int)(workRect.Height - (_offSet.Top + _offSet.Bottom));
            mmi.MaxPosition.X = (int)(workRect.X - rect.X + _offSet.Left);
            mmi.MaxPosition.Y = (int)(workRect.Y - rect.Y + _offSet.Top);

            mmi.MaxTrackSize.X = mmi.MaxSize.X;
            mmi.MaxTrackSize.Y = mmi.MaxSize.Y;
            mmi.MinTrackSize.X = mmi.MaxSize.X;
            mmi.MinTrackSize.Y = mmi.MaxSize.Y;

            Marshal.StructureToPtr(mmi, lParam, true);
        }
    }
}
