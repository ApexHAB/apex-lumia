using System;
using System.Net;
using System.Windows;
using System.Text;


namespace ApexLumia
{
    public class Habitat
    {

        public Boolean status { get { return couch.status; } }

        private string _callsign;
        private CouchInterface couch;

        public Habitat(string dburl, string dbname, string callsign)
        {

            couch = new CouchInterface(dburl, dbname);

        }

        public void uploadTelemetry(string fullsentence)
        {

        }

        static private string base64ify(string thing)
        {
            byte[] thebytesofthing = UTF8Encoding.UTF8.GetBytes(thing);
            string base64ified = Convert.ToBase64String(thebytesofthing);
            return base64ified;
        }

    }
}
