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
        public static class XAPInstaller
        {
            //NativeInstaller.dll -> CleanupApplicationInstall | InstallApplication | UninstallApplication 

            [Obsolete("Just implented, dont use/call", true)]
            public static void InstallApplication(string fullXapPath)
            {
                XAPInstaller.InstallApplication("");
                throw new NotImplementedException("Dll calls with args not supported");

                return; //dont know yet
            }
            [Obsolete("Just implented, dont use/call", true)]
            public static void UninstallApplication(Guid guid)
            {
                throw new NotImplementedException("Dll calls with args not supported");
                return; //dont know yet
            }
            [Obsolete("Just implented, dont use/call", true)]
            public static void CleanupApplicationInstall(Guid guid)
            {
                throw new NotImplementedException("Dll calls with args not supported");
                return; //dont know yet

            }
            private static int z(string method)
            {
                return DllImportCaller.lib.VoidCall("NativeInstaller", method);
            }
        }
    }
}
