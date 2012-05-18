using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Reflection;


namespace CSharp___DllImport
{
    //[System.Runtime.InteropServices.ComImport, System.Guid("A892EDBD-E9F5-4FBA-8BDE-A12C1227221E"), 


    public partial class MainPage
    {
        private int rand() // prevent raging exception (tile url collision) "?id=" + rand()
        {
            Random random = new Random();
            return random.Next(0, int.MaxValue);
        }

        private void spawnTile(HackedTileInterface tile)
        {
            HackedShellTile.CreateFreedomTile(new Guid("{8dc5214e-88fa-4c2d-a379-2cd74fe24b72}"), "?id=" + rand(), "_default", tile);
        }

        private void RenderDemos()
        {
            int i = 0;
            foreach (var item in new Demo[] 
            {
                new Demo { Call = () => 
                {
                    spawnTile(new HackedXboxTile("Hello XDA!"));

                }, Name = "Create Xbox tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedCalendarTile("appointmentTitle", "appointmentDescription", "appointmentLocation"));

                }, Name = "Create Calendar tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedIconTile("res://StartMenu!TokenIE.png", "@BrowsuiRes.dll,-12251"));

                }, Name = "Create IE icon tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedWideIconTile("res://StartMenu!TokenIE.png", "@BrowsuiRes.dll,-12251"));

                }, Name = "Create wide IE icon tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedSeparatorTitle());

                }, Name = "Create separator tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedSimpleTextTile("My title", "Im subtitle"));

                }, Name = "Create simple text tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedTrippleRowTitle("res://StartMenu!TokenIE.png", "res://StartMenu!token.xboxlive.png", "res://StartMenu!TokenCamera.png"));

                }, Name = "Create tripple row tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedPeopleHubTitle("People of XDA", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png", "res://StartMenu!TokenIE.png"));

                }, Name = "Create people hub tile"},
                new Demo { Call = () => 
                {
                    spawnTile(new HackedWidePictureAnimator(
                        @"file://\Application Data\Photos\Pictures_00.jpg",
                        @"file://\Application Data\Photos\Pictures_01.jpg",
                        @"file://\Application Data\Photos\Pictures_02.jpg",
                        @"file://\Application Data\Photos\Pictures_03.jpg"
                        /*Up to 18 supported*/
                        ));

                }, Name = "Create wide picture animator"},
                null,
                new Demo { Call = ()=> 
                {
                    var winds = Phone.TaskManager.GetEnumWindows(false,false);
                    var procs = Phone.TaskManager.AllProcesses();

                    foreach (var item in procs)
                    {
                        foreach (var item2 in winds)
                        {
                            //if(item.MemoryInfo ==item2)
                            //{

                            //}
                        }
                    }

                    MessageBox.Show(string.Join("\n", Phone.TaskManager.AllProcesses().Select(p => p.RAW.szExeFile + " - " + p.RAW.th32ProcessID).ToArray()));  
                }, Name ="Running process'es list (OK)" },
                //null,
                //new Demo { Call = ()=> 
                //{
                //    MessageBox.Show("Bugged");
                //    //string str = ""; 
                //    //var o = DllImportCaller.lib.PrntScr(ref str);
                //    //MessageBox.Show("Captured a string at len: " + str.Length.ToString());

                //}, Name ="Capture screen" },
                null,
                new Demo { Call = ()=> 
                {
                    var t = Phone.OS.Shutdown(EWX.EWX_SHUTDOWN);

                }, Name ="Shutdown Operating system (OK)" },
                null,
                new Demo { Call = ()=> 
                {
                    var proc = Phone.WP7Process.GetCurrentProcess();

                    proc.Kill(0); 

                }, Name ="Kill this TaskHost.exe (OK)" },
                new Demo { Call = ()=> 
                {
                    var info = Phone.TaskHost.GetCurrenHostInfo();
                    var rep = info.ToString().Replace(",", ",\n");
                    MessageBox.Show(rep);

                }, Name ="Show current TaskHost info (OK)" },
                null,
                new Demo { Call = ()=> 
                {
                    Phone.Screen.CaptureScreenToPictures();
                    MessageBox.Show("Dumped/saved screen to \"Pictures\"", "SAVED", MessageBoxButton.OK);

                }, Name ="Capture screen to pix (SLOW) (OK)" },
                null,
                new Demo { Call = ()=> 
                {
                    MessageBox.Show(Phone.Battery.GetBatteryAdvanced().ToString());

                }, Name ="System battery info (OK)" },
                 new Demo { Call = ()=> 
                {
                    MessageBox.Show(Phone.OS.Memory.GetMemoryStatus().ToString());

                }, Name ="System RAM info (OK)" },
                null,
                new Demo { Call = ()=> 
                {
                    var uptime = Phone.OS.Uptime;
                    MessageBox.Show("Days: " + uptime.Days + "\nHours: " + uptime.Hours + "\nMinutes: " + uptime.Minutes + "\nSeconds:" + uptime.Seconds);

                }, Name ="System uptime (OK)" },
                null,
                new Demo { Call = ()=> { Phone.Vibrator.Vibrate(); }, Name ="Vibrator ON (OK)" },
                new Demo { Call = ()=> { Phone.Vibrator.Stop(); }, Name ="Vibrator STOP (OK)" },
                null,
                new Demo { Call = ()=> { Phone.Sound.MasterVolume++; }, Name ="MasterVolume++ (OK)" },
                new Demo { Call = ()=> { Phone.Sound.MasterVolume--; }, Name ="MasterVolume-- (OK)" },
                null,
                new Demo { Call = ()=> { Phone.Zune.NextSong(); }, Name ="Zune NextSong (OK)" },
                new Demo { Call = ()=> { Phone.Zune.PrevSong(); }, Name ="Zune PrevSong (OK)" },
                null,
                new Demo { Call = ()=> { Phone.Zune.Pause(); }, Name ="Zune Pause (OK)" },
                new Demo { Call = ()=> { Phone.Zune.Resume(); }, Name ="Zune Resume (OK)" },
                new Demo { Call = ()=> { MessageBox.Show("Code: " + ((Win32ErrorCode) DllImportCaller.lib.ShellExecuteEx7(@"\Windows\calc7.exe", "")).ToString()); }, Name ="Start calculator (FAIL)" },
                null,
                new Demo { Call = ()=> 
                {
                    using (var f = Phone.IO.File7.Open("\\Windows\\0000_System.Windows.xaml", "r"))
                    {
                        var s = f.GetFileSize();

                        MessageBox.Show("Kilobyte: " + (s / 1024).ToString());
                    }

                }, Name ="0000_System.Windows.xaml SIZE (OK)" },
                null,
                //new Demo { Call = ()=> 
                //{
                //  IntPtr ptr;
                //  DllImportCaller.lib.CaptureScreen(out ptr);

                //  if (ptr != IntPtr.Zero)
                //  {
                //      DllImportCaller.lib.DeleteCapturedScreen(ptr);
                //  }

                //  MessageBox.Show(ptr.ToString(), "IntPtr of screen capture", MessageBoxButton.OK);

                //}, Name ="Capture screen (IntPtr) (OK)" }, 
                //null,
                new Demo { Call = ()=> 
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                       // tt = new System.Threading.Thread(new System.Threading.ThreadStart(() => // bugs without thread here ... ???
                       //{
                           System.Diagnostics.Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
                           int res;

                           DllImportCaller.lib.MessageBox7("Native Messabe box :)", "Native caption", MB.MB_OKCANCEL, out res);

                           MainPage.This.Dispatcher.BeginInvoke(() =>
                           {
                               MessageBox.Show("Result: " + res.ToString());
                           });

                       //})); tt.Start();
                    });

                }, Name ="Native MessageBox (OK)" },
                null,
                new Demo { Call = ()=> {Phone.Zune.Radio.InOn = true; }, Name ="Turn on radio (headphones!) (OK)" },
                null,
                new Demo { Call = ()=> { /*Phone.Clipboard.Value = "Hack?";*/ }, Name ="Copy \"Hack?\" (NoDo update) (FAIL)" },
                null,
                new Demo { Call = ()=> { Phone.Zune.ShutdownZune(); }, Name ="Zune ShutdownZune (FAIL)" },
                new Demo { Call = ()=> { Phone.Zune.StartZune(); }, Name ="Zune StartZune (FAIL)" },
                new Demo { Call = ()=> { Phone.Zune.Stop(); }, Name ="Zune Stop (FAIL)" },
                null,
                new Demo { Call = ()=> { MessageBox.Show("I know you want more, but i dont have more at the moment binded to buttons :)"); }, Name ="More to come :)" },

            })
            {
                if (item != null)
                {
                    var b = new Button { Content = item.Name, Tag = item };

                    b.SetValue(Grid.RowProperty, i);


                    b.Click += (o, e2) =>
                    {
                        ((o as Button).Tag as Demo).Call();
                    };
                    API.Children.Add(b);
                }
                else
                {
                    Button b = new Button();
                    b.SetValue(Grid.RowProperty, i);

                    b.IsEnabled = false;
                    b.Opacity = 0.01;

                    API.Children.Add(b);
                }
                API.RowDefinitions.Add(new RowDefinition());

                i++;
            }
        }
        public class Demo
        {
            public Action Call { get; set; }
            public string Name { get; set; }
        }
    }
}
