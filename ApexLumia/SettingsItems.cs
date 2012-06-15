﻿using System;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ApexLumia
{
    public class SettingsItems : INotifyPropertyChanged
    {

        IsolatedStorageSettings savedSettings;

        private String _settingID;
        private String _settingName;
        private Object _settingValue;
        private Object _settingDefaultValue;

        /// <summary>
        /// Constructor: Grabs saved settings from Isolated Storage and loads the value of this new setting.
        /// </summary>
        /// <param name="id">Used as the key for saving the setting value in IsolatedStorage.</param>
        /// <param name="name">Readable name for display on the settings page</param>
        /// <param name="defaultvalue">The default value for the setting.</param>
        public SettingsItems(String id, String name, Object defaultvalue)
        {
            savedSettings = IsolatedStorageSettings.ApplicationSettings;

            this._settingID = id;
            this._settingName = name;
            this._settingDefaultValue = defaultvalue;

            Load();
        }

        /// <summary>
        /// Gets the saved value from IsolatedStorage, or, if not available, sets the default value.
        /// </summary>
        public void Load()
        {
            if (savedSettings.Contains(_settingID)){
                _settingValue = (Object)savedSettings[_settingID];
            }else{
                setDefault();
            }
        }

        /// <summary>
        /// Save the current value to IsolatedStorage, or if it doesn't exist yet, add it and save it.
        /// </summary>
        public void Save()
        {
            if (savedSettings.Contains(_settingID))
            {
                savedSettings[_settingID] = _settingValue;
            }
            else
            {
                savedSettings.Add(_settingID, _settingValue);
            }
            savedSettings.Save();
        }

        /// <summary>
        /// Set the current value to the default and save to IsolatedStorage.
        /// </summary>
        public void setDefault()
        {
            settingValue = _settingDefaultValue;
            Save();
        }

        public String settingID
        {
            get { return _settingID; }
        }

        public String settingName
        {
            get { return _settingName; }
        }


        public Object settingValue
        {
            get { return _settingValue; }
            set
            {
                _settingValue = value;
                Save();
                OnPropertyChanged("settingValue");  // When the value changes, it will update the binded control on the settings page immediately.
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
