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
    public class CouchInterface
    {

        private bool _status;
        public bool status { get { return _status; } }

        private string _databaseurl;
        private string _databasename;

        public CouchInterface(string url, string name)
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
        private async Task<string> getNewUUID()
        {
            string url = _databaseurl + "_uuids";
            string retrievedJSON = await HTTPRequests.getRequest(url);
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

        public async void uploadDocument(string json, string id = "")
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) { _status = false; return; }

            if (id == "") { id = await getNewUUID(); }

            string url = _databaseurl + _databasename + "/" + id;

            try
            {
                string result = await HTTPRequests.putRequest(url, json);
                Dictionary<string, object> resultJSON = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                _status = (bool)resultJSON["ok"];
            }
            catch { _status = false; }

        }

        
   
    }
}
