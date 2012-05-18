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
        public static partial class Display
        {
            public static bool GetDisplay(uint id, uint flags, ref DISPLAY_DEVICE device)
            {
                var val = DllImportCaller.lib.EnumDisplayDevices7(id, ref device, flags);
                return val != 0;
            }
            public static DISPLAY_DEVICE GetDisplay(uint id, uint flags)
            {
                DISPLAY_DEVICE dev = new DISPLAY_DEVICE();
                dev.cb = 840;
                var display = GetDisplay(id, flags, ref dev);

                return dev;
            }

            public static DISPLAY_DEVICE[] GetDisplays()
            {
                var displays = new System.Collections.Generic.List<DISPLAY_DEVICE>();

                DISPLAY_DEVICE d = new DISPLAY_DEVICE();
                d.cb = 840;

                for (uint id = 0; GetDisplay(id, 0, ref d); id++)
                {
                    //System.Diagnostics.Debug.WriteLine(
                    //    String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                    //             id,
                    //             d.DeviceName,
                    //             d.DeviceString,
                    //             d.StateFlags,
                    //             d.DeviceID,
                    //             d.DeviceKey
                    //             )
                    //              );
                    displays.Add(d);
                    d = new DISPLAY_DEVICE();
                    d.cb = 840;
                }

                return displays.ToArray();
            }

            public static DEVMODE GetDisplaySetting()
            {
                string res = "";

                DllImportCaller.lib.EnumDisplaySettings7(null, 0, ref res);

                var item = new Phone.Display.DEVMODE();
                item.dmSize = 192;

                var sp = res.Split('\n');
                item.dmDeviceName = sp[0];
                item.dmSpecVersion = short.Parse(sp[1]);
                item.dmDriverVersion = short.Parse(sp[2]);
                item.dmSize = short.Parse(sp[3]);
                item.dmDriverExtra = short.Parse(sp[4]); ;
                item.dmFields = int.Parse(sp[5]); ;

                item.dmOrientation = short.Parse(sp[6]);
                item.dmPaperSize = short.Parse(sp[7]);
                item.dmPaperLength = short.Parse(sp[8]);
                item.dmPaperWidth = short.Parse(sp[9]);
                item.dmScale = short.Parse(sp[10]);
                item.dmCopies = short.Parse(sp[11]);

                item.dmDefaultSource = short.Parse(sp[12]);
                item.dmPrintQuality = short.Parse(sp[13]);
                item.dmColor = short.Parse(sp[14]);
                item.dmDuplex = short.Parse(sp[15]);
                item.dmYResolution = short.Parse(sp[16]);
                item.dmTTOption = short.Parse(sp[17]);
                item.dmCollate = short.Parse(sp[18]);

                item.dmFormName = sp[19];
                item.dmLogPixels = short.Parse(sp[20]);
                item.dmBitsPerPel = short.Parse(sp[21]);
                item.dmPelsWidth = int.Parse(sp[22]);
                item.dmPelsHeight = int.Parse(sp[23]);
                item.dmDisplayFlags = int.Parse(sp[24]);
                item.dmDisplayFrequency = int.Parse(sp[25]);
                item.dmDisplayOrientation = int.Parse(sp[26]);

                return item;
            }


            [Flags()]
            public enum DisplayDeviceStateFlags : int
            {
                /// <summary>The device is part of the desktop.</summary>
                AttachedToDesktop = 0x1,
                MultiDriver = 0x2,
                /// <summary>The device is part of the desktop.</summary>
                PrimaryDevice = 0x4,
                /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
                MirroringDriver = 0x8,
                /// <summary>The device is VGA compatible.</summary>
                VGACompatible = 0x16,
                /// <summary>The device is removable; it cannot be the primary display.</summary>
                Removable = 0x20,
                /// <summary>The device has more display modes than its output devices support.</summary>
                ModesPruned = 0x8000000,
                Remote = 0x4000000,
                Disconnect = 0x2000000
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public struct DISPLAY_DEVICE
            {
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
                public int cb;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
                public string DeviceName;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
                public string DeviceString;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
                public DisplayDeviceStateFlags StateFlags;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
                public string DeviceID;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
                public string DeviceKey;
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct POINTL
            {
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I4)]
                public int x;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I4)]
                public int y;
            }

            [Flags()]
            public enum DM : int
            {
                Orientation = 0x1,
                PaperSize = 0x2,
                PaperLength = 0x4,
                PaperWidth = 0x8,
                Scale = 0x10,
                Position = 0x20,
                NUP = 0x40,
                DisplayOrientation = 0x80,
                Copies = 0x100,
                DefaultSource = 0x200,
                PrintQuality = 0x400,
                Color = 0x800,
                Duplex = 0x1000,
                YResolution = 0x2000,
                TTOption = 0x4000,
                Collate = 0x8000,
                FormName = 0x10000,
                LogPixels = 0x20000,
                BitsPerPixel = 0x40000,
                PelsWidth = 0x80000,
                PelsHeight = 0x100000,
                DisplayFlags = 0x200000,
                DisplayFrequency = 0x400000,
                ICMMethod = 0x800000,
                ICMIntent = 0x1000000,
                MediaType = 0x2000000,
                DitherType = 0x4000000,
                PanningWidth = 0x8000000,
                PanningHeight = 0x10000000,
                DisplayFixedOutput = 0x20000000
            }


            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            public class DEVMODE
            {
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
                public string dmDeviceName;
                public short dmSpecVersion;
                public short dmDriverVersion;
                public short dmSize;
                public short dmDriverExtra;
                public int dmFields;

                public short dmOrientation;
                public short dmPaperSize;
                public short dmPaperLength;
                public short dmPaperWidth;

                public short dmScale;
                public short dmCopies;
                public short dmDefaultSource;
                public short dmPrintQuality;
                public short dmColor;
                public short dmDuplex;
                public short dmYResolution;
                public short dmTTOption;
                public short dmCollate;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
                public string dmFormName;
                public short dmLogPixels;
                public short dmBitsPerPel;
                public int dmPelsWidth;
                public int dmPelsHeight;

                public int dmDisplayFlags;
                public int dmDisplayFrequency;
                public int dmDisplayOrientation;
            }
            

            public static bool MultiMonitorSupport
            {
                get
                {
                    return Phone.OS.GetSystemMetrics(OS.SystemMetric.SM_CMONITORS) != 0;
                }
            }
        }
    }
}
