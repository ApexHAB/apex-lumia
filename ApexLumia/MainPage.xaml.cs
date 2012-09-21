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
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace ApexLumia
{
        public partial class MainPage : PhoneApplicationPage
    {

        public string dataCount { get; set; }

        public Pushpin mapLocation = new Pushpin();

        Camera camera;
        FlightLoop loop = new FlightLoop();
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = loop;

        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            camera = new Camera(cameraViewBrush,0);
            camera.start();
        }

        private void toggleFlight(object sender, EventArgs e)
        {
            ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];

            if (loop.isRunning)
            {
                // Then we want to stop it
                loop.stop();
                btn.Text = "Start Flight";
                btn.IconUri = new Uri("/Images/appbar.transport.play.rest.png", UriKind.Relative);
            }
            else
            {
                // Then we want to start it
                loop.start(camera, map);
                btn.Text = "Stop Flight";
                btn.IconUri = new Uri("/Images/appbar.stop.rest.png",UriKind.Relative);
            }
        }

        private void gotoSettings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
        




    }



}