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
using System.Runtime.InteropServices;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public class Battery
        {
            //http://www.pinvoke.net/default.aspx/coredll/SYSTEM_POWER_STATUS_EX.html
            [StructLayout(LayoutKind.Sequential)]
            public struct SYSTEM_POWER_STATUS_EX
            {
                /// <summary>
                /// AC power status. It is one of the following values. Phone.Battery.AC_LINE_*.
                /// </summary>
                public byte ACLineStatus;
                /// <summary>
                /// Phone.Battery.BATTERY_*. Battery charge status. It can be a combination of the following values: 
                /// </summary>
                public byte BatteryFlag;
                /// <summary>
                /// Percentage of full battery charge remaining. This member can be a value in the range 0 to 100, or 255 if status is unknown. All other values are reserved. 
                /// </summary>
                public byte BatteryLifePercent;
                public byte Reserved1;
                /// <summary>
                /// Number of seconds of battery life remaining, or 0xFFFFFFFF if remaining seconds are unknown. 
                /// </summary>
                public uint BatteryLifeTime;
                /// <summary>
                /// Number of seconds of battery life when at full charge, or 0xFFFFFFFF if full lifetime is unknown. 
                /// </summary>
                public uint BatteryFullLifeTime;
                public byte Reserved2;
                /// <summary>
                /// Backup battery charge status. It can be a combination of the following values: 
                /// </summary>
                public byte BackupBatteryFlag;
                /// <summary>
                /// Percentage of full backup battery charge remaining. Must be in the range 0 to 100, or BATTERY_PERCENTAGE_UNKNOWN. 
                /// </summary>
                public byte BackupBatteryLifePercent;
                public byte Reserved3;
                /// <summary>
                /// Number of seconds of backup battery life remaining, or BATTERY_LIFE_UNKNOWN if remaining seconds are unknown. 
                /// </summary>
                public uint BackupBatteryLifeTime;
                /// <summary>
                /// Number of seconds of backup battery life when at full charge, or BATTERY_LIFE_UNKNOWN if full lifetime is unknown. 
                /// </summary>
                public uint BackupBatteryFullLifeTime;

                public override string ToString()
                {
                    return this.DumpToString();
                    //return string.Format(
                    //    "ACLineStatus: {0}\nBatteryFlag: {1}\nBatteryLifePercent: {2}\nBatteryLifeTime: {4}\nBatteryFullLifeTime: {5}\n\nBackupBatteryFlag: {7}\nBackupBatteryLifePercent: {8}\nBackupBatteryLifeTime: {10}\nBackupBatteryFullLifeTime: {11}\n\nReserved1: {3}\nReserved2: {6}\nReserved3: {9}",
                    //     ACLineStatus,      BatteryFlag,      BatteryLifePercent,      BatteryLifeTime,      BatteryFullLifeTime,      BackupBatteryFlag,      BackupBatteryLifePercent,      BackupBatteryLifeTime,       BackupBatteryFullLifeTime,         Reserved1,      Reserved2,      Reserved2);
                }
            }

            [StructLayout(LayoutKind.Sequential, Size = 53)]
            public class SYSTEM_POWER_STATUS_EX2
            {
                public ACLineStatus ACLineStatus;
                public byte BatteryFlag;
                public byte BatteryLifePercent;
                public byte Reserved1;
                public uint BatteryLifeTime;
                public uint BatteryFullLifeTime;
                public byte Reserved2;
                public byte BackupBatteryFlag;
                public byte BackupBatteryLifePercent;
                public byte Reserved3;
                public uint BackupBatteryLifeTime;
                public uint BackupBatteryFullLifeTime;
                public uint BatteryVoltage;
                public uint BatteryCurrent;
                public uint BatteryAverageCurrent;
                public uint BatteryAverageInterval;
                public uint BatterymAHourConsumed;
                public uint BatteryTemperature;
                public uint BackupBatteryVoltage;
                public BatteryChemistry BatteryChemistry;

                public override string ToString()
                {
                    return this.DumpToString();
                }
            }

            public enum ACLineStatus : byte
            {
                /// <summary>
                /// Offline
                /// </summary>
                AC_LINE_OFFLINE = 0, // 
                /// <summary>
                /// Online
                /// </summary>
                AC_LINE_ONLINE = 1, // 
                /// <summary>
                /// Backup Power
                /// </summary>
                AC_LINE_BACKUP_POWER = 2, 
                AC_LINE_UNKNOWN = 0xFF, 
                Unknown = 0xFF, 
            }

            public enum BatteryChemistry : byte
            {
                /// <summary>
                /// Alkaline battery. 
                /// </summary>
                BATTERY_CHEMISTRY_ALKALINE = 0x01,
                /// <summary>
                /// Nickel Cadmium battery. 
                /// </summary>
                BATTERY_CHEMISTRY_NICD = 0x02,
                /// <summary>
                /// Nickel Metal Hydride battery. 
                /// </summary>
                BATTERY_CHEMISTRY_NIMH = 0x03,
                /// <summary>
                /// Lithium Ion battery. 
                /// </summary>
                BATTERY_CHEMISTRY_LION = 0x04,
                /// <summary>
                /// Lithium Polymer battery. 
                /// </summary>
                BATTERY_CHEMISTRY_LIPOLY = 0x05,
                /// <summary>
                /// Zinc Air battery. 
                /// </summary>
                BATTERY_CHEMISTRY_ZINCAIR = 0x06,
                /// <summary>
                /// Battery chemistry is unknown. 
                /// </summary>
                BATTERY_CHEMISTRY_UNKNOWN = 0xFF
            }

            public const byte BATTERY_FLAG_HIGH = 0x01;
            public const byte BATTERY_FLAG_LOW = 0x02;
            public const byte BATTERY_FLAG_CRITICAL = 0x04;
            public const byte BATTERY_FLAG_CHARGING = 0x08;
            public const byte BATTERY_FLAG_NO_BATTERY = 0x80;
            public const byte BATTERY_FLAG_UNKNOWN = 0xFF;
            public const byte BATTERY_PERCENTAGE_UNKNOWN = 0xFF;
            public const uint BATTERY_LIFE_UNKNOWN = 0xFFFFFFFF;

            public const byte AC_LINE_OFFLINE = 0x00;
            public const byte AC_LINE_ONLINE = 0x01;
            public const byte AC_LINE_BACKUP_POWER = 0x02;
            public const byte AC_LINE_UNKNOWN = 0xFF;

            public static SYSTEM_POWER_STATUS_EX GetBatteryBasic()
            {
                //application crashes everytime when sending "out SYSTEM_POWER_STATUS_EX", so then we do the CString way. Allways work.

                string str = "";
                var val = DllImportCaller.lib.GetSystemPowerStatusEx7(ref str, true) > 0; //1

                if (val)
                {
                    SYSTEM_POWER_STATUS_EX powerStatus = new SYSTEM_POWER_STATUS_EX();

                    var s = str.Split('\n');
                    powerStatus.ACLineStatus = byte.Parse(s[0]);
                    powerStatus.BackupBatteryFlag = byte.Parse(s[1]);
                    powerStatus.BackupBatteryFullLifeTime = uint.Parse(s[2]);
                    powerStatus.BackupBatteryLifePercent = byte.Parse(s[3]);
                    powerStatus.BackupBatteryLifeTime = uint.Parse(s[4]);
                    powerStatus.BatteryFlag = byte.Parse(s[5]);
                    powerStatus.BatteryFullLifeTime = uint.Parse(s[6]);
                    powerStatus.BatteryLifePercent = byte.Parse(s[7]);
                    powerStatus.BatteryLifeTime = uint.Parse(s[8]);
                    
                    powerStatus.Reserved1 = byte.Parse(s[9]);
                    powerStatus.Reserved2 = byte.Parse(s[10]);
                    powerStatus.Reserved3 = byte.Parse(s[11]);

                    return powerStatus;
                }
                return default(SYSTEM_POWER_STATUS_EX);
            }
            public static SYSTEM_POWER_STATUS_EX2 GetBatteryAdvanced()
            {
                string str = "";
                var v = DllImportCaller.lib.GetSystemPowerStatusExAdv7(ref str, true);
                var val = v > 0; //or == 53. aka sizeof(SYSTEM_POWER_STATUS_EX2);

                if (val)
                {
                    SYSTEM_POWER_STATUS_EX2 powerStatus = new SYSTEM_POWER_STATUS_EX2();

                    
                    var s = str.Split('\n');
                    powerStatus.ACLineStatus = (ACLineStatus)byte.Parse(s[0]);
                    powerStatus.BackupBatteryFlag = byte.Parse(s[1]);
                    powerStatus.BackupBatteryFullLifeTime = uint.Parse(s[2]);
                    powerStatus.BackupBatteryLifePercent = byte.Parse(s[3]);
                    powerStatus.BackupBatteryLifeTime = uint.Parse(s[4]);
                    powerStatus.BatteryFlag = byte.Parse(s[5]);
                    powerStatus.BatteryFullLifeTime = uint.Parse(s[6]);
                    powerStatus.BatteryLifePercent = byte.Parse(s[7]);
                    powerStatus.BatteryLifeTime = uint.Parse(s[8]);

                    powerStatus.BatteryVoltage = uint.Parse(s[9]);
                    powerStatus.BatteryCurrent = uint.Parse(s[10]);
                    powerStatus.BatteryAverageCurrent = uint.Parse(s[11]);
                    powerStatus.BatteryAverageInterval = uint.Parse(s[12]);
                    powerStatus.BatterymAHourConsumed = uint.Parse(s[13]);
                    powerStatus.BatteryTemperature = uint.Parse(s[14]);
                    powerStatus.BackupBatteryVoltage = uint.Parse(s[15]);
                    powerStatus.BatteryChemistry = (BatteryChemistry)byte.Parse(s[16]);


                    powerStatus.Reserved1 = byte.Parse(s[17]);
                    powerStatus.Reserved2 = byte.Parse(s[18]);
                    powerStatus.Reserved3 = byte.Parse(s[19]);

                    return powerStatus;
                }
                return default(SYSTEM_POWER_STATUS_EX2);
            }
        }
    }
}
