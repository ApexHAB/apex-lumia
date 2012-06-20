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

        /// <summary>
        /// Async: Get a new, randomly generated UUID from the CouchDB itself for use in new documents.
        /// </summary>
        /// <returns></returns>
        public async Task<String> getNewUUID()
        {
            String url = _databaseurl + "_uuids";
            String retrievedJSON = await HTTPRequests.getRequest(url);
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



        // Temporary
        public async void writeSomethingRandom(String data){

            string uuid = await getNewUUID();
            if (uuid == "") { _status = false;  return; }
            

            string url = _databaseurl + _databasename + "/" + uuid;

            String result = await HTTPRequests.putRequest(url, data);
            Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<String, object>>(result);
            _status = (Boolean)json["ok"];

        }




    }
}
