using System;
using System.Net;
using System.Windows;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ApexLumia
{
    public class SettingsItems
    {

        IsolatedStorageSettings savedSettings;

        private string _settingID;
        private string _settingName;
        private object _settingValue;
        private object _settingDefaultValue;

        public SettingsItems(string id, string name, object defaultvalue)
        {
            savedSettings = IsolatedStorageSettings.ApplicationSettings;

            this._settingID = id;
            this._settingName = name;
            this._settingDefaultValue = defaultvalue;

            Load();
        }

        public void Load()
        {
            if (savedSettings.Contains(_settingID)){
                _settingValue = (object)savedSettings[_settingID];
            }else{
                setDefault();
            }
        }

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

        public void setDefault()
        {
            _settingValue = _settingDefaultValue;
            Save();
        }

        public string settingID
        {
            get { return _settingID; }
        }

        public string settingName
        {
            get { return _settingName; }
        }

        public object settingValue
        {
            get { return _settingValue; }
            set { _settingValue = value; Save();}
        }

    }
}
