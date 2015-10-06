using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;




namespace GoogleEarthControl
{
    public class Win32
    {
        [DllImport("user32", CharSet = CharSet.Auto)]
        public extern static IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public extern static bool MoveWindow(IntPtr hWnd, 
            int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public extern static IntPtr SetParent(IntPtr hWndChild, 
            IntPtr hWndNewParent);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public extern static IntPtr PostMessage(int hWnd, 
            int msg, int wParam, int IParam);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public extern static bool SetWindowPos(int hWnd, 
            IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("coredll.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        public static readonly Int32  WM_QUIT           = 0x0012;
        public static readonly IntPtr HWND_TOP          = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM       = new IntPtr(1);
        public static readonly UInt32 SWP_HIDEWINDOW    = 128;
        public static readonly UInt32 SWP_SHOWWINDOW    = 64;
        public static readonly uint   WM_SYSCOMMAND     = 0x0112;
        public static readonly int    SC_CLOSE          = 0xF060;

        public static IntPtr GEHrender = (IntPtr)5;
    }
}
