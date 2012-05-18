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
            public static class Bluetooth
            {
                public enum BTH_RADIO_MODE : long //(DWORD) //http://msdn.microsoft.com/en-us/library/ms837416.aspx
                {
                    /// <summary>
                    /// Turn off the Bluetooth radio.
                    /// </summary>
                    BTH_POWER_OFF,
                    /// <summary>
                    /// Turn on the Bluetooth radio, and make it connectable.
                    /// </summary>
                    BTH_CONNECTABLE,
                    /// <summary>
                    /// Turn on the Bluetooth radio, and make it both connectable and discoverable.
                    /// </summary>
                    BTH_DISCOVERABLE
                };
                /*
        
                int BthSetMode(
                  DWORD dwMode 
                );
        
                int BthGetMode( // needs a new c++ out call "IntOutCall"
                  DWORD* pdwMode
                );
                
                */
                public static void TurnOff()
                {
                    Turn(BTH_RADIO_MODE.BTH_POWER_OFF);
                }

                public static void TurnOn()
                {
                    Turn(BTH_RADIO_MODE.BTH_DISCOVERABLE);
                }
                public static void Turn(BTH_RADIO_MODE mode)
                {
                    DllImportCaller.lib.IntCall("coredll", "BthSetMode", (int)mode);
                }
            }
        }
    }
}
