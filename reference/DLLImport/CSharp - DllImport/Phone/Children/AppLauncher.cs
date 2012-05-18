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
using System.ComponentModel;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static class AppLauncher
        {
            private static int internalLaunch(Guid guid, string task)
            {
                var args = string.Format("app://{0}/{1}", guid.ToString("D").ToUpper(), task ?? "");
                
                var re = DllImportCaller.lib.StringCall("aygshell", "SHLaunchSessionByUri", args);

                return re;
                //"app://5B04B775-356B-4AA0-AAF8-6491FFEA5660/_default?StartURL=www.google.com"
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="guid"></param>
            /// <param name="task">app://GUID/task. _default for example, empty = default. Supports "?param=10" or "task?param=10"</param>
            /// <returns></returns>
            public static int LaunchApplication(object guid, string task = "")
            {
                if (guid == null) 
                {
                    throw new ArgumentNullException("guid"); 
                }

                Guid parse;

                if (guid is string)
                {
                    parse = new Guid((string)guid);
                }
                else if (guid is Guid)
                {
                    parse = (Guid)guid;
                }
                else
                {
                    throw new ArgumentException("argument \"guid\" must be a System.Guid or a valid Guid string");
                }

                //
                var code = internalLaunch(parse, task);


                if (code == -2147024809 && (task == "" || task == " "))
                {
                    //retry with "Default" as task
                    code = internalLaunch(parse, "Default");

                    if (code == -2147024809)
                    {
                        //still same error? "Unknown error"

                        throw new EntryPointNotFoundException(
                            "The application \"" + parse.ToString("D") + "\" does not exist, or does not have a task with the name \"" + task + "\"");
                    }
                    //-2147024871 KeyboardSettings
                }

                return code;

                /*
                 N - 32 digits: 00000000000000000000000000000000
                 D - 32 digits separated by hyphens: 00000000-0000-0000-0000-000000000000
                 B - 32 digits separated by hyphens, enclosed in braces: {00000000-0000-0000-0000-000000000000}
                 P - 32 digits separated by hyphens, enclosed in parentheses: (00000000-0000-0000-0000-000000000000)
                 X - Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces: {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x
                */
            }

            public static int LaunchBuiltInApplication(Apps app, string task = "")
            {
                string enchanted = task;

                if(task == "" || task == " ") //check if there is a better task than empty.
                {
                    var def = Ext.GetDefaultTask(app);

                    if (def != null)
                    {
                        enchanted = def;
                    }
                }
                
                if (app < Apps.FeedbackDirect && app > Apps.Settings)
                {
                    //Description Attribute
                    return LaunchApplication(GuidFromApp(app), enchanted);
                }
                else
                {
                    return LaunchApplication(Ext.GetDescription(app), enchanted);
                }
            }


            public static int OpenWebsite(string url)
            {
                return LaunchBuiltInApplication(Apps.Internet7, "_default?StartURL=" + url);
            }

            public static int OpenPicture(string fullPathName)
            {
                return LaunchBuiltInApplication(Apps.Pictures, "PhotoViewer?File=" + fullPathName);
            }

            public static Guid GuidFromApp(Apps app)
            {
                return new Guid(string.Format("5B04B775-356B-4AA0-AAF8-6491FFEA5{0}", (int)app));
            }

            public enum Apps
            {
                //FileBrowser = 600, (Banned)
                Settings = 601,
                Calc = 603,
                About = 605,
                DateTime = 606,
                LockAndWallp = 607,
                KeyboardSettings = 608,
                SMS = 610,
                CallHistory = 611,
                Calendar = 612,
                AddEmailAccount = 614,
                People = 615,
                Word = 617,
                BluetoothSettings = 620,
                AirplaneMode = 621,
                OfficeSettings = 622,
                Wifi = 623,
                SpeetchSettings = 624,
                FindMyPhone = 625,
                [DefaultTask("Default")]
                ZipView = 628,
                [DefaultTask("MusicAndVideoHub")]
                Zune = 630,
                Camera = 631,
                Pictures = 632,
                PicturesCameraCPL = 635,
                Marketplace = 633,
                Games = 634,
                PhoneUpdateSettings = 640,
                LocationSettings = 642,
                [DefaultTask("_default")]
                Internet7 = 660,
                Maps = 661,
                Internet7Settings = 663,
                //SearchSettings = 664, // do a 800 + check, DescAttr
                [Description("5B04B775-356B-4AA0-AAF8-6491FFEA5664"), DefaultTask("SearchSettings")]
                SearchSettings = 800,
                [Description("5B04B775-356B-4AA0-AAF8-6491FFEA5664"), DefaultTask("MapsSettings")] //same, but diff default task
                MapsSettings = 801,
                Feedback = 670,
                FeedbackDirect = 673,

                [Description("5B04B775-356B-4AA0-AAF8-6491FFEA5614"), DefaultTask("ShareContent")]
                SendEmail = 807, //The missing link, Default 614 did only show "add", here is send @="app://5B04B775-356B-4AA0-AAF8-6491FFEA5614/ShareContent?MailTo=%s"

                [Description("5B04B775-356B-4AA0-AAF8-6491FFEA561A")]
                SharePoint = 808,

                [Description("5B04B775-356B-4AA0-AAF8-6491FFEA5661"), DefaultTask("SearchHome")]
                SearchHome = 809
            }

            private static class Ext
            {
                public static string GetDescription(Enum value)
                {
                    return GetAttributeString<DescriptionAttribute>(value, d => d.Description);
                }

                public static string GetDefaultTask(Enum value)
                {
                    return GetAttributeString<DefaultTaskAttribute>(value, d => d.TaskName);
                }

                public static string GetAttributeString<T>(Enum value, Func<T, string> getter) where T : Attribute
                {
                    System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
                    T[] attributes =
                        (T[])fi.GetCustomAttributes(typeof(T),
                        false);

                    return (attributes.Length > 0) ? getter(attributes[0]) : null;
                }
            }
            private class DefaultTaskAttribute : Attribute
            {
                public string TaskName;
                public DefaultTaskAttribute(string value)
                {
                    TaskName = value;
                }
            }
        }
    }
}
