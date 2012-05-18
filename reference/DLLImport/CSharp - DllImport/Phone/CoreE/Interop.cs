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
using System.Runtime.InteropServices;

namespace CSharp___DllImport
{
    //http://msdn.microsoft.com/en-us/library/system.runtime.interopservices.marshal.getdelegateforfunctionpointer.aspx
    //http://stackoverflow.com/questions/3242331/difference-between-dllimport-and-getprocaddress
    public static class Registrer
    {
        private static uint last = 0;

        public static uint GetLastRegisterCode()
        {
            return last;
        }
        public static uint Register(string assemblyDLL, string guid)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.Load("Microsoft.Phone.InteropServices, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e");


            Type comBridgeType = null;

            comBridgeType = asm.GetType("Microsoft.Phone.InteropServices.ComBridge");
            var dynMethod = comBridgeType.GetMethod("RegisterComDll", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            return last = (uint)dynMethod.Invoke(null, new object[] { assemblyDLL, new Guid(guid) });
        }
    }

    public static class Interop
    {
        //public const string BasePath = @"\Applications\Install\8DC5214E-88FA-4C2D-A379-2CD74FE24B72\Install\";

        private static bool initialized;
        public static void Init()
        {
            if (!initialized)
            {
                var id = ManifestAppInfo.Info.ProductId.Replace("{", "").Replace("}", "");
                string BasePath = string.Format(@"\Applications\Install\{0}\Install\", id);
                Registrer.Register(BasePath +
#if RUNNS_UNDER_MANGO
 "DllImportMango.dll", "434B816A-3ADA-4386-8421-33B0E669F3F1"
#else
"FileSystem.dll", "F0D5AFD8-DA24-4e85-9335-BEBCADE5B92A"
#endif
);
                //Registrer.Register("DllImportMango.dll", "434B816A-3ADA-4386-8421-33B0E669F3F1");
                initialized = true;
            }
        }
        public static void NopRndCall()
        {
            int a = 10;
            return;
        }
    }
    public sealed class S
    {
        public const int S_OK = 0;
        public const int S_FALSE = 1;
    }
    public sealed class MB
    {
        public const int MB_OK = 0;
        public const int MB_OKCANCEL = 1;
        public const int MB_ABORTRETRYIGNORE = 2;
        public const int MB_YESNOCANCEL = 3;
        public const int MB_YESNO = 4;
        public const int MB_RETRYCANCEL = 5;
    }

    public enum EWX : uint
    {
        /// <summary>
        /// Same as EWX_POWEROFF
        /// </summary>
        EWX_LOGOFF = 0,
        /// <summary>
        /// Same as EWX_POWEROFF
        /// </summary>
        EWX_SHUTDOWN = 1,
        /// <summary>
        /// Reboots the device.
        /// </summary>
        EWX_REBOOT = 2,
        /// <summary>
        /// Same as EWX_POWEROFF
        /// </summary>
        EWX_FORCE = 4,
        /// <summary>
        /// Shuts down the device completely.
        /// </summary>
        EWX_POWEROFF = 8
    }

    public static class DllImportCaller
    {
        private static System.Collections.Generic.Dictionary<string, IntPtr> dllPtrs;
        private static System.Collections.Generic.Dictionary<string, IntPtr> methodPtrs;

        public static IFileSystemIO lib;
        /// <summary>
        /// Filled on FreeLibrarys
        /// </summary>
        public static int[] DestructorInts;

        static DllImportCaller()
        {
            dllPtrs = new System.Collections.Generic.Dictionary<string, IntPtr>();
            methodPtrs = new System.Collections.Generic.Dictionary<string, IntPtr>();

            Interop.Init();

            var c = new FileSystemClass();
            lib = c as IFileSystemIO;
        }

         
        //public static Delegate ptrToDelegate(IntPtr ptr)
        //{
          
        //    //constA.Invoke(

        //    return null;
        //}

        public static IntPtr lookupDll(string dll)
        {
            IntPtr ptr;
            if (dllPtrs.TryGetValue(dll, out ptr)) //call
            {
                return ptr;
            }
            else
            {
                int call = lib.LoadLibrary7(dll, out ptr);
                dllPtrs.Add(dll, ptr);

                return ptr;
            }
        }

        public static IntPtr lookupMethod(IntPtr dll, string method)
        {
            string key = dll.ToString() + "__" + method;
            IntPtr ptr;
            if (methodPtrs.TryGetValue(key, out ptr))
            {
                return ptr;
            }
            else
            {
                int call = lib.GetProcAddress7(dll, method, out ptr);
                methodPtrs.Add(key, ptr);

                return ptr;
            }
        }


        //public static T Call<T>(string dll, string method, ref string arg)
        //{
        //    IntPtr dllLook;
        //    if ((dllLook = lookupDll(dll)) != IntPtr.Zero)
        //    {
        //        IntPtr methodLookup;
        //        if ((methodLookup = lookupMethod(dllLook, method)) != IntPtr.Zero)
        //        {
        //            //var t = lib.NativeCallExecArgT_1(dllLook, methodLookup, arg);
        //            //unsafe
        //            //{

        //            //var d = new BOX(;
        //            //}
        //            //http://stackoverflow.com/questions/3242331/difference-between-dllimport-and-getprocaddress native ptr to managed method
        //            //var str  = Marshal.PtrToStringUni(methodLookup);//.GetDelegateForFunctionPointer(pFunc, typeof(BarType));
        //            return default(T);
        //        }
        //        else
        //        {
        //            throw new MissingMethodException("Dll: \"" + dllLook.ToString() + "\" Method: \"" + method + "\"");
        //        }
        //    }
        //    else
        //    {
        //        throw new DllNotFoundException("Dll: \"" + dll + "\"");
        //    }
        //}
        public enum NMethodE { FoundMethod, MissingDLL, MissingMethod };
        public static NMethodE NativeMethodExists(string dll, string method)
        {
            IntPtr dllLook;
            if ((dllLook = lookupDll(dll)) != IntPtr.Zero)
            {
                IntPtr methodLookup;
                if ((methodLookup = lookupMethod(dllLook, method)) != IntPtr.Zero)
                {
                    return NMethodE.FoundMethod;
                }
                else
                {
                    DllImportCaller.lib.FreeLibrary7(dllLook);
                    return NMethodE.MissingMethod;
                }
            }
            else
            {
                return NMethodE.MissingDLL;
            }
        }

        public static void FreeLibrarys()
        {
            int[] _destr = new int[dllPtrs.Count];
            for (int i = 0; i < dllPtrs.Count; i++)
            {
                _destr[i] = lib.FreeLibrary7(dllPtrs.ElementAt(i).Value); // 1 = SUCCESS, 0 = FAIL
            }
            dllPtrs.Clear();

            DllImportCaller.DestructorInts = _destr;
        }

        public static Win32ErrorCode LastError()
        {
            return (Win32ErrorCode)DllImportCaller.lib.GetLastError7();
        }
    }

    [ComImport, Guid(
#if RUNNS_UNDER_MANGO
        "434B816A-3ADA-4386-8421-33B0E669F3F1"
#else
        "F0D5AFD8-DA24-4e85-9335-BEBCADE5B92A"
#endif
), ClassInterface(ClassInterfaceType.None)]
    public class FileSystemClass { }

    [ComImport, Guid(
#if RUNNS_UNDER_MANGO
        "A980817D-1DFC-4307-A069-A725E544F79C"
#else //NODO
        "2C49FA3D-C6B7-4168-BE80-D044A9C0D9DF"
#endif
        
        ), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public partial interface IFileSystemIO
    {
        [PreserveSig]
        int Zune_VoidCall(string method);

        [PreserveSig]
        int Radio_Toggle(uint powerMode);

        [PreserveSig]
        int LoadLibrary7(string lpFileName, out IntPtr hFind);

        [PreserveSig]
        int GetProcAddress7(IntPtr hFind, string lpProcName, out IntPtr procAddr);

        [PreserveSig]
        int FreeLibrary7(IntPtr hFind);

        [PreserveSig]
        int Clipboard_SET([MarshalAs(UnmanagedType.BStr)] string text);

        [PreserveSig]
        int Clipboard_GET([Out,In, MarshalAs(UnmanagedType.BStr)] ref string text);

        [PreserveSig]   
        int VoidCall(string dll, string method);

        [PreserveSig]
        int MessageBoxRunningProc([Out, In, MarshalAs(UnmanagedType.BStr)] ref string result);

        [PreserveSig]
        int TerminateProcess7(uint pid, uint exitCode);

        [PreserveSig]
        int GetAsyncKeyState7(int key);

        [PreserveSig]
        int Crnch(ref string result);

        [PreserveSig]
        int QueryPerformanceCounter7(out Int64 result);

        //[PreserveSig]
        //int CrnchV2(ref string result);

        [PreserveSig, Obsolete("Not implented", true)]
        int SetWindowsHookExW(int key, int pre);

        [PreserveSig]
        int ShellExecuteEx7(string file, string args = "");

        [PreserveSig]
        int CreateProcess7(string file, string args = "");

        [PreserveSig]
        int GetProcessTimes7(IntPtr hProcess,
                                             out int ctime1, out int ctime2,
                                             out int etime1, out int etime2,
                                             out int ktime1, out int ktime2,
                                             out int utime1, out int utime2);

        [PreserveSig, Obsolete("Trying to access exe runtime. Corrupt, dont use.", true)]
        int NavigateToExternalPage7(IntPtr safeHandle, string pageUri, bool fIsInvocation, int cbPageArgs);

        /// <summary>
        /// Need to run on "PhoneApplicationPage.Dispatcher.BeginInvoke" (syncron) or "System.Threading.Thread" (different thread) (asyncron) 
        /// The Native msg box does not vibrate.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        int MessageBox7(string lpText, string lpCaption, uint uType, out int result);

        [PreserveSig]
        int ShutdownOS(uint ewxCode);

        [PreserveSig, Obsolete("Fails, same error as on ::CreateProcess: INVALID_EXE_SIGNATURE", true)]
        int CeCreateProcessEx();


        [PreserveSig]
        int GetLastError7();

        [PreserveSig]
        int StringCall(string dll, string method, string value);

        int Tests();

        [PreserveSig]
        int CreateDirectoryPath7(string fullPath);

        [PreserveSig]
        int NdisGetSystemUpTimeEx7(int part);

        [PreserveSig]
        int fopen7(string filename, string mode);

        [PreserveSig]
        int fclose7(int handle);

        [PreserveSig]
        int fseek7(int handle, long offset, int origin);

        [PreserveSig]
        int fgetc7(int handle);

        [PreserveSig]
        int fsize7(int handle);

        [PreserveSig]
        int fputc7(int handle, char value);

        [PreserveSig]
        int ddirFiles7(string fullFolderName, ref string result);

        [PreserveSig]
        int dirFiles7(string fullFolderName, ref string result);

        [PreserveSig]
        int FindFirstFile7(string dir, out WIN32_FIND_DATA data);

        [PreserveSig]
        int FindNextFile7(int handle, out WIN32_FIND_DATA data);

        [PreserveSig]
        int FindClose7(int handle);

        [PreserveSig]
        int UintCall(string dll, string method, uint value);

        [PreserveSig]
        int WindowTextOfHWND(int hwnd, ref string result, bool setOrGet);

        [PreserveSig]
        int BoolCall(string dll, string method, bool value);

        [PreserveSig]
        int EnumWindows7(bool onlyIsWindow, ref string result);

        [PreserveSig]
        int TextForHWNDEnumWindows(int hwnd, ref string result);

        [PreserveSig]
        int GetClassNameForHWNDEnumWindows(int hwnd, ref string result);

        [PreserveSig]
        int IntCall(string dll, string method, int value);

        [PreserveSig]
        int IntDualCall(string dll, string method, int value, int value2);

        [PreserveSig]
        int EnumDisplayDevices7(uint id, ref Phone.Display.DISPLAY_DEVICE display, uint flags);

        [PreserveSig]
        int waveOutGetVolume7(out ulong volume);

        [PreserveSig]
        int waveOutSetVolume7(ulong volume);

        [PreserveSig]
        int waveOutSetPitch7(ulong value);

        [PreserveSig]
        int waveGetHWAVEOUT();

        /// <summary>
        /// Kernel control, hard reset possible from here. DONT! ?
        /// 
        /// This function provides the kernel with a generic I/O control for
        /// carrying out I/O operations.
        /// </summary>
        /// <param name="dwIoControlCode">I/O control code, which should support the
        /// OAL I/O controls. For a list of these I/O controls, see Supported
        /// OAL APIs.</param>
        /// <param name="lpInBuf">Pointer to the input buffer.</param>
        /// <param name="nInBufSize">Size, in bytes, of lpInBuf.</param>
        /// <param name="lpOutBuf">Pointer to the output buffer.</param>
        /// <param name="nOutBufSize">Maximum number of bytes that can be returned in
        /// lpOutBuf.</param>
        /// <param name="lpBytesReturned">Address of a DWORD that receives the size,
        /// in bytes, of the data returned.</param>
        /// <returns>TRUE indicates success; FALSE indicates failure.</returns>
        [PreserveSig]
        int KernelIoControl
        (
            uint dwIoControlCode,
            IntPtr lpInBuf,
            uint nInBufSize,
            IntPtr lpOutBuf,
            uint nOutBufSize,
            ref uint lpBytesReturned
        );

        [PreserveSig]
        int GetSystemPowerStatusEx7(ref string battery, bool fUpdate);

        [PreserveSig]
        int StringIntIntCall(string dll, string method, string _string, int int1, int int2);

        [PreserveSig]
        int StringIntIntOutCall(string dll, string method, string _string, int int1, out int int2);

        /// <summary>
        /// Set screen contrast
        /// </summary>
        /// <param name="dwContrast">A value [0 - 255]</param>
        /// <returns></returns>
        [PreserveSig]
        int ScreenSetContrast(int dwContrast);

        /// <returns>Current contrast</returns>
        [PreserveSig]
        int ScreenGetContrast();

        [PreserveSig]
        int IntQuadCall(string dll, string method, int int1, int int2, int int3, int int4);

        [PreserveSig]
        int PhoneMakeCall7(string number);

        [PreserveSig, Obsolete("Blocked by security policy, or? Returns 0 (error, time not chanhed) (SE_SYSTEMTIME_NAME privilege needed)", true)]
        int SetSystemTime7(short wYear, short wMonth, short wDayOfWeek, short wDay, short wHour, short wMinute, short wSecond, short wMilliseconds);

        [PreserveSig]
        int StringStringCall(string dll, string method, string value, string value2);

        [PreserveSig]
        int GetSystemPowerStatusExAdv7(ref string battery, bool fUpdate);

        [PreserveSig]
        int GlobalMemoryStatus7(ref string status);

        [PreserveSig]
        int CeRunAppAtTime7(string result, SYSTEMTIME time);

        [PreserveSig]
        int EnumDisplaySettings7(string lpszDeviceName, int iModeNum, ref string devMode);

        [PreserveSig, Obsolete("Dont call, will return an empty string anyways", true)]
        int GetCommandLine7(ref string result);

        [PreserveSig]
        int PostMessage7(int hwnd, uint msg, uint wParam, uint lParam);

        [PreserveSig]
        int SendMessage7(int hwnd, uint msg, uint wParam, uint lParam);

        [PreserveSig]
        int ThisTaskHostInformation(string dll, string method, out Phone.TaskHost.HostInformation info);

        [PreserveSig]
        int CaptureScreen(out IntPtr pHandle, out int pSize, out IntPtr ppvBits);
        [PreserveSig]
        int DeleteObject(IntPtr hObject);

        [PreserveSig]
        int ASMExecute(ref byte functionBytesAddr);

        [PreserveSig]
        int GetHelloWorldMSGBPtr();

        [PreserveSig]
        int ValueAtAddres(int addr);

        [PreserveSig]
        int AddressOfObject(ref object _object);

        [PreserveSig]
        int TestFunc1();

        [PreserveSig]
        int TestFunc2();

        [return: MarshalAs(UnmanagedType.Bool)]
        [PreserveSig]
        bool DeviceIoControl7(uint dDevice, uint dwIoControlCode, [In] byte[] lpInBuf, int nInBufferSize, [Out] byte[] lpOutBuf, int nOutBufferSize, ref int pBytesReturned, IntPtr lpOverlapped);

        [PreserveSig]
        int WriteByte(int address, byte value);

        [PreserveSig]
        int EDB_Mount([MarshalAs(UnmanagedType.LPWStr)] string databaseFile, out bool sucCode);

        [PreserveSig]
        /*guidAddr*/ int EDB_OpenMounted([MarshalAs(UnmanagedType.LPWStr)] string databaseFile, int guidAddr);

        [PreserveSig]
        int EDB_FindFirstDB(int guidAddr);

        [PreserveSig]
        int EDB_FindNextDB(int findFirstHandle, int guidAddr);

        [PreserveSig]
        int EDB_OpenDBSession(int guidAddr, int findHandle);

        [PreserveSig]
        int EDB_CloseHandle(int findFirstHandle);

        [PreserveSig]
        int EDB_CeOidGetInfoEx(int guidAddr, int findHandle);

        [PreserveSig]
        int EDB_OpenDBFind(int guidAddr, int findHandle);

        [PreserveSig]
        int EDB_CeOpenDatabaseEx(int guidAddr, int findHandle);
        //[PreserveSig]
        //int GetNativeDelegatePtr(/*[MarshalAs( UnmanagedType.FunctionPtr)]*/ Delegate d);

        //[PreserveSig]
        //int Laksasd(ref string res);
       


        ////[PreserveSig]
        ////int CaptureScreenBytes7([Out, In, MarshalAs(UnmanagedType.BStr)] ref string result);

        ////[PreserveSig]
        ////int PrntScr([Out, In, MarshalAs(UnmanagedType.BStr)] ref string result);

        ////[PreserveSig]
        ////unsafe int SayHi();

        ////[PreserveSig]
        ////unsafe int BeginEv();






        


        ///// <summary>
        ///// Use DeleteCapturedScreen after "Image.FromHbitmap"
        ///// </summary>
        ///// <param name="hBitmap"></param>
        ///// <returns></returns>
        ////[PreserveSig]
        ////unsafe int CaptureScreen(out IntPtr hBitmap);

        ////[PreserveSig]
        ////unsafe int DeleteCapturedScreen(IntPtr hBitmap);

        ////[PreserveSig]
        ////unsafe int SetThreadPriority(ulong threadIdNative, int newPriority);
        
   

       

        ////[PreserveSig]
        ////unsafe int CreateProcess7(out int result);

        ////[PreserveSig]
        ////unsafe int Shell7();

        ////[PreserveSig]
        ////unsafe int GetLastError7(out uint /*DWORD*/ err);

        ////[PreserveSig]
        ////unsafe int ExitProccess();

        ////[PreserveSig]
        ////unsafe int Lold();
        ////[PreserveSig]
        ////unsafe int HookToSearchButton();

        ////[PreserveSig]
        ////unsafe int UnHookToSearchButton();

        ////[PreserveSig]
        ////unsafe int GetCommandLine7(ref string commandLineOut);

        ////[PreserveSig]
        ////unsafe int GetSystemInfo7(out SYSTEM_INFO info);

        ////[PreserveSig]
        ////unsafe int SendSMS7(string pszNumber, string pszMessage);

        ////[PreserveSig]
        ////unsafe int FileDialog();

        //[PreserveSig]
        //int OpenFile7(string lpFilename, long dwDesiredAccess, int dwShareMode, int dwCreationDisposition, int dwFlagsAndAttributes, out IntPtr hFile);

        //[PreserveSig]
        //int ReadFile7(IntPtr hfile, IntPtr lpBuffer, int nNumberOfBytesToRead, out int lpNumberOfBytesRead);

        //[PreserveSig]
        //int CloseFile7(IntPtr hFile);

        //[PreserveSig]
        //int SeekFile7(IntPtr hFile, int lDistanceToMove, ref int lpDistanceToMoveHigh, int dwMoveMethod);

        //[PreserveSig]
        //int GetFileSize7(IntPtr hFile, out int lpFileSizeHigh);

        //[PreserveSig]
        //int CopyFile7(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);

        //[PreserveSig]
        //int FindFirstFile7(string lpFileName, out WIN32_FIND_DATA lpFindFileData, out IntPtr hFind);

        //[PreserveSig]
        //int FindNextFile7(IntPtr hFind, out WIN32_FIND_DATA lpFindFileData);

        //[PreserveSig]
        //int FindClose7(IntPtr hFind);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct pinvoke_call
    {
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 384000)]//,SizeParamIndex =0,SafeArraySubType = VarEnum.VT_I4)]
        //public int[] mynum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 192000)]
        public int[] a;
    }

}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct FILETIME
{
    public uint dwLowDateTime;
    public uint dwHighDateTime;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct SYSTEMTIME
{
    public ushort wYear;
    public ushort wMonth;
    public ushort wDayOfWeek;
    public ushort wDay;
    public ushort wHour;
    public ushort wMinute;
    public ushort wSecond;
    public ushort wMilliseconds;

    public static SYSTEMTIME FromDateTime(DateTime time)
    {
        SYSTEMTIME Stime = new SYSTEMTIME();

        Stime.wYear = (ushort)time.Year;
        Stime.wMonth = (ushort)time.Month;
        Stime.wDayOfWeek = (ushort)time.DayOfWeek;
        Stime.wDay = (ushort)time.Day;
        Stime.wHour = (ushort)time.Hour;
        Stime.wMinute = (ushort)time.Minute;
        Stime.wSecond = (ushort)time.Second;
        Stime.wMilliseconds = (ushort)time.Millisecond;

        return Stime;
    }

    public SYSTEMTIME(DateTime dt)
    {
        this.wYear = (dt.Year == 1) ? ((ushort)0) : ((ushort)dt.Year);
        this.wMonth = (ushort)dt.Month;
        this.wDayOfWeek = (ushort)dt.DayOfWeek;
        this.wDay = (ushort)dt.Day;
        this.wHour = (ushort)dt.Hour;
        this.wMinute = (ushort)dt.Minute;
        this.wSecond = (ushort)dt.Second;
        this.wMilliseconds = (ushort)dt.Millisecond;
    }

    internal static SYSTEMTIME Empty
    {
        get
        {
            SYSTEMTIME systemtime;
            systemtime.wYear = 0;
            systemtime.wMonth = 0;
            systemtime.wDayOfWeek = 0;
            systemtime.wDay = 0;
            systemtime.wHour = 0;
            systemtime.wMinute = 0;
            systemtime.wSecond = 0;
            systemtime.wMilliseconds = 0;
            return systemtime;
        }
    }
}

//[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
//public struct WIN32_FIND_DATA
//{
//    public uint dwFileAttributes;
//    public FILETIME ftCreationTime;
//    public FILETIME ftLastAccessTime;
//    public FILETIME ftLastWriteTime;
//    public uint nFileSizeHigh;
//    public uint nFileSizeLow;
//    public uint dwReserved0;
//    public uint dwReserved1;
//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
//    public string cFileName;
//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
//    public string cAlternateFileName;
//}

[StructLayout(LayoutKind.Sequential)]
public struct SYSTEM_INFO
{
    internal _PROCESSOR_INFO_UNION uProcessorInfo;
    public uint dwPageSize;
    public IntPtr lpMinimumApplicationAddress;
    public IntPtr lpMaximumApplicationAddress;
    public IntPtr dwActiveProcessorMask;
    public uint dwNumberOfProcessors;
    public uint dwProcessorType;
    public uint dwAllocationGranularity;
    public ushort dwProcessorLevel;
    public ushort dwProcessorRevision;
}

[StructLayout(LayoutKind.Explicit)]
public struct _PROCESSOR_INFO_UNION
{
    [FieldOffset(0)]
    internal uint dwOemId;
    [FieldOffset(0)]
    internal ushort wProcessorArchitecture;
    [FieldOffset(2)]
    internal ushort wReserved;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct WIN32_FIND_DATA
{
    public System.IO.FileAttributes dwFileAttributes;
    public FILETIME ftCreationTime;
    public FILETIME ftLastAccessTime;
    public FILETIME ftLastWriteTime;
    public int nFileSizeHigh;
    public int nFileSizeLow;
    public int dwOID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string cFileName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    public string cAlternateFileName;
}

//[StructLayout(LayoutKind.Explicit, Size=204)]
//unsafe public struct NCALL
//{
//    //[MarshalAs(UnmanagedType.I4)]
//    //public int argCount;

//    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
//    //public string firstname;
//    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
//    //public string lastname;
//} ;