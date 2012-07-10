using System;
using System.Net;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ApexLumia
{
    public class RTTY
    {

        Sound sound;
        BackgroundWorker tx = new BackgroundWorker();

        bool _isReady = true;
        public bool isReady { get { return _isReady; } }

        int _baudrate;

        int _shift;
        double lowVolume;
        double highVolume;

        int _stopBits;
        string _preamble;

        /// <summary>
        /// Constructor: Settings for transmitting RTTY.
        /// </summary>
        /// <param name="baud">The baud rate of the RTTY transmission.</param>
        /// <param name="shift">The shift for the transmission (Hz)</param>
        /// <param name="stopbits">The number of stop bits for each byte.</param>
        /// <param name="preamble">The preamble.</param>
        public RTTY(int baud = 300, int shift = 425, int stopbits = 2, string preamble = "UUUUUUUUU")
        {
            _baudrate = baud;
            _shift = shift;
            _stopBits = stopbits;
            _preamble = preamble;

            shiftToVolumes();

            sound = new Sound(1000.0, 22000);

            tx.WorkerSupportsCancellation = true;
            tx.WorkerReportsProgress = false;

        }

        public void Start()
        {
            sound.Start();
            _isReady = true;
        }

        public void Stop()
        {
            sound.Stop();
            _isReady = false;
        }

        public void transmitPreamble()
        {
            string preamble = _preamble + "\n";
            List<int> bits = convertToBits(preamble);

        }

        public void transmitSentence(string sentence)
        {
            sentence = sentence + "\n";
            List<int> bits = convertToBits(sentence);



        }

        private void tx_DoWork(object sender, DoWorkEventArgs e)
        {
            _isReady = false;
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending == true)
            {
                // Should probably do something here... like stop transmitting stuff...
                e.Cancel = true;
                return;
            }

            // Then here, we'll do work! Like loop through all the bits using the correct timing and transmit them!!! Yay!!!

        }

        private void tx_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                // It hasnt actually completed, it was cancelled... but it sends us here anyway. But since it was cancelled we don't want to do anything.
                _isReady = true;
                return;
            }

            if (e.Error != null)
            {
                // There was an error... eeeek... oh well, not going to bother doing anything about it... may log it later on.
            }

            // It's finished transmitting those bits it was meant to be transmitting... now we can notify stuff that we're ready to transmit new stuff.
            _isReady = true;
        }



        /// <summary>
        /// Convert a string to a list of bits incl. start bits and stop bits.
        /// </summary>
        /// <param name="toConvert">The string to convert.</param>
        /// <returns></returns>
        private List<int> convertToBits(string toConvert)
        {

            byte[] theBytes = UTF8Encoding.UTF8.GetBytes(toConvert);
            var result = new List<int>();
            
            foreach (byte b in theBytes)
            {
                byte c = b;

                // Start bit
                result.Add(0); 

                // Byte bits
                for (int i = 0; i < 8; i++)
                {
                    if ((c & 1) == 1) { result.Add(1); } else { result.Add(0); }
                    c = (byte)(c >> 1);
                }

                // Stop bits
                for (int i = 0; i < _stopBits; i++)
                {
                    result.Add(1);
                }
            }
            return result;
        }

        private void shiftToVolumes(){

            // Need the NTX2 graph to properly do this...
            // May do it eventually.

            if (_shift == 425)
            {
                lowVolume = 0.5;
                highVolume = 0.7;
            }
        }

    }
}
