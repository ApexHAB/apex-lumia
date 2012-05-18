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
            public static partial class User
            {
                public static bool GetPasswordActive()
                {
                    return DllImportCaller.lib.VoidCall("coredll", "GetPasswordActive") == 1;
                }
                public static bool CheckPassword(string password)
                {
                    if (password == null) throw new ArgumentNullException("password");

                    return DllImportCaller.lib.StringCall("coredll", "CheckPassword", password) == 1;
                }
                /// <summary>
                /// This function returns the amount of time, in milliseconds, that the system has been idle.
                /// </summary>
                /// <returns>milliseconds idle</returns>
                public static int GetIdleTime()
                {
                    return DllImportCaller.lib.VoidCall("coredll", "GetIdleTime");
                }
            }
        }
    }
}
