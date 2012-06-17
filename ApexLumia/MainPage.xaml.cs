using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace ApexLumia
{
    public partial class MainPage : PhoneApplicationPage
    {

        public string dataCount { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set context for binded controls to this class.
            DataContext = this;
            dataCount = "0000";

            
            
        }

        private void button1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            HabitatInterface habitat = new HabitatInterface("http://habitat.habhub.org", "habitat");
            habitat.writeSomethingRandom("{\"type\": \"listener_telemetry\",\"time_created\": 1339984864,\"time_uploaded\": 1339984868,\"data\": {\"callsign\": \"LUMIA_CHASE\",\"time\": {\"hour\": 21,\"minute\": 0,\"second\": 0},\"latitude\": -35.11,\"longitude\": 137.567,\"altitude\": 12}}");
        }


    }



}