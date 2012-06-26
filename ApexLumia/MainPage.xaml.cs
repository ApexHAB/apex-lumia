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

        
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set context for binded controls to this class.
            DataContext = this;
            dataCount = "0000";

            Twitter tweet = new Twitter("apexlumia", "Zf9SUn9D4TKqeq5j8TXwaw", "g7ywiXPaCffibK6iOa8KGR12iMZKKzkqrN2GTffpg", "616537021-B4vbyk3n4v6ht3or8r4YFrCjOHNDrkaohrdFn6lY", "THCNSCDw4OQU5zPNax4pUBoLIVwIvjWX6fdzWkwXXo");
            tweet.newStatus("Hello Everyone!");
            //System.Diagnostics.Debug.WriteLine(tweet.generateAuthorizationHeader("https://api.twitter.com/1/statuses/update.json","POST","Hello World"));
        }


    }



}