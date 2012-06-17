using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
using Newtonsoft.Json;


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

        public async Task<String> getNewUUID()
        {
            String url = _databaseurl + "_uuids";
            String retrievedJSON = await getRequest(url);
            Dictionary<string, string[]> json;
            string newUUID = "";

            try
            {
                json = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(retrievedJSON);
                newUUID = json["uuids"][0];
            }
            catch
            { return ""; }

            return newUUID;
        }

        private async Task<String> getRequest(String url)
        {
            String result = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = await request.GetResponseAsync();
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch{ return ""; }

            return result;
        }

    }
}
