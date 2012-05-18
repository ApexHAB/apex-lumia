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
        public static partial class Sound
        {
            private static readonly System.Collections.Generic.List<ulong> systemVolumes = new System.Collections.Generic.List<ulong>(new ulong[] 
            { 
                0, 2426704036, 2491126907, 2555549778, 2619972649, 2684395520, 2748818391, 2813241262, 2877664133, 2942087004, 3006509875, 3070932746, 3135355617, 3199778488, 3264201359, 3328624230, 3393047101, 3457469972, 3521892843, 3586315714, 3650738585, 3715161456, 3779584327, 3844007198, 3908430069, 3972852940, 4037275811, 4101698682, 4166121553, 4230544424, 4294967295 
            });

            /// <summary>
            /// A range from "0 to 30" (WP7 standards). The priminaty master phone volume. controls all sounds in all programs.                                                                                                                                                                  
            /// </summary>
            public static byte MasterVolume
            {
                set
                {
                    ulong fix = value;

                    if (value == 255) //from 0 and do -- (fuggin byte.., hate you 0-- = 255)
                    {
                        fix = 0;
                    }

                    if (fix > 30)
                    {
                        fix = 30;
                    }

                    var newVol = systemVolumes[(int)fix];

                    DllImportCaller.lib.waveOutSetVolume7(newVol);
                }
                get
                {
                    ulong volume;
                    DllImportCaller.lib.waveOutGetVolume7(out volume);
                    var i = systemVolumes.IndexOf(volume);

                    return (byte)i;
                }
            }

            /// <summary>
            /// Amount of speakers, including headjacks and speakers
            /// </summary>
            public static int NumberOUTDevices
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("coredll", "waveOutGetNumDevs");
                }
            }
            /// <summary>
            /// Amount of microphones
            /// </summary>
            public static int NumberINDevices
            {
                get
                {
                    return DllImportCaller.lib.VoidCall("coredll", "waveInGetNumDevs");
                }
            }

            public static int GetDeviceHWND()
            {
                return DllImportCaller.lib.waveGetHWAVEOUT();
            }
        }
    }
}
