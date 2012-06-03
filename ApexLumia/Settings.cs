﻿using System;
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

        public ObservableCollection<SettingsItems> settingsGeneral { get; private set; }
        public ObservableCollection<SettingsItems> settingsRTTY { get; private set; }
        public ObservableCollection<SettingsItems> settingsHabitat { get; private set; }
        public ObservableCollection<SettingsItems> settingsCamera { get; private set; }
        public ObservableCollection<SettingsItems> settingsTwitter { get; private set; }
        public ObservableCollection<SettingsItems> settingsLogging { get; private set; }

        /// <summary>
        /// Constructor: Make collections for each category of settings and then loads settings into them.
        /// </summary>
        public Settings()
        {
            
            this.settingsGeneral = new ObservableCollection<SettingsItems>();
            this.settingsRTTY = new ObservableCollection<SettingsItems>();
            this.settingsHabitat = new ObservableCollection<SettingsItems>();
            this.settingsCamera = new ObservableCollection<SettingsItems>();
            this.settingsTwitter = new ObservableCollection<SettingsItems>();
            this.settingsLogging = new ObservableCollection<SettingsItems>();

            LoadAllSettings();
        }


        /// <summary>
        /// Adds SettingsItems objects for each individual setting to their corresponding collection.
        /// </summary>
        public void LoadAllSettings()
        {
            settingsGeneral.Add(new SettingsItems("generalProjectName", "project name", "ApexHAB")); // 0

            settingsRTTY.Add(new SettingsItems("rttyRTTYToggle", "rtty transmission", true)); // 0

            settingsHabitat.Add(new SettingsItems("habitatHabitatToggle", "habitat upload", true)); // 0
            settingsHabitat.Add(new SettingsItems("habitatCouchURL", "couch url", "http://habitat.habhub.org")); // 1
            settingsHabitat.Add(new SettingsItems("habitatCouchDB", "couch db", "habitat")); // 2

            settingsLogging.Add(new SettingsItems("generalDataLogToggle", "data logger", true)); // 0

            settingsCamera.Add(new SettingsItems("generalCameraToggle", "camera", true)); // 0

            settingsTwitter.Add(new SettingsItems("generalTwitterToggle", "twitter update", true)); // 0

        }

        /// <summary>
        /// Loops through all the settings in each collection, setting them to their default (defined when each SettingsItems object was created)
        /// </summary>
        public void SetDefaultSettings()
        {
            // General Settings
            for (int i = 0; i <= settingsGeneral.Count-1; i++) { settingsGeneral[i].setDefault(); }
            // RTTY Settings
            for (int i = 0; i <= settingsRTTY.Count-1; i++) { settingsRTTY[i].setDefault(); }
            // Habitat Settings
            for (int i = 0; i <= settingsHabitat.Count-1; i++) { settingsHabitat[i].setDefault(); }
            // Logging Settings
            for (int i = 0; i <= settingsLogging.Count-1; i++) { settingsLogging[i].setDefault(); }
            // Camera Settings
            for (int i = 0; i <= settingsCamera.Count-1; i++) { settingsCamera[i].setDefault(); }
            // Twitter Settings
            for (int i = 0; i <= settingsTwitter.Count-1; i++) { settingsTwitter[i].setDefault(); }
        }



    }




}
