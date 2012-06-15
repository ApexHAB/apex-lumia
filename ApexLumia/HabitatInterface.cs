using System;
using System.Net;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;


namespace ApexLumia
{
    public class HabitatInterface
    {

        private Boolean _status;
        public Boolean status { get { return _status; } }

        private String _databaseurl;
        private String _databasename;

        public HabitatInterface(String url, String name)
        {

            if (url.Substring(url.Length - 1,1) != "/"){ url += "/"; }

            _databaseurl = url;
            _databasename = name;

            _status = true;
        }

        private async Task<string> getNewUUID()
        {
            //if (!NetworkInterface.GetIsNetworkAvailable()) { return ; }
            Uri url = new Uri(_databaseurl + "_uuids");

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
