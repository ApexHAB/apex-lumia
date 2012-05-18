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
            //static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

            public static int SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam)
            {
                return Phone.OS.SendMessage((int)hWnd, (uint)Msg, (int)wParam, (int)lParam);
            }

            public static int SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam)
            {
                return Phone.OS.SendMessage((int)hWnd, (uint)Msg, (int)wParam, (int)lParam);
            }

            public static int SendMessage(int hWnd, uint Msg, int wParam, int lParam)
            {
                return DllImportCaller.lib.SendMessage7(hWnd, Msg, (uint)wParam, (uint)lParam);
            }
            public static int SendMessage(int hWnd, WM Msg, int wParam, int lParam)
            {
                return DllImportCaller.lib.SendMessage7(hWnd, (uint)Msg, (uint)wParam, (uint)lParam);
            }
        }
    }
}
