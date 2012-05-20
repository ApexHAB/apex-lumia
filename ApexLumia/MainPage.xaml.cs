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

    }



}