using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using WpfScreenHelper;
using System.Windows.Interop;
using System.Threading.Tasks;

namespace CornControls.Window
{
    // This class is used for usercontrols that also provide window functionality for blank window
    public class UserControlWindow : UserControl
    {
        private BlankWindow _ownerWindow = null;
        public BlankWindow OwnerWindow
        {
            get
            {
                return _ownerWindow;
            }
        }

        public double DelayBeforeSetWndSize { get; set; } = 1.5;

        public UserControlWindow()
        {
            IsVisibleChanged += (s, e) =>
            {
                if (_ownerWindow == null)
                {
                    _ownerWindow = Helper.GetParentWindow(this) as BlankWindow;
                    OnGetWindowParent();
                }
            };
        }

        public virtual void SetWindowSize(double width, double height, double maxWidth = double.PositiveInfinity, double maxHeight = double.PositiveInfinity, double minWidth = 0, double minHeight = 0)
        {
            if (DelayBeforeSetWndSize <= 0)
                SetWindowSizePrivate(width, height, maxWidth, maxHeight, minWidth, minHeight);
            else
                Task.Delay((int)(DelayBeforeSetWndSize * 1000)).ContinueWith( t => {
                        this.Dispatcher.Invoke( () => {
                            SetWindowSizePrivate(width, height, maxWidth, maxHeight, minWidth, minHeight);
                        });
                    });
        }

        private void SetWindowSizePrivate(double width, double height, double maxWidth = double.PositiveInfinity, double maxHeight = double.PositiveInfinity, double minWidth = 0, double minHeight = 0)
        {
            var wnd = OwnerWindow;

            double sumWidth = 0, sumHeight = 0;
            foreach (var screen in Screen.AllScreens)
            {
                sumWidth += screen.Bounds.Width;
                sumHeight += screen.Bounds.Height;
            }

            wnd.MaxWidth = maxWidth;
            wnd.MaxHeight = maxHeight;

            var posX = wnd.Left + wnd.Width / 2;
            var posY = wnd.Top + wnd.Height / 2;

            wnd.MinHeight = minHeight;
            wnd.MinWidth = minWidth;

            wnd.Height = height;
            wnd.Width = width;

            var left = posX - wnd.Width / 2;
            var top = posY - wnd.Height / 2;

            if (wnd.WindowState == System.Windows.WindowState.Normal)
            {
                left = left < 0 ? 0 : left;
                top = top < 0 ? 0 : top;

                left = left + wnd.Width > sumWidth ? sumWidth - wnd.Width : left;
                top = top + wnd.Height > sumHeight ? sumHeight - wnd.Height : top;
            }

            wnd.Top = top;
            wnd.Left = left;
        }

        public virtual void OnTopMouseDown(object sender, MouseEventArgs e)
        {
            OwnerWindow.OnTopMouseDown(sender, e);
        }

        public virtual void WindowResize_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OwnerWindow.WindowResize_OnPreviewMouseDown(sender, e);
        }

        public virtual void WindowResize_OnMouseEnter(object sender, MouseEventArgs e)
        {
            OwnerWindow.WindowResize_OnMouseEnter(sender, e);
        }

        public virtual void WindowResize_OnMouseLeave(object sender, MouseEventArgs e)
        {
            OwnerWindow.WindowResize_OnMouseLeave(sender, e);
        }

        public virtual void Window_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            OwnerWindow.Window_OnPreviewMouseMove(sender, e);
        }

        public virtual void OnMinimizeClicked(object sender, EventArgs e)
        {
            OwnerWindow.OnMinimizeClicked(sender, e);
        }

        public virtual void OnMaximizeClicked(object sender, EventArgs e)
        {
            OwnerWindow.OnMaximizeClicked(sender, e);
        }

        public virtual void OnCloseClicked(object sender, EventArgs e)
        {
            OwnerWindow.OnCloseClicked(sender, e);
        }

        protected virtual void OnGetWindowParent() { }
    }
}
