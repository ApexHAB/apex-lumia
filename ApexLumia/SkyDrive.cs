using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Live;
using Microsoft.Live.Controls;

namespace ApexLumia
{
    public class SkyDrive
    {


        public bool isLoggedIn { get { return _isLoggedIn; } }
        private bool _isLoggedIn = false;

        public SkyDrive()
        {
            if (App.Session != null) { _isLoggedIn = true; }
        }

        public void Login(string clientid = "000000004C0C610D")
        {
            var auth = new LiveAuthClient(clientid);
            auth.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(LoginCompleted);
            auth.LoginAsync(new string[] { "wl.signin", "wl.basic", "wl.skydrive", "wl.skydrive_update", "wl.offline_access", "wl.photos" });
        }

        public void LoginCompleted(object sender, LiveConnectSessionChangedEventArgs e)
        {
            if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                App.Session = e.Session;
                if (dataFullName != null && dataPhoto != null) { GetProfileData(); }
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error logging in.");
            }
        }

        public void LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                App.Session = e.Session;
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error logging in.");
            }
        }

        public TextBlock dataFullName;
        public Image dataPhoto;

        public void GetProfileData()
        {
            LiveConnectClient clientGetMe = new LiveConnectClient(App.Session);
            clientGetMe.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(clientGetMe_GetCompleted);
            clientGetMe.GetAsync("me");

            LiveConnectClient clientGetPicture = new LiveConnectClient(App.Session);
            clientGetPicture.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(clientGetPicture_GetCompleted);
            clientGetPicture.GetAsync("me/picture");
        }

        void clientGetPicture_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var img = new System.Windows.Media.Imaging.BitmapImage();
                img.UriSource = new Uri((string)e.Result["location"]);
                
                dataPhoto.Source = img;
            }
        }

        void clientGetMe_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                dataFullName.Text = (string)e.Result["name"];
            }
        }

        public void uploadFile(string filename, System.IO.Stream file, string skydrivePath = "folder.559920a76be10760.559920A76BE10760!162")
        {
            LiveConnectClient client = new LiveConnectClient(App.Session);
            client.UploadAsync(skydrivePath, filename, file);
            client.UploadCompleted += new EventHandler<LiveOperationCompletedEventArgs>(UploadCompleted); 

        }

        void UploadCompleted(object sender, LiveOperationCompletedEventArgs args)
        {
            if (args.Error == null)
            {
                // File uploaded.
            }
            else
            {
                // There was an error uploading... should probably log this.
            }
        }

    }
}
