﻿using System;
using System.Net;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ApexLumia
{
    public class SettingsItems
    {
        private string _settingID;
        private string _settingName;
        private string _settingValue;

        public int IndexOf(string _settingID);

        public SettingsItems(string id, string name, string value)
        {
            this._settingID = id;
            this._settingName = name;
            this._settingValue = value;
        }

        public string settingID
        {
            get { return _settingID; }
        }

        public string settingName
        {
            get { return _settingName; }
        }

        public string settingValue
        {
            get { return _settingValue; }
            set { _settingValue = value; }
        }

    }
}
