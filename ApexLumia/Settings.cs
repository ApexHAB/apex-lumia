using System;
using System.Net;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ApexLumia
{
    public class Settings
    {
        //IsolatedStorageSettings settings;

        public Settings()
        {
            //settings = IsolatedStorageSettings.ApplicationSettings;
            this.settingsList = new ObservableCollection<SettingsList>();
            LoadSettings();
        }

        public ObservableCollection<SettingsList> settingsList { get; private set; }

        public void LoadSettings()
        {
            this.settingsList.Add(new SettingsList() { settingName = "project name" });
        }

    }

    public class SettingsList
    {

        public string settingName { get; set; }


    }


}
