using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SXUtils.Native
{
    public static class User32
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public static bool IsMinimized(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                return false;

            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            GetWindowPlacement(handle, ref placement);

            return placement.showCmd == 2;
        }

        #region Pixel 
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        public static Color GetPixelColor(IntPtr hwnd, int x, int y)
        {
            IntPtr hdc = GetDC(hwnd);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(hwnd, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                            (int)(pixel & 0x0000FF00) >> 8,
                            (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
        #endregion

        #region Messages 
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        #endregion

        #region Window stuff 
        [DllImport("User32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public RECT(int x = 0, int y = 0, int x2 = 0, int y2 = 0)
            {
                Left = x;
                Top = y;
                Right = x2;
                Bottom = y2;
            }

            public bool IsNull
            {
                get
                {
                    if (Left == 0 && Top == 0 && Right == 0 && Bottom == 0)
                        return true;

                    return false;
                }
            }

            public RECT Null()
            {
                return new RECT(0, 0, 0, 0);
            }

            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        // Define the FindWindow API function.
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        // Define the SetWindowPos API function.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            SetWindowPosFlags uFlags);

        // Define the SetWindowPosFlags enumeration.
        [Flags()]
        public enum SetWindowPosFlags : uint
        {
            SynchronousWindowPosition = 0x4000,
            DeferErase = 0x2000,
            DrawFrame = 0x0020,
            FrameChanged = 0x0020,
            HideWindow = 0x0080,
            DoNotActivate = 0x0010,
            DoNotCopyBits = 0x0100,
            IgnoreMove = 0x0002,
            DoNotChangeOwnerZOrder = 0x0200,
            DoNotRedraw = 0x0008,
            DoNotReposition = 0x0200,
            DoNotSendChangingEvent = 0x0400,
            IgnoreResize = 0x0001,
            IgnoreZOrder = 0x0004,
            ShowWindow = 0x0040,
        }
        #endregion
    }
}

