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
using System.Collections.Generic;

namespace ApexLumia
{
    public class Sentence
    {

        // Compulsory Data
        public string callsign;
        public int sentence_id;
        public string latitude;
        public string longitude;
        public string altitude;
       
        // Optional Data goes in List
        public List<string> sentenceData;

        private string _wholeSentence = "";
        public string wholeSentence { get { return _wholeSentence; } }

        public Sentence(string _callsign, int _sentence_id, string _latitude, string _longitude, string _altitude){

            callsign = _callsign;
            sentence_id = _sentence_id;
            latitude = _latitude;
            longitude = _longitude;
            altitude = _altitude;

        }

        public bool compileSentence()
        {
            //CALLSIGN,SENTENCE_ID,TIME,LATITUDE,LONGITUDE,ALTITUDE,[CUSTOM, DATA, ..., ...]
            string _sentence;
            if (callsign.Length > 1) { _sentence = callsign.ToUpper(); } else { return false; }
            if (sentence_id >= 0) { _sentence += "," + sentence_id; } else { return false; }
            _sentence += "," + System.DateTime.Now.ToString("HH:mm:ss");
            _sentence += "," + latitude;
            _sentence += "," + longitude;
            _sentence += "," + altitude;
            foreach (string item in sentenceData)
            {
                _sentence += "," + item;
            }

            Crc16Ccitt crc = new Crc16Ccitt();
            string checksum = crc.ComputeChecksum(System.Text.UTF8Encoding.UTF8.GetBytes(_sentence)).ToString("X");

            // $$$$$$CALLSIGN,SENTENCE_ID,TIME,LATITUDE,LONGITUDE,ALTITUDE,[CUSTOM, DATA, ..., ...]*XXXX
            if (checksum.Length == 4) { _wholeSentence = "$$$$$$" + _sentence + "*" + checksum; } else { return false; }

            return true;

        }

        public void logSentence()
        {
            if (_wholeSentence.Length > 0)
            {
                // Log the sentence...
            }
        }

    }
}
