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
        public static class Shell
        {
            public static bool IsShellReady
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("shcore", "IsShellReady") == 1;
                }
            }
        }
    }
}
