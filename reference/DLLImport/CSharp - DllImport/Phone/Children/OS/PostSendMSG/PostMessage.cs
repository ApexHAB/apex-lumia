using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static partial class OS
        {
            //[DllImport("user32.dll", CharSet = CharSet.Auto)]
            //static extern IntPtr PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

            public static int PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam)
            {
                return Phone.OS.PostMessage((int)hWnd, (uint)Msg, (int)wParam, (int)lParam);
            }

            public static int PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam)
            {
                return Phone.OS.PostMessage((int)hWnd, (uint)Msg, (int)wParam, (int)lParam);
            }

            public static int PostMessage(int hWnd, uint Msg, int wParam, int lParam)
            {
                return DllImportCaller.lib.PostMessage7(hWnd, Msg, (uint)wParam, (uint)lParam);
            }
            public static int PostMessage(int hWnd, WM Msg, int wParam, int lParam)
            {
                return DllImportCaller.lib.PostMessage7(hWnd, (uint)Msg, (uint)wParam, (uint)lParam);
            }
        }
    }
}
