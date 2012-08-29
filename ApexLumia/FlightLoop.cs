using System;
using System.Threading;
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;
using System.ComponentModel;

namespace ApexLumia
{
    public class FlightLoop : INotifyPropertyChanged
    {

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        Camera camera;

        int sentenceID;

        public string dataSentenceID { get; set; }
        public string dataTime { get; set; }
        public string dataLat { get; set; }
        public string dataLong { get; set; }
        public string dataAltitude { get; set; }
        public string dataSpeed { get; set; }

        public string statusTransmit { get; set; }
        public string statusDataConnection { get; set; }
        public string statusHabitat { get; set; }
        public string statusTwitter { get; set; }
        public string statusLogger { get; set; }
        public string statusCamera { get; set; }

        System.Windows.Threading.DispatcherTimer cameraTimer;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public void start(Camera _camera)
        {
            Thread loopThread = new Thread(new ThreadStart(this.loop));
            loopThread.IsBackground = true;
            loopThread.Name = "Flight Loop";

            camera = _camera;

            _isRunning = true;
            loopThread.Start();

            // Start camera on a timer
            
            cameraTimer = new System.Windows.Threading.DispatcherTimer();
            if ((bool)settings["generalCameraToggle"])
            {
                cameraTimer.Interval = new TimeSpan(0, 0, (int)settings["cameraInterval"]);
                cameraTimer.Tick += new EventHandler(cameraTimer_Tick);
                cameraTimer.Start();
            }

        }

        public void stop()
        {
            _isRunning = false;
            if ((bool)settings["generalCameraToggle"]) { cameraTimer.Stop(); }
        }

        private void loop()
        {

            // Start location class
            var location = new Location();
            location.start();

            // Start RTTY Audio
            var rtty = new RTTY((double)settings["rttySineFreq"], (int)settings["rttySampleRate"], (int)settings["rttyBaud"], 0, (double)settings["rttyLow"],(double)settings["rttyHigh"],(int)settings["rttyStopBits"]); // Defaults are fine
            if ((bool)settings["rttyRTTYToggle"]) { rtty.Start(); rtty.transmitSentence("Tranmission started!"); }

            // Start habitat & couch
            var habitat = new Habitat((string)settings["habitatCouchURL"], (string)settings["habitatCouchDB"], (string)settings["sentenceCallsign"]);

            // Start Twitter
            var twitter = new Twitter((string)settings["twitterUsername"], (string)settings["twitterConsumerKey"], (string)settings["twitterConsumerSecret"], (string)settings["twitterAccessToken"], (string)settings["twitterAccessSecret"]);


            if (settings.Contains("sentenceID"))
            {
                sentenceID = (int)settings["sentenceID"];
            }
            else
            {
                sentenceID = 0;
            }

            OnPropertyChanged("dataSentenceID");

            while (_isRunning == true)
            {
                /////////////////////////////////////
                // Main Loop Here. Yay. Whoop. No. //
                /////////////////////////////////////

                sentenceID++;
                settings["sentenceID"] = sentenceID;
                dataSentenceID = sentenceID.ToString();
                OnPropertyChanged("dataSentenceID");

                bool hasNetworkConnection = NetworkInterface.GetIsNetworkAvailable();
                statusDataConnection = hasNetworkConnection.ToString();
                OnPropertyChanged("statusDataConnection");

                // Collect Data into Sentence object
                var sentence = new Sentence((string)settings["sentenceCallsign"], sentenceID, location.latitude, location.longitude, location.altitude);
                sentence.sentenceData.Add(location.speed);
                if (hasNetworkConnection) { sentence.sentenceData.Add("1"); } else { sentence.sentenceData.Add("0"); }

                dataAltitude = location.altitude;
                OnPropertyChanged("dataAltitude");
                dataLat = location.latitude;
                OnPropertyChanged("dataLat");
                dataLong = location.longitude;
                OnPropertyChanged("dataLong");
                dataSpeed = location.speed;
                OnPropertyChanged("dataSpeed");
                dataTime = System.DateTime.Now.ToString("HH:mm:ss");
                OnPropertyChanged("dataTime");

                // Construct sentence
                if(sentence.compileSentence()){

                    // Log sentence & data

                    // Upload to habitat, if internet
                    if (hasNetworkConnection && (bool)settings["habitatHabitatToggle"])
                    {
                        habitat.uploadTelemetry(sentence.wholeSentence);
                        statusHabitat = habitat.status.ToString();
                        OnPropertyChanged("statusHabitat");
                    }
                    else
                    {
                        statusHabitat = "False";
                        OnPropertyChanged("statusHabitat");
                    }

                    // Transmit sentence
                    if (rtty.isRunning)
                    {
                        rtty.transmitSentence(sentence.wholeSentence);
                        statusTransmit = "True";
                        OnPropertyChanged("statusTransmit");
                    }
                    else
                    {
                        statusTransmit = "False";
                        OnPropertyChanged("statusTransmit");
                    }
                }

                // Twitter, if internet
                if (hasNetworkConnection && (bool)settings["twitterTwitterToggle"])
                {
                    string tweet = "I'm at " + location.latitude + ", " + location.longitude + " at an altitude of " + location.altitude + "m #apexhab #ukhas";
                    twitter.newStatusAsync(tweet, location.latitude, location.longitude);
                    statusTwitter = twitter.status.ToString();
                    OnPropertyChanged("statusTwitter");
                }
                else
                {
                    statusTwitter = "False";
                    OnPropertyChanged("statusTwitter");
                }


                // Upload photos to SkyDrive

                // Probably wait a bit
                Thread.Sleep(2000);

            }

            // End & Close stuff, possibly not necessary
            
            location.stop();
            rtty.Stop();

        }

        void cameraTimer_Tick(object sender, EventArgs e)
        {
            if (camera.isRunning) { camera.takePhoto(); statusCamera = "True"; OnPropertyChanged("statusCamera"); } else { statusCamera = "False"; OnPropertyChanged("statusCamera"); }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(()=>
                { 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }

        #endregion

    }
}
