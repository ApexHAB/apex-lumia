using System;
using System.Net;
using System.Windows;


namespace ApexLumia
{
    public class Habitat
    {

        public Boolean status { get { return couch.status; } }

        private String _callsign;
        private CouchInterface couch;

        public Habitat(String dburl, String dbname, String callsign)
        {

            couch = new CouchInterface(dburl, dbname);

        }

        public void uploadTelemetry(String fullsentence)
        {

        }

        static private String base64ify(String thing)
        {
            byte[] thebytesofthing = System.Text.UTF8Encoding.UTF8.GetBytes(thing);
            string base64ified = System.Convert.ToBase64String(thebytesofthing);
            return base64ified;
        }

    }
}
