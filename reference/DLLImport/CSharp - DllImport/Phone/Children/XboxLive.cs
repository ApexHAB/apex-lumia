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
        public class XboxLive
        {
            public static bool GetIsXboxLiveEnable()
            {
                return DllImportCaller.lib.VoidCall("ZMF_Client", "GF_IsXboxLiveEnable") == 1;
            }

            public static bool GetIsSignedIn()
            {
                return DllImportCaller.lib.VoidCall("ZMF_Client", "GF_IsSignedIn") == 1;
            }
        }
    }
}
