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
using System.Windows.Threading;
using Microsoft.Live;
using Microsoft.Live.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ApexLumia
{
    public partial class SettingsPage : PhoneApplicationPage
    {

        Settings settings;
        SkyDrive skydrive = new SkyDrive();


        public SettingsPage()
        {
            InitializeComponent();

            settings = new Settings();
            DataContext = settings;

            signInButton.SessionChanged += new EventHandler<LiveConnectSessionChangedEventArgs>(skydrive.LoginCompleted);
            skydrive.dataFullName = skydrivename;
            skydrive.dataPhoto = skydrivephoto;
        }

        private void btnRestoreDefaults_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            settings.SetDefaultSettings();
            MessageBox.Show("Default Settings Restored.");
        }





        



    }
}