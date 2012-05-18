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
using System.Linq;
using System.Collections.Generic;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public class IExplorer
        {
            /// <summary>
            /// NFO: Minimum tab count is 1 (Is always blank) (Except on boot = 0)
            /// </summary>
            public static int OpenTabsCount
            {
                get
                {
                    var onlyTitle = GetTabsWindowPointers().Count();

                    return onlyTitle == 0 ? 0 : onlyTitle - 1;
                }
            }

            public static bool IEStarted
            {
                get
                {
                    return OpenTabsCount != 0;
                }
            }

            public static IETab[] GetTabs()
            {
                var tabs = GetTabsWindowPointers();

                return tabs.Select(tab => new IETab(tab)).ToArray();
                //var a = tabs.Select(tab =>
                //{
                //    WM IEM_PROPCHANGE = (WM)(((int)WM.WM_USER) + 338);
                //    return Phone.OS.SendMessage(tab.HWND, 0x552, 0, 0);

                //}).ToArray();
            }

            public static IEnumerable<Phone.TaskManager.WindowPointer> GetTabsWindowPointers()
            {
                return Phone.TaskManager.GetEnumWindows(false, false).Where(sa => sa.GetClassName().Contains("Internet Explorer_Hidden"));
            }

            public class IETab
            {
                public Phone.TaskManager.WindowPointer TabPointer { get; private set; }
                public IETab(Phone.TaskManager.WindowPointer windowHwnd)
                {
                    this.TabPointer = windowHwnd;
                }

                public int Redraw()
                {
                    return Phone.OS.SendMessage(TabPointer.HWND, 0x552, 0, 0) & Phone.OS.SendMessage(TabPointer.HWND, WM.WM_PAINT, 0, 0);
                }
                public int SendSettingsChanged()
                {
                    return Phone.OS.SendMessage(TabPointer.HWND, WM.WM_SETTINGCHANGE, 0, 0);
                }

                //[Obsolete("Working on it.", true)]
                //public int Refresh()
                //{
                //    return 0;
                //}
            }
        }  
    }
}
