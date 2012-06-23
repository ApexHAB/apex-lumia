using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Threading;

namespace ApexLumia
{
    public class Location
    {

        #region "Location Properties (Private & Public)"
        private bool _status;
        public bool status { get { return _status; } }

        private string _latitude;
        public string latitude { get { return _latitude; } }

        private string _longitude;
        public string longitude { get { return _longitude; } }

        private string _speed;
        public string speed { get { return _speed; } }

        private string _heading;
        public string heading { get { return _heading; } }

        private string _altitude;
        public string altitude { get { return _altitude; } }

        private string _horizontalaccuracy;
        public string horizontalaccuracy { get { return _horizontalaccuracy; } }

        private string _verticalaccuracy;
        public string verticalaccuracy { get { return _verticalaccuracy; } }

        private GeoCoordinate _currentlocation;
        public GeoCoordinate currentlocation { get { return _currentlocation; } }
        #endregion

        GeoCoordinateWatcher watcher;

        /// <summary>
        /// Constructor: Creates the GeoCoordinateWatcher object, sets the Positioning accuracy, movement threshold and Event Handlers.
        /// </summary>
        public Location()
        {
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High); // High accuracy - use GPS.
            watcher.MovementThreshold = 1;  // Will trigger whenever the phone has moved at least 1 metre.

            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcherStatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcherPositionChanged);         
        }

        /// <summary>
        /// Starts the location service in a new thread.
        /// </summary>
        public void start(){
            new Thread(startLocationService).Start();
        }

        /// <summary>
        /// Stops the location service.
        /// </summary>
        public void stop(){
            watcher.Stop();
            _status = false;
        }

        /// <summary>
        /// Tries to start the location service.
        /// </summary>
        void startLocationService(){
            watcher.TryStart(true, TimeSpan.FromMilliseconds(60000));
        }

        /// <summary>
        /// Event Handler: Handles status change of location service.
        /// </summary>
        void watcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    _status = false;
                    MessageBox.Show("Location Service is disabled or not functioning - fix it!");
                    break;
                case GeoPositionStatus.Initializing:
                    _status = false;
                    break;
                case GeoPositionStatus.NoData:
                    _status = false;
                    break;
                case GeoPositionStatus.Ready:
                    _status = true;
                    break;
            }
        }

        /// <summary>
        /// Event Handler: Called whenever the position changes and updates the class properties to the latest data.
        /// </summary>
        void watcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e){

            if (e.Position.Location.IsUnknown) { _status = false; return; }

            _latitude = e.Position.Location.Latitude.ToString("0.000000");
            _longitude = e.Position.Location.Longitude.ToString("0.000000");
            _speed = e.Position.Location.Speed.ToString("0.0");
            _heading = e.Position.Location.Course.ToString("0.0");
            _altitude = e.Position.Location.Altitude.ToString("0.0");
            _horizontalaccuracy = e.Position.Location.HorizontalAccuracy.ToString("0.0");
            _verticalaccuracy = e.Position.Location.VerticalAccuracy.ToString("0.0");
            _currentlocation = e.Position.Location;
            _status = true;

        }

    }
}
