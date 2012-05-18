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

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static partial class OS
        {
            /// <param name="ewxCode"></param>
            /// <returns>1 true if success shutdown else 0</returns>
            public static bool Shutdown(EWX ewxCode)
            {
                switch (ewxCode)
                {
                    case EWX.EWX_FORCE:
                    case EWX.EWX_LOGOFF:
                    case EWX.EWX_POWEROFF:
                    case EWX.EWX_SHUTDOWN:
                        {
                            DllImportCaller.lib.ShutdownOS(8);
                            return true;
                        }
                    case EWX.EWX_REBOOT:
                        {
                            DllImportCaller.lib.ShutdownOS(2);
                            return true;
                        }
                    default:
                        {
                            return false;
                        }
                }
                //uint v = (uint)ewxCode; //this cant be inside ShutdownOS( ... ) below as "ShutdownOS((uint)ewxCode)". Fails then... !??!?!
                //return DllImportCaller.lib.ShutdownOS((uint)ewxCode) == 1;
                //DllImportCaller.lib.ShutdownOS(8);
                //return true;
            }
            //public static DateTime SystemTime
            //{
            //    set
            //    {
            //        var res = DllImportCaller.lib.SetSystemTime7((short)value.Year, (short)value.Month, (short)value.DayOfWeek, (short)value.Day, (short)value.Hour, (short)value.Minute, (short)value.Second, (short)value.Millisecond);
            //        var e = DllImportCaller.LastError();
            //    }
            //}

            public static LANGID OSLanguage
            {
                get
                {
                    var lang = DllImportCaller.lib.VoidCall("coredll", "GetSystemDefaultUILanguage");

                    LANGID id = new LANGID { DATA = lang };

                    return id;
                }
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = 8)]
            public struct LANGID
            {
                [System.Runtime.InteropServices.FieldOffset(0)]
                public int DATA;
                [System.Runtime.InteropServices.FieldOffset(0)]
                public byte SubLanguageID;
                [System.Runtime.InteropServices.FieldOffset(1)]
                public byte PrimaryLanguageID;
            }

            public static bool IsFullBoot
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("shcore", "IsFullBoot") == 1;
                }
            }
            public static bool IsShellReady
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("shcore", "IsShellReady") == 1;
                }
            }
            public static TimeSpan Uptime
            {
                get
                {
                    var part0 = DllImportCaller.lib.NdisGetSystemUpTimeEx7(0);
                    var part1 = DllImportCaller.lib.NdisGetSystemUpTimeEx7(1);

                    LARGE_INTEGER integer = new LARGE_INTEGER();
                    integer.HighPart = part0;
                    integer.LowPart = (uint)part1;


                    //long sec0 = part0 / 1000;
                    //long sec1 = part1 / 1000;
                    //var val = sec0 * (2 ^ 32) + sec1;

                    //var l = (((long)(part0) << 32) + (long)part1);

                    return new TimeSpan(0, 0, 0, ((int)integer.QuadPart / 1000));
                }
            }
            public static int GetSystemDefaultLangID()
            {
                return DllImportCaller.lib.VoidCall("coredll", "GetSystemDefaultLangID");
            }
            public static int GetSystemDefaultUILanguage()
            {
                return DllImportCaller.lib.VoidCall("coredll", "GetSystemDefaultUILanguage");
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = 8)]
            private struct LARGE_INTEGER
            {
                [System.Runtime.InteropServices.FieldOffset(0)]
                public Int64 QuadPart;
                [System.Runtime.InteropServices.FieldOffset(0)]
                public UInt32 LowPart;
                [System.Runtime.InteropServices.FieldOffset(4)]
                public Int32 HighPart;
            }

       

            public class StartUpItem
            {
                public string Name;
                public string Launches;
            }

            public static StartUpItem[] GetSystemStartupItems()
            {
                var dir = Phone.IO.Directory.OpenDirectory(@"\Windows\StartUp");
                var files = dir.GetFiles();

                StartUpItem[] items = new StartUpItem[files.Length];

                for (int i = 0; i < files.Length; i++)
                {
                    var data = files[i];
                    items[i] = new StartUpItem { Name = data.FileName, Launches = Phone.IO.File7.ReadAllString(data.FullFileName) };
                }

                return items;
            }

            public static int GetDesktopWindowHandle()
            {
                return DllImportCaller.lib.VoidCall("coredll", "GetDesktopWindow");
            }

            public static int IdleTimeMS
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("coredll", "GetIdleTime");
                }
            }
            /// <summary>
            /// If OS has security policy enabled. If true, your screwed, aka blocked from IO and non signed COM's
            /// </summary>
            public static bool CePolicyIsEnabled
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("coredll", "CePolicyIsEnabled") == 1;
                }
            }

            /// <summary>
            /// Probably 480x800 due thats WP7 standards
            /// </summary>
            public static Size ScreenSize
            {
                get
                {
                    var x = DllImportCaller.lib.IntCall("coredll", "GetSystemMetrics", 0);
                    var y = DllImportCaller.lib.IntCall("coredll", "GetSystemMetrics", 1);

                    return new Size(x, y);
                }
            }

            public static class Security
            {
                //S-1-5-112-0-0X00- = TCB Chamber Group
                //S-1-5-112-0-0X80 = Least Privilege Chamber Group
                //S-1-5-112-0-0X22 = Standard Rights Identity Group
                //S-1-5-112-0-0X21 = VSelf-Isolated Standard Rights Chamber Groupl (HERE ARE WE)

                public static int CeCreateTokenFromAccount(string account_S_1_5etc) 
                {
                    if (account_S_1_5etc == null) throw new ArgumentNullException("account_S_1_5etc");

                    return DllImportCaller.lib.StringCall("coredll", "CeCreateTokenFromAccount", account_S_1_5etc);//"S-1-5-112-0-0X80");
                }

                [Obsolete("not working to crack into TCB yet.", true)]
                public static void Crack(Action a)
                {
                    if (a != null)
                    {
                        if (Phone.TaskHost.CeImpersonateCurrentProcess())
                        {
                            var ta = Phone.TaskManager.GetAllRunningSilverligt_XNA();

                            var token = Phone.OS.Security.CeCreateTokenFromAccount("S-1-5-112-0-0X00");

                            //BOOL WINAPI SetThreadToken(
                            //  __in_opt  PHANDLE Thread,
                            //  __in_opt  HANDLE Token
                            //);
                            var thr = (int)ta[0].GetWindowThreadProcessId();
                            var t = DllImportCaller.lib.IntDualCall("coredll", "SetThreadToken", thr, token);

                            a();

                            var rev = Phone.TaskHost.CeRevertToSelf();
                        }
                    }
                }
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public class MEMORYSTATUS
            {
                /// <summary>
                /// Size of the MEMORYSTATUS data structure, in bytes. You do not need to set this member before calling the GlobalMemoryStatus function; the function sets it. 
                /// </summary>
                public uint dwLength;

                /// <summary>
                /// Number between 0 and 100 that specifies the approximate percentage of physical memory that is in use (0 indicates no memory use and 100 indicates full memory use). 
                /// Windows NT:  Percentage of approximately the last 1000 pages of physical memory that is in use.
                /// </summary>
                public uint dwMemoryLoad;

                /// <summary>
                /// Total size of physical memory, in bytes. 
                /// </summary>
                public uint dwTotalPhys;

                /// <summary>
                /// Size of physical memory available, in bytes
                /// </summary>
                public uint dwAvailPhys;

                /// <summary>
                /// Size of the committed memory limit, in bytes. 
                /// </summary>
                public uint dwTotalPageFile;

                /// <summary>
                /// Size of available memory to commit, in bytes. 
                /// </summary>
                public uint dwAvailPageFile;

                /// <summary>
                /// Total size of the user mode portion of the virtual address space of the calling process, in bytes. 
                /// </summary>
                public uint dwTotalVirtual;

                /// <summary>
                /// Size of unreserved and uncommitted memory in the user mode portion of the virtual address space of the calling process, in bytes. 
                /// </summary>
                public uint dwAvailVirtual;

                public override string ToString()
                {
                    return this.DumpToString();
                }

            } // class MEMORYSTATUS

            public static partial class Memory
            {
                public static MEMORYSTATUS GetMemoryStatus()
                {
                    string status = "";
                    var ta = DllImportCaller.lib.GlobalMemoryStatus7(ref status);

                    var s = status.Split('\n');

                    return new MEMORYSTATUS
                    {
                        dwLength = uint.Parse(s[0]),
                        dwMemoryLoad = uint.Parse(s[1]),
                        dwTotalPhys = uint.Parse(s[2]),
                        dwAvailPhys = uint.Parse(s[3]),
                        dwTotalPageFile = uint.Parse(s[4]),
                        dwAvailPageFile = uint.Parse(s[5]),
                        dwTotalVirtual = uint.Parse(s[6]),
                        dwAvailVirtual = uint.Parse(s[7])
                    };
                }
            }

            /// <summary>
            /// http://msdn.microsoft.com/en-us/library/ms724385(v=vs.85).aspx
            /// </summary>
            public enum SystemMetric : int
            {
                SM_CXSCREEN = 0,
                SM_CYSCREEN = 1,
                SM_CXVSCROLL = 2,
                SM_CYHSCROLL = 3,
                SM_CYCAPTION = 4,
                SM_CXBORDER = 5,
                SM_CYBORDER = 6,
                SM_CXDLGFRAME = 7,
                SM_CYDLGFRAME = 8,
                SM_CYVTHUMB = 9,
                SM_CXHTHUMB = 10,
                SM_CXICON = 11,
                SM_CYICON = 12,
                SM_CXCURSOR = 13,
                SM_CYCURSOR = 14,
                SM_CYMENU = 15,
                SM_CXFULLSCREEN = 16,
                SM_CYFULLSCREEN = 17,
                SM_CYKANJIWINDOW = 18,
                SM_MOUSEPRESENT = 19,
                SM_CYVSCROLL = 20,
                SM_CXHSCROLL = 21,
                SM_DEBUG = 22,
                SM_SWAPBUTTON = 23,
                SM_RESERVED1 = 24,
                SM_RESERVED2 = 25,
                SM_RESERVED3 = 26,
                SM_RESERVED4 = 27,
                SM_CXMIN = 28,
                SM_CYMIN = 29,
                SM_CXSIZE = 30,
                SM_CYSIZE = 31,
                SM_CXFRAME = 32,
                SM_CYFRAME = 33,
                SM_CXMINTRACK = 34,
                SM_CYMINTRACK = 35,
                SM_CXDOUBLECLK = 36,
                SM_CYDOUBLECLK = 37,
                SM_CXICONSPACING = 38,
                SM_CYICONSPACING = 39,
                SM_MENUDROPALIGNMENT = 40,
                SM_PENWINDOWS = 41,
                SM_DBCSENABLED = 42,
                SM_CMOUSEBUTTONS = 43,

                //#if(WINVER >= 0x0400)
                SM_CXFIXEDFRAME = SM_CXDLGFRAME,  /* ;win40 name change */
                SM_CYFIXEDFRAME = SM_CYDLGFRAME,  /* ;win40 name change */
                SM_CXSIZEFRAME = SM_CXFRAME,    /* ;win40 name change */
                SM_CYSIZEFRAME = SM_CYFRAME,     /* ;win40 name change */

                SM_SECURE = 44,
                SM_CXEDGE = 45,
                SM_CYEDGE = 46,
                SM_CXMINSPACING = 47,
                SM_CYMINSPACING = 48,
                SM_CXSMICON = 49,
                SM_CYSMICON = 50,
                SM_CYSMCAPTION = 51,
                SM_CXSMSIZE = 52,
                SM_CYSMSIZE = 53,
                SM_CXMENUSIZE = 54,
                SM_CYMENUSIZE = 55,
                SM_ARRANGE = 56,
                SM_CXMINIMIZED = 57,
                SM_CYMINIMIZED = 58,
                SM_CXMAXTRACK = 59,
                SM_CYMAXTRACK = 60,
                SM_CXMAXIMIZED = 61,
                SM_CYMAXIMIZED = 62,
                SM_NETWORK = 63,
                SM_CLEANBOOT = 67,
                SM_CXDRAG = 68,
                SM_CYDRAG = 69,
                //#endif /* WINVER >= 0x0400 */
                SM_SHOWSOUNDS = 70,
                //#if(WINVER >= 0x0400)
                SM_CXMENUCHECK = 71,   /* Use instead of GetMenuCheckMarkDimensions()! */
                SM_CYMENUCHECK = 72,
                SM_SLOWMACHINE = 73,
                SM_MIDEASTENABLED = 74,
                //#endif /* WINVER >= 0x0400 */

                //#if (WINVER >= 0x0500) || (_WIN32_WINNT >= 0x0400)
                SM_MOUSEWHEELPRESENT = 75,
                //#endif
                //#if(WINVER >= 0x0500)
                SM_XVIRTUALSCREEN = 76,
                SM_YVIRTUALSCREEN = 77,
                SM_CXVIRTUALSCREEN = 78,
                SM_CYVIRTUALSCREEN = 79,
                SM_CMONITORS = 80,
                SM_SAMEDISPLAYFORMAT = 81,
                //#endif /* WINVER >= 0x0500 */
                //#if(_WIN32_WINNT >= 0x0500)
                SM_IMMENABLED = 82,

                #region Unsupported
                //#endif /* _WIN32_WINNT >= 0x0500 */
                //#if(_WIN32_WINNT >= 0x0501)
                //SM_CXFOCUSBORDER = 83,
                //SM_CYFOCUSBORDER = 84,
                ////#endif /* _WIN32_WINNT >= 0x0501 */

                ////#if(_WIN32_WINNT >= 0x0501)
                //SM_TABLETPC = 86,
                //SM_MEDIACENTER = 87,
                //SM_STARTER = 88,
                //SM_SERVERR2 = 89,
                ////#endif /* _WIN32_WINNT >= 0x0501 */

                ////#if(_WIN32_WINNT >= 0x0600)
                //SM_MOUSEHORIZONTALWHEELPRESENT = 91,
                //SM_CXPADDEDBORDER = 92,
                ////#endif /* _WIN32_WINNT >= 0x0600 */

                ////#if(WINVER >= 0x0601)

                //SM_DIGITIZER = 94,
                //SM_MAXIMUMTOUCHES = 95,
                ////#endif /* WINVER >= 0x0601 */

                ////#if (WINVER < 0x0500) && (!defined(_WIN32_WINNT) || (_WIN32_WINNT < 0x0400))
                ////////SM_CMETRICS = 76,
                //////////#elif WINVER == 0x500
                ////////SM_CMETRICS = 83,
                //////////#elif WINVER == 0x501
                ////////SM_CMETRICS = 91,
                //////////#elif WINVER == 0x600
                ////////SM_CMETRICS = 93,
                ////#else
                //SM_CMETRICS = 97,
                ////#endif

                ////#if(WINVER >= 0x0500)
                //SM_REMOTESESSION = 0x1000,


                ////#if(_WIN32_WINNT >= 0x0501)
                //SM_SHUTTINGDOWN = 0x2000,
                ////#endif /* _WIN32_WINNT >= 0x0501 */

                ////#if(WINVER >= 0x0501)
                //SM_REMOTECONTROL = 0x2001,
                ////#endif /* WINVER >= 0x0501 */

                ////#if(WINVER >= 0x0501)
                //SM_CARETBLINKINGENABLED = 0x2002
                //#endif /* WINVER >= 0x0501 */

                //#endif /* WINVER >= 0x0500 */
                #endregion
            }
            /// <summary>
            /// http://msdn.microsoft.com/en-us/library/ms724385(v=vs.85).aspx
            /// </summary>
            public static int GetSystemMetrics(SystemMetric nIndex)
            {
                int bughandler = (int)nIndex;
                return DllImportCaller.lib.IntCall("coredll", "GetSystemMetrics", bughandler);
            }
        }
    }
}
