﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.IO.IsolatedStorage;

namespace ApexLumia
{
    public class FlightLoop
    {

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        public void start()
        {
            Thread loopThread = new Thread(new ThreadStart(this.loop));
            loopThread.IsBackground = true;
            loopThread.Name = "Flight Loop";

            _isRunning = true;
            loopThread.Start();
        }

        public void stop()
        {
            _isRunning = false;
        }

        private void loop()
        {

            //Get settings
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            // Start location class
            var location = new Location();
            location.start();


            while (_isRunning == true)
            {
                /////////////////////////////////////
                // Main Loop Here. Yay. Whoop. No. //
                /////////////////////////////////////

                // Collect Data into Sentence object

                var sentence = new Sentence((string)settings["sentenceCallsign"], (int)settings["sentenceID"], location.latitude, location.longitude, location.altitude);
                sentence.sentenceData.Add(location.speed);

                // Construct sentence, checksum etc.

                // Log sentence & data

                // Transmit sentence

                // Upload to habitat, if internet

                // Twitter, if internet

                // Take photo - or video?

                // Upload photos to SkyDrive

                // Probably wait a bit


            }
        }

    }
}
