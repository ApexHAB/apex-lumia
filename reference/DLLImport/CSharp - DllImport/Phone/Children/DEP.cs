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
        /// <summary>
        /// DeviceExtendedProperties wrapper
        /// </summary>
        public static class DEP
        {
            static DEP()
            {
                try
                {
                    m_DeviceUniqueId = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                    CanAccessDeviceUniqueId = m_DeviceUniqueId != null;
                }
                catch (Exception)
                {
                    m_DeviceUniqueId = null;
                    CanAccessDeviceUniqueId = false;
                }
            }
            public static readonly string DeviceManufacturer = (string)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceManufacturer");
            public static readonly string DeviceName = (string)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceName");

            private static byte[] m_DeviceUniqueId;
            /// <summary>
            /// A byte array. 20 bytes in length. 
            /// http://msdn.microsoft.com/en-us/library/ff941122(v=VS.92).aspx
            /// </summary>
            public static byte[] DeviceUniqueId
            {
                /*
                    var test = BitConverter.ToString(DEP.DeviceUniqueId); 
                    60-EE-9D-08-22-46-56-23-4A-42-1F-A7-A9-94-1E-84-C6-12-E2-3E
                    60EE9D08224656234A421FA7A9941E84C612E23E -> str len(40), byte len(20)
                */
                get
                {
                    return IsEmulator ?
                        new byte[] { 0x60, 0xee, 0x9d, 0x08, 0x22, 0x46, 0x56, 0x23, 0x4a, 0x42, 0x1f, 0xa7, 0xa9, 0x94, 0x1e, 0x84, 0xc6, 0x12, 0xe2, 0x3e } :
                        m_DeviceUniqueId;
                }
                private set
                {
                    m_DeviceUniqueId = value;
                }
            }
            /// <summary>
            /// A string len(40) containing packed hex, null if DeviceUniqueId is null
            /// </summary>
            public static string DeviceUniqueId_Str
            {
                get
                {
                    if (DeviceUniqueId == null)
                        return null;

                    var bits = BitConverter.ToString(DeviceUniqueId);
                    return bits.Replace("-", string.Empty);
                }
            }

            private static bool m_CanAccessDeviceUniqueId;
            public static bool CanAccessDeviceUniqueId
            {
                get
                {
                    return IsEmulator ?
                        true :
                        m_CanAccessDeviceUniqueId;
                }
                private set
                {
                    m_CanAccessDeviceUniqueId = value;
                }
            }
            public static readonly string DeviceFirmwareVersion = (string)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceFirmwareVersion");
            public static readonly string DeviceHardwareVersion = (string)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceHardwareVersion");
            public static readonly long DeviceTotalMemory = (long)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceTotalMemory");
            public static long ApplicationCurrentMemoryUsage
            {
                get
                {
                    return (long)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
                }
            }
            public static long ApplicationPeakMemoryUsage
            {
                get
                {
                    return (long)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage");
                }
            }

            new public static string ToString()
            {
                return
                    "DeviceManufacturer: " + DeviceManufacturer + "\n" +
                    "DeviceName: " + DeviceName + "\n" +
                    "DeviceUniqueId: " + DeviceUniqueId + "\n" +
                    "DeviceFirmwareVersion: " + DeviceFirmwareVersion + "\n" +
                    "DeviceHardwareVersion: " + DeviceHardwareVersion + "\n" +
                    "DeviceTotalMemory: " + DeviceTotalMemory + "\n" +
                    "ApplicationCurrentMemoryUsage: " + ApplicationCurrentMemoryUsage + "\n" +
                    "ApplicationPeakMemoryUsage: " + ApplicationPeakMemoryUsage;
            }
            public static readonly bool IsEmulator = DeviceName.Contains("Emulator");

            public static readonly string WindowsLiveAnonymousID = GetWindowsLiveAnonymousID();

            private static readonly int ANIDLength = 32;
            private static readonly int ANIDOffset = 2;

            private static string GetWindowsLiveAnonymousID()
            {
                string result = string.Empty;
                object anid;
                if (AnonymousID(out anid))
                {
                    if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))
                    {
                        result = anid.ToString().Substring(ANIDOffset, ANIDLength);
                    }
                }

                return result;
            }
            public static bool AnonymousID(out object anid)
            {
                object _anid;
                var val = Microsoft.Phone.Info.UserExtendedProperties.TryGetValue("ANID", out _anid);
                anid = _anid;

                return val;
            }
        }
    }
}
