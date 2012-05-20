using System;
using System.Net;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace ApexLumia
{
    public class Settings
    {
        IsolatedStorageSettings settings;

        public Settings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public string settingProjectName
        {
            get { return (string)settings["ProjectName"]; }
        }

    }


}
