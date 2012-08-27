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

namespace ApexLumia
{
    public partial class MainPage : PhoneApplicationPage
    {

        public string dataCount { get; set; }

        public Pushpin mapLocation = new Pushpin();

        Camera camera;
        VideoCamera video;
        SkyDrive skydrive;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set context for binded controls to this class.
            DataContext = this;
            dataCount = "0000";

            Crc16Ccitt crc = new Crc16Ccitt();
            ushort result = crc.ComputeChecksum(System.Text.UTF8Encoding.UTF8.GetBytes("habitat"));
            string result2 = result.ToString("X");
            System.Diagnostics.Debug.WriteLine(result2);


        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //camera = new Camera(cameraViewBrush,0);
            //camera.start();
            //video = new VideoCamera(cameraViewBrush, viewfinderRectangle, 0);
            //video.start();
        }




    }



}