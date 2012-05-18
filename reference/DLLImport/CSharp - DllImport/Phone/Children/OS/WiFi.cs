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
        public static partial class Network
        {
            public static class WiFi
            {
                // strAdapter --> SWLD25SP1

                //WCHAR strDevice[MAX_PATH];
                //wsprintf(strDevice, L"{98C5250D-C29A-4985-AE5F-AFE5367E5006}\%s",
                //strAdapter);
                //SetDevicePower(strDevice, POWER_NAME, D3);

                //For power on:

                //SetDevicePower(strDevice, POWER_NAME, D0);


                //[DllImport("coredll.dll", SetLastError = true)]
                //private static extern int SetDevicePower(string pvDevice, int dwDeviceFlags, DevicePowerState DeviceState);

                //[DllImport("coredll.dll", SetLastError = true)]
                //private static extern int GetDevicePower(string pvDevice, int dwDeviceFlags, ref DevicePowerState DeviceState);

                //HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Power\State\Suspend\{98C5250D-C29A-4985-AE5F-AFE5367E5006}


                //Root-tools ONMIA 7 (IEEE standards?, another android had same, HD2): {98C5250D-C29A-4985-AE5F-AFE5367E5006}\BCMSDDHD1
                public enum ConnectionType { Unk0, Unk1, Unk2, Connected /*3, connected usb and wifi on*/, Unk4, Unk5, Unk6, Unk7, Unk8, Unk9, Unk10, Unk11 }
                public static ConnectionType GetWirlessState
                {
                    get
                    {
                        //also "ChangeWirelessState", probably 
                        return (ConnectionType)z("GetWirelessState");
                    }
                }
          

                private static int z(string method)
                {
                    return DllImportCaller.lib.VoidCall("COREDLL", method);
                }


                private static int wifiCall(string m, int _1, int _2)
                {
                    return DllImportCaller.lib.StringIntIntCall("coredll", m, "{98C5250D-C29A-4985-AE5F-AFE5367E5006}\\BCMSDDHD1", _1, _2);
                }

                public static bool IsEnabled
                {
                    get
                    {
                        switch (GetState())
                        {
                            case PowerState.D0:
                            case PowerState.D1:
                            case PowerState.D2: return true;

                            default: return false; //D3 + D4 (sleep & off)  
                        }
                    }
                    set
                    {
                        SetState(PowerState.D0);
                    }
                }

                public static void SetState(PowerState state)
                {
                    var notify = wifiCall("DevicePowerNotify", (int)state, 1);
                    var set = wifiCall("SetDevicePower", 1, (int)state);
                }

                public static PowerState GetState()
                {
                    int state;
                    var res = DllImportCaller.lib.StringIntIntOutCall("coredll", "GetDevicePower", "{98C5250D-C29A-4985-AE5F-AFE5367E5006}\\BCMSDDHD1", 1, out state);
                    return (PowerState)state;
                }

                public enum PowerState : int
                {
                    [Obsolete("msdn.microsoft.com/en-us/library/bb201981.aspx: The PwrDeviceUnspecified and PwrDeviceMaximum values are not valid device power states but are used for some Power Manager functions.", true)]
                    PwrDeviceUnspecified = -1,
                    /// <summary>
                    /// Full On: full power, full functionality
                    /// </summary>
                    D0 = 0,
                    /// <summary>
                    /// Low Power On: fully functional at low power/performance
                    /// </summary>
                    D1 = 1,
                    /// <summary>
                    /// Standby: partially powered with automatic wake
                    /// </summary>
                    D2 = 2, 
                    /// <summary>
                    /// Sleep: partially powered with device initiated wake
                    /// </summary>
                    D3 = 3,
                    /// <summary>
                    /// Off: unpowered
                    /// </summary>
                    D4 = 4,
                    [Obsolete("msdn.microsoft.com/en-us/library/bb201981.aspx: The PwrDeviceUnspecified and PwrDeviceMaximum values are not valid device power states but are used for some Power Manager functions.", true)]
                    PwrDeviceMaximum = 5
                }
            }
        }
    }
}
