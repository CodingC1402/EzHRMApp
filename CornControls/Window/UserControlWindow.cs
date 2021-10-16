using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

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
