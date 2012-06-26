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
            string _raw = Utils.base64ify(fullsentence);
            string id = Utils.sha256ify(_raw);
            int timecreated = Utils.unix_timestamp();
            int timeuploaded = Utils.unix_timestamp();

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




    }
}
