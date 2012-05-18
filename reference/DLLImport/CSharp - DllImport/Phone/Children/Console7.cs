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
        public class Console7
        {
            public static int GetForegroundWindow()
            {
                return DllImportCaller.lib.VoidCall("coredll", "GetForegroundWindow");
            }
            public static string Title
            {
                get
                {
                    string result = "";
                    DllImportCaller.lib.WindowTextOfHWND(Console7.GetForegroundWindow(), ref result, false);
                    return result;
                }
                set
                {
                    string newVal = value;
                    DllImportCaller.lib.WindowTextOfHWND(Console7.GetForegroundWindow(), ref newVal, true);
                }
            }
            public static uint GetUIThreadIdFor(int hwnd)
            {
                var val = DllImportCaller.lib.UintCall("coredll", "GetWindowThreadProcessId", (uint)hwnd);

                return (uint)val;
            }
            public static uint GetThisTaskHostUIThread()
            {
                return Console7.GetUIThreadIdFor(Console7.GetForegroundWindow());
            }
        }
    }
}
