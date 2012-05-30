using System;
using System.Windows;
using System.Net;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ApexLumia
{
    public class Settings
    {
        //IsolatedStorageSettings storagesettings;

        public Settings()
        {
            //settings = IsolatedStorageSettings.ApplicationSettings;
            this.settingsGeneral = new ObservableCollection<SettingsItems>();
            this.settingsRTTY = new ObservableCollection<SettingsItems>();

            LoadAllSettings();
        }

        public ObservableCollection<SettingsItems> settingsGeneral { get; private set; }
        public ObservableCollection<SettingsItems> settingsRTTY { get; private set; }

        public void LoadAllSettings()
        {
            settingsGeneral.Add(new SettingsItems("generalProjectName","project name", "ApexHAB","text"));
            settingsRTTY.Add(new SettingsItems("another","a setting to do with RTTY", "value of the setting","bam"));
        }

    }




}
