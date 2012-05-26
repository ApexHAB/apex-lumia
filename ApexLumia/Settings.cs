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
        IsolatedStorageSettings settings;

        public Settings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
            this.settingsGeneral = new ObservableCollection<Settings>();
        }

        public ObservableCollection<Settings> settingsGeneral { get; private set; }

        public string settingProjectName
        {
            get { return (string)settings["ProjectName"]; }
        }

        public void LoadSettings()
        {
            this.settingsGeneral.Add();
        }

    }


}
