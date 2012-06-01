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
            this.settingsHabitat = new ObservableCollection<SettingsItems>();
            this.settingsCamera = new ObservableCollection<SettingsItems>();
            this.settingsTwitter = new ObservableCollection<SettingsItems>();
            this.settingsLogging = new ObservableCollection<SettingsItems>();

            LoadAllSettings();
        }

        public ObservableCollection<SettingsItems> settingsGeneral { get; private set; }
        public ObservableCollection<SettingsItems> settingsRTTY { get; private set; }
        public ObservableCollection<SettingsItems> settingsHabitat { get; private set; }
        public ObservableCollection<SettingsItems> settingsCamera { get; private set; }
        public ObservableCollection<SettingsItems> settingsTwitter { get; private set; }
        public ObservableCollection<SettingsItems> settingsLogging { get; private set; }

        public void LoadAllSettings()
        {
            settingsGeneral.Add(new SettingsItems("generalProjectName", "project name", "ApexHAB")); // 0

            settingsRTTY.Add(new SettingsItems("generalRTTYToggle", "rtty transmission", true)); // 0

            settingsHabitat.Add(new SettingsItems("generalHabitatToggle", "habitat upload", true)); // 0

            settingsLogging.Add(new SettingsItems("generalDataLogToggle", "data logger", true)); // 0

            settingsCamera.Add(new SettingsItems("generalCameraToggle", "camera", true)); // 0

            settingsTwitter.Add(new SettingsItems("generalTwitterToggle", "twitter update", true)); // 0

        }

        public void SetDefaultSettings()
        {
            // General Settings
            for (int i = 0; i <= settingsGeneral.Count; i++) { settingsGeneral[i].setDefault(); }
            // RTTY Settings
            for (int i = 0; i <= settingsRTTY.Count; i++) { settingsRTTY[i].setDefault(); }
            // Habitat Settings
            for (int i = 0; i <= settingsHabitat.Count; i++) { settingsHabitat[i].setDefault(); }
            // Logging Settings
            for (int i = 0; i <= settingsLogging.Count; i++) { settingsLogging[i].setDefault(); }
            // Camera Settings
            for (int i = 0; i <= settingsCamera.Count; i++) { settingsCamera[i].setDefault(); }
            // Twitter Settings
            for (int i = 0; i <= settingsTwitter.Count; i++) { settingsTwitter[i].setDefault(); }
        }

    }




}
