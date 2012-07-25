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

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set context for binded controls to this class.
            DataContext = this;
            dataCount = "0000";



        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            camera = new Camera(cameraViewBrush,0);
            camera.start();
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            camera.takePhoto();
        }



    }



}