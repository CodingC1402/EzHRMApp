using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzHRMApp.Helper
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Native Windows API methods and interfaces.
    /// </summary>
    internal static class NativeMethods
    {
        #region Constants
        // The WM_GETMINMAXINFO message is sent to a window when the size or
        // position of the window is about to change.
        // An application can use this message to override the window's
        // default maximized size and position, or its default minimum or
        // maximum tracking size.
        private const int WM_GETMINMAXINFO = 0x0024;

        // Constants used with MonitorFromWindow()
        // Returns NULL.
        private const int MONITOR_DEFAULTTONULL = 0;

        // Returns a handle to the primary display monitor.
        private const int MONITOR_DEFAULTTOPRIMARY = 1;

        // Returns a handle to the display monitor that is nearest to the window.
        private const int MONITOR_DEFAULTTONEAREST = 2;
        #endregion

        #region Structs
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// The MINMAXINFO structure contains information about a window's
        /// maximized size and position and its minimum and maximum tracking size.
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/ms632605%28VS.85%29.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT Reserved;
            public POINT MaxSize;
            public POINT MaxPosition;
            public POINT MinTrackSize;
            public POINT MaxTrackSize;
        }

        /// <summary>
        /// The WINDOWINFO structure contains window information.
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/ms632610%28VS.85%29.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            /// <summary>
            /// The size of the structure, in bytes.
            /// The caller must set this to sizeof(WINDOWINFO).
            /// </summary>
            public uint Size;

            /// <summary>
            /// Pointer to a RECT structure
            /// that specifies the coordinates of the window.
            /// </summary>
            public RECT Window;

            /// <summary>
            /// Pointer to a RECT structure
            /// that specifies the coordinates of the client area.
            /// </summary>
            public RECT Client;

            /// <summary>
            /// The window styles. For a table of window styles,
            /// <see cref="http://msdn.microsoft.com/en-us/library/ms632680%28VS.85%29.aspx">
            /// CreateWindowEx
            /// </see>.
            /// </summary>
            public uint Style;

            /// <summary>
            /// The extended window styles. For a table of extended window styles,
            /// see CreateWindowEx.
            /// </summary>
            public uint ExStyle;

            /// <summary>
            /// The window status. If this member is WS_ACTIVECAPTION,
            /// the window is active. Otherwise, this member is zero.
            /// </summary>
            public uint WindowStatus;

            /// <summary>
            /// The width of the window border, in pixels.
            /// </summary>
            public uint WindowBordersWidth;

            /// <summary>
            /// The height of the window border, in pixels.
            /// </summary>
            public uint WindowBordersHeight;

            /// <summary>
            /// The window class atom (see
            /// <see cref="http://msdn.microsoft.com/en-us/library/ms633586%28VS.85%29.aspx">
            /// RegisterClass
            /// </see>).
            /// </summary>
            public ushort WindowType;

            /// <summary>
            /// The Windows version of the application that created the window.
            /// </summary>
            public ushort CreatorVersion;
        }

        /// <summary>
        /// The MONITORINFO structure contains information about a display monitor.
        /// The GetMonitorInfo function stores information in a MONITORINFO structure.
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/dd145065%28VS.85%29.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int Size;
            public RECT Monitor;
            public RECT WorkArea;

            public uint Flags;
        }
        #endregion

        #region Imported methods
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
        #endregion

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            // Get the MINMAXINFO structure from memory location given by lParam
            MINMAXINFO mmi =
                (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                monitorInfo.Size = Marshal.SizeOf(typeof(MONITORINFO));
                GetMonitorInfo(monitor, ref monitorInfo);

                var wokringArea = monitorInfo.WorkArea;
                mmi.MaxPosition.X = wokringArea.Left;
                mmi.MaxPosition.Y = wokringArea.Top;
                mmi.MaxSize.X = wokringArea.Right - wokringArea.Left;
                mmi.MaxSize.Y = wokringArea.Bottom - wokringArea.Top;

                mmi.MinTrackSize.X = mmi.MaxSize.X;
                mmi.MinTrackSize.Y = mmi.MaxSize.Y;
                mmi.MaxTrackSize.X = mmi.MaxSize.X;
                mmi.MaxTrackSize.Y = mmi.MaxSize.Y;
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }
    }
}
