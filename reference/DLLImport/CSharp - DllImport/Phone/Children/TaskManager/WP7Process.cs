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
        public class WP7Process
        {
            public static int CurrentProcessID
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("coredll", "GetDirectCallerProcessId");
                }
            }

            public static WP7Process GetCurrentProcess()
            {
                var id = CurrentProcessID; // cache'd get;

                return Phone.TaskManager
                    .AllProcesses()
                    .FirstOrDefault(a => a.RAW.th32ProcessID == id);
            }
          

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct ProcMemCounter
            {
                public uint size;
                public uint pageFaultCount;
                public UIntPtr peakPrivateUsage;
                public UIntPtr privateUsage;
                public UIntPtr peakSharedDataPaged;
                public UIntPtr sharedDataPaged;
                public UIntPtr peakSharedDataNonPaged;
                public UIntPtr sharedDataNonPaged;
                public UIntPtr peakCodePaged;
                public UIntPtr codePaged;
                public UIntPtr peakCodeNonPaged;
                public UIntPtr codeNonPaged;
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct MEMORYSTATUS
            {
                public uint dwLength;
                public uint dwMemoryLoad;
                public uint dwTotalPhys;
                public uint dwAvailPhys;
                public uint dwTotalPageFile;
                public uint dwAvailPageFile;
                public uint dwTotalVirtual;
                public uint dwAvailVirtual;
            }






            //[DllImport("coredll.dll", SetLastError = true)]
            //public static extern bool CeGetProcessMemoryInfo(IntPtr hProcess, ref ProcMemCounter processMemoryCounters);

            public object MemoryInfo()
            {
                //lpBuffer.dwTotalPhys; //Default API (only total);
                //AnidProperty <- slutade i net reflector
                return false;
            }

            static DateTime _1601 = new DateTime(1601, 1, 1, 1, 0, 0);
            public int Kill(uint exitCode)
            {
                return DllImportCaller.lib.TerminateProcess7(RAW.th32ProcessID, exitCode);
            }

            private DateTime lowHighTimeToDateTime(int low, int high)
            {
                long hFT2 = (((long)high) << 32) + low;

                return DateTime.FromFileTime(hFT2);
            }
            public void UpdateTimes()
            {
                int ctime1 /*low*/, ctime2 /*high*/,
                   etime1, etime2,
                   ktime1, ktime2,
                   utime1, utime2;

                var res = DllImportCaller.lib.GetProcessTimes7(
                    (IntPtr)Phone.WP7Process.GetCurrentProcess().RAW.th32ProcessID,
                    out ctime1, out ctime2,
                    out etime1, out etime2,
                    out ktime1, out ktime2,
                    out utime1, out utime2);

                m_CreationTime = lowHighTimeToDateTime(ctime1, ctime2);
                m_ExitTime = lowHighTimeToDateTime(etime1, etime2);
                m_KernelTime = lowHighTimeToDateTime(ktime1, ktime2) - _1601;
                m_UserTime = lowHighTimeToDateTime(utime1, utime2) - _1601;

                filledOnce = true;
            }
            bool filledOnce = false;

            DateTime m_CreationTime;
            public DateTime CreationTime
            {
                get
                {
                    if (!filledOnce) UpdateTimes();

                    return m_CreationTime;
                }
            }

            DateTime m_ExitTime;
            public DateTime ExitTime
            {
                get
                {
                    if (!filledOnce) UpdateTimes();

                    return m_ExitTime;
                }
            }

            TimeSpan m_KernelTime;
            public TimeSpan KernelTime
            {
                get
                {
                    if (!filledOnce) UpdateTimes();

                    return m_KernelTime;
                }
            }

            TimeSpan m_UserTime;
            public TimeSpan UserTime
            {
                get
                {
                    if (!filledOnce) UpdateTimes();

                    return m_UserTime;
                }
            }

            //public string GetStartArguments()
            //{
            //    return 
            //    //DllImportCaller.lib.GetStartArguments(RAW.th32ProcessID);
            //}

            public PROCESSENTRY32 RAW;

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct PROCESSENTRY32
            {
                public uint dwSize;
                public uint cntUsage;
                public uint th32ProcessID;
                public IntPtr th32DefaultHeapID;
                public uint th32ModuleID;
                public uint cntThreads;
                public uint th32ParentProcessID;
                public int pcPriClassBase;
                public uint dwFlags;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szExeFile;
            };
        }
    }
}
