

using System;
using System.Linq;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static class TaskManager
        {
            public static long GetCurrentProcessId()
            {
                return DllImportCaller.lib.VoidCall("coredll", "GetProcessId");
            }

            public static long GetFocusHWND()
            {
                return DllImportCaller.lib.VoidCall("coredll", "GetFocus");
            }

            public static WP7Process[] AllProcesses()
            {
                string s = "";
                var t = DllImportCaller.lib.MessageBoxRunningProc(ref s);

                var processes = s.Split('\n');

                var procCount = processes.Length - 1;

                WP7Process[] _out = new WP7Process[procCount];


                for (int i = 0; i < processes.Length - 1 /*last proc add's \n for new parse line*/; i++)
                {
                    var arr = processes[i].Split('-');

                    uint dwSize = uint.Parse(arr[0]);
                    uint cntUsage = uint.Parse(arr[1]);
                    uint th32ProcessID = uint.Parse(arr[2]);
                    IntPtr th32DefaultHeapID = arr[3] == "" ? IntPtr.Zero : new IntPtr(int.Parse(arr[3]));
                    uint th32ModuleID = uint.Parse(arr[4]);
                    uint cntThreads = uint.Parse(arr[5]);
                    uint th32ParentProcessID = uint.Parse(arr[6]);
                    int pcPriClassBase = int.Parse(arr[7]);
                    uint dwFlags = uint.Parse(arr[8]);
                    string szExeFile = arr[arr.Length - 1];

                    _out[i] = new WP7Process
                    {
                        RAW = new WP7Process.PROCESSENTRY32
                        {
                            dwSize = dwSize,
                            cntUsage = cntUsage,
                            th32ProcessID = th32ProcessID,
                            th32DefaultHeapID = th32DefaultHeapID,
                            th32ModuleID = th32ModuleID,
                            cntThreads = cntThreads,
                            th32ParentProcessID = th32ParentProcessID,
                            pcPriClassBase = pcPriClassBase,
                            dwFlags = dwFlags,
                            szExeFile = szExeFile
                        }
                    };
                }

                return _out;
            }
            [Obsolete("Use Phone.WP7Process.GetCurrentProcess()", true)]
            public static WP7Process CurrentProcess()
            {
                var proc = Phone.TaskManager.AllProcesses().FirstOrDefault(a => a.RAW.szExeFile.ToLower().Contains("taskhost"));

                return proc;
            }

            public static WP7Process[] Named(string exeWithoutExtension)
            {
                if (exeWithoutExtension == null) throw new ArgumentNullException("exeWithoutExtension");

                var proc = Phone.TaskManager.AllProcesses().Where(a => a.RAW.szExeFile.ToLower().Contains(exeWithoutExtension));

                return proc.ToArray();
            }

            public static int GentlyExitCurrentProcess()
            {
                return DllImportCaller.lib.IntCall("coredll", "PostQuitMessage", 0);
            }

            public static int CloseWindow(int hwnd)
            {
                return Phone.OS.SendMessage(hwnd, WM.WM_CLOSE, 0, 0);
                //return DllImportCaller.lib.IntDualCall("coredll", "SendMessage", hwnd, 0x10 /*WM_CLOSE*/);
            }

            public static int SetForegroundWindow(int hwnd)
            {
                return DllImportCaller.lib.IntCall("coredll", "SetForegroundWindow", hwnd);
            }

            private const int SW_SHOWNORMAL = 1;
            public static int ShowWindow(int hwnd, int SW_Command)
            {
                return DllImportCaller.lib.IntDualCall("coredll", "ShowWindow", hwnd, SW_Command);
            }

            public static void TaskSwitch_SwitchTo(int hwnd)
            {
                for (int i = 0; i < 9; i++)
                {
                    var n1 = ShowWindow(hwnd, i);
                    var n2 = SetForegroundWindow(hwnd);
                    System.Threading.Thread.Sleep(50);
                }
               
            }

            /// <param name="onlyWithTitle">Prevents of finding certain windows that actualy are windows but with no title.</param>
            /// <returns></returns>
            public static WindowPointer[] GetEnumWindows(bool onlyWithTitle, bool onlyIsWindow = true)
            {
                string result = "";
                var t = DllImportCaller.lib.EnumWindows7(true, ref result);

                var split = result.Split('\n');

                var ret = new System.Collections.Generic.List<WindowPointer>();

                if (onlyWithTitle)
                {
                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        var w = new WindowPointer { HWND = int.Parse(split[i]) };

                        var title = w.GetTitle();

                        if (title.Length > 0 && title != "" && ((int)title.ToCharArray()[0]) != 6260)
                        {
                            ret.Add(w);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        var w = new WindowPointer { HWND = int.Parse(split[i]) };

                        ret.Add(w);
                    }
                }

                return ret.ToArray();
            }

            public static WindowPointer[] GetAllRunningSilverligt_XNA()
            {
                return GetEnumWindows(false, false).Where(window => window.GetTitle().Contains("app://")).ToArray();
            }

            public static void DumpEnumWindowsToConsole()
            {
                var wnds = Phone.TaskManager.GetEnumWindows(false, false).Select(c => c.GetClassName()).ToArray();
                int i = 0;
                foreach (var item in wnds)
                {
                    System.Diagnostics.Debug.WriteLine("[" + i++ + "] \"" + item + "\"");
                }
            }

            public class WindowPointer
            {
                public int HWND;

                public string GetTitle()
                {
                    string result = "";
                    var t = DllImportCaller.lib.TextForHWNDEnumWindows(HWND, ref result);
                    return result;
                }

                public uint GetWindowThreadProcessId()
                {
                    return Phone.Console7.GetUIThreadIdFor(HWND);
                }

                public string GetClassName()
                {
                    string result = "";
                    var t = DllImportCaller.lib.GetClassNameForHWNDEnumWindows(HWND, ref result);
                    return result;
                }

                public override string ToString()
                {
                    return string.Format("\"{0}\" @ {1}", GetTitle(), HWND); 
                }
            }
        }
    }
}
