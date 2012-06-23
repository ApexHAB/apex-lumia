using System;
using System.Net;
using System.Windows;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace ApexLumia
{
    public class Habitat
    {

        public bool status { get { return couch.status; } }

        private string _callsign;
        private CouchInterface couch;

        /// <summary>
        /// Constructor: Get stuff ready for interacting with Habitat.
        /// </summary>
        /// <param name="dburl">The URL of CouchDB</param>
        /// <param name="dbname">The habitat database name</param>
        /// <param name="callsign">The callsign of the phone</param>
        public Habitat(string dburl, string dbname, string callsign)
        {

            couch = new CouchInterface(dburl, dbname);
            _callsign = callsign;

        }

        /// <summary>
        /// Upload a new telemetry_doc to Habitat.
        /// </summary>
        /// <param name="fullsentence">The full, UKHAS style sentence ($$BLA,,,,,*CHECKSUM)</param>
        public void uploadTelemetry(string fullsentence)
        {

            // This should really really really take into account the fact that
            // if it already exists, we just want to add our receiver details.

            // BUT, I can't be bothered since we're always going to be the first
            // ones to upload it since it won't have been transmitted yet

            // PLUS, if we're not the first person, then we don't really care
            // about ensuring the system knows we 'received' it too.

            string type = "payload_telemetry";
            string _raw = base64ify(fullsentence);
            string id = sha256ify(_raw);
            int timecreated = unix_timestamp();
            int timeuploaded = unix_timestamp();

            var telemetry_doc = new Dictionary<string, object>(){
                {"type", type},
                {"data", 
                    new Dictionary<string, object>(){
                        {"_raw", _raw}
                    }
                },
                {"receivers",
                    new Dictionary<string, object>(){
                        {_callsign,
                            new Dictionary<string, object>(){
                                {"time_created", timecreated},
                                {"time_uploaded", timeuploaded}
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(telemetry_doc);
            couch.uploadDocument(json, id);
        }

        /// <summary>
        /// Encode a string into a Base64 string version.
        /// </summary>
        /// <param name="thing">The string to encode into a Base64 version.</param>
        /// <returns></returns>
        static private string base64ify(string thing)
        {
            byte[] thebytesofthing = UTF8Encoding.UTF8.GetBytes(thing);
            string base64ified = Convert.ToBase64String(thebytesofthing);
            return base64ified;
        }

        /// <summary>
        /// Hash a string using SHA256
        /// </summary>
        /// <param name="thing">The string to hash.</param>
        /// <returns></returns>
        static private string sha256ify(string thing)
        {
            byte[] thebytesofthing = UTF8Encoding.UTF8.GetBytes(thing);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] sha256ifiedbytes = algorithm.ComputeHash(thebytesofthing);

            string hex = "";
            foreach(byte x in sha256ifiedbytes){ hex += String.Format("{0:x2}", x); }

            return hex;
            
        }

        /// <summary>
        /// Guess what: it gives you a UNIX timestamp.
        /// </summary>
        /// <returns></returns>
        public int unix_timestamp()
        {
            TimeSpan unix_time = (System.DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (int)unix_time.TotalSeconds;
        }


    }
}
