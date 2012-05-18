﻿using System;
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
        public static partial class Network
        {
            public static int GetIsNetworkAvailable()
            {
                return DllImportCaller.lib.VoidCall("agcore", "GetIsNetworkAvailable");
            }
        }
    }
}
