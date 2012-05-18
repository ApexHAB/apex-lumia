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
using System.Runtime.InteropServices;
using System.Reflection;

namespace CSharp___DllImport
{
    public class StandardRightsTest
    {
        public StandardRightsTest()
        {
            var a = Assembly.Load("Microsoft.Phone.InteropServices, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e");
            Type comBridgeType = a.GetType("Microsoft.Phone.InteropServices.ComBridge");
            MethodInfo dynMethod = comBridgeType.GetMethod("RegisterComDll", BindingFlags.Public | BindingFlags.Static);
            dynMethod.Invoke(null, new object[] { "COMRilClient.dll", new Guid("A18F6B1A-924E-4787-AA82-19F98B49CF5D") });
          
            try
            {
                this.SecRILControlInterface = (ISecRilControl)new COSecRilControl();
            }
            catch { }
        }

        public ISecRilControl SecRILControlInterface;

        [ComImport, ClassInterface(ClassInterfaceType.None), Guid("A18F6B1A-924E-4787-AA82-19F98B49CF5D")]
        public class COSecRilControl
        {
        }

        [ComImport, Guid("A5857C17-04C2-49c5-A460-05A21660588F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ISecRilControl
        {
            void Init();
            void Deinit();
            void Run(int mode);
            void End();
            void SetInput(int type);
            void Back();
            void GetDispInfo(out uint svcMode, [MarshalAs(UnmanagedType.SafeArray)] out byte[] info);
            void GetEvent(int type, out int pEvent);
            void SetEventCOM(string name);
            void LaunchExe(string exe, string arg);
            void DoHiddenKey(int hashcode);
            void GetLockingStatus(out uint m_dwLockFacility, [MarshalAs(UnmanagedType.SafeArray)] out byte[] pPasswd);
            void SetLockingStatus(out uint m_dwLockFacility, string data, out uint m_dwStatus, [MarshalAs(UnmanagedType.SafeArray)] out byte[] result);
            void GetIMSI(out string IMSI);
            void GetIMEI(out string IMEI);
            void DoHiddenKeyWithResult(int hashcode, out string jobName);
            void WaitNamedEvent(int timeout, string name);
            void RegSetDWORD(uint HKEY, string pwszPath, string valueName, uint value);
            void RegGetDWORD(uint HKEY, string pwszPath, string valueName, out uint value);
            void RegSetString(uint HKEY, string pwszPath, string valueName, string value);
            void RegGetString(uint HKEY, string pwszPath, string valueName, out string value);
            void ReadTextFile(string path, out string result);
        }
    }
}
