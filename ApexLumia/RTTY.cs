using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace ApexLumia
{
    public class RTTY
    {

        #region "Properties"
        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        DynamicSoundEffectInstance _dynamicSound;
        int _sampleRate;
        double _frequency;
        double _timechange;
        int _BufferLength;
        int _BitLength;
        double[] _FloatBuffer; 
        byte[] _ByteBuffer;
        double _Phase = 0.0;
        double _amplitude = 0.5;
        
        int _baudrate;
        int _shift;
        double lowVolume;
        double highVolume;
        int _stopBits;

        private List<bool> _nextTransmission = new List<bool>();
        private List<bool> _currentTransmission = new List<bool>();
        private int x;
        #endregion

        /// <summary>
        /// Constructor: Setup RTTY and Sound settings.
        /// </summary>
        /// <param name="frequency">The frequency of the sine wave.</param>
        /// <param name="samplerate">The sample rate of the sound.</param>
        /// <param name="baud">The baud rate of the data transmission.</param>
        /// <param name="shift">The eventual radio frequency shift in Hz. (Not used)</param>
        /// <param name="low">The amplitude for a 'low' bit. 0.0->1.0</param>
        /// <param name="high">The amplitude for a 'high' bit. 0.0->1.0</param>
        /// <param name="stopbits">The number of stop bits for each byte.</param>
        public RTTY(double frequency = 3000, int samplerate = 42000, int baud = 300, int shift = 425, double low = 0.1, double high = 1.0, int stopbits = 2)
        {
            _sampleRate = samplerate;
            _frequency = frequency;
            _timechange = _frequency * (1.0d / _sampleRate);
            _baudrate = baud;
            _shift = shift;
            _stopBits = stopbits;
            lowVolume = low;
            highVolume = high;

                     
            _BufferLength = (int)((1d/(double)_baudrate) * 11d * (double)_sampleRate * 2d);
            _BitLength = _BufferLength / 11 /2;
            System.Diagnostics.Debug.WriteLine(_BitLength);
            _FloatBuffer = new double[_BufferLength];
            _ByteBuffer = new byte[_BufferLength * 2];

            _dynamicSound = new DynamicSoundEffectInstance(_sampleRate, AudioChannels.Mono);
            _dynamicSound.BufferNeeded += BufferNeeded;
            _dynamicSound.Volume = 1.0f;
            SoundEffect.MasterVolume = 1.0f;

        }

        /// <summary>
        /// Start the sine wave playing.
        /// </summary>
        public void Start()
        {
            updateBuffer("");
            updateBuffer("");
            updateBuffer("");
            _dynamicSound.Play();
            _isRunning = true;
        }

        /// <summary>
        /// Stop the sine wave from playing.
        /// </summary>
        public void Stop()
        {
            _dynamicSound.Stop();
            _isRunning = false;
        }


        /// <summary>
        /// Gets a sentence ready to be transmitted next. If this is called again whilst a sentence is already queued, the queued sentence is overwritten with the new one.
        /// </summary>
        /// <param name="sentence">The string to be transmitted.</param>
        /// <returns>Whether it was successfully queued.</returns>
        public bool transmitSentence(string sentence)
        {
            if (_isRunning == false) { return false; }

            sentence = sentence + "\n";
            _nextTransmission = convertToBits(sentence);

            return true;
           
        }

        /// <summary>
        /// Event Handler: Called when a new buffer is needed. Calls updateBuffer() on a new thread.
        /// </summary>
        void BufferNeeded(object sender, EventArgs e)
        {
            // Update the buffer in a different thread
            ThreadPool.QueueUserWorkItem(updateBuffer);
        }

        /// <summary>
        /// Fills the next buffer with a sine wave, amplitude modulated with the data to be transmitted.
        /// </summary>
        private void updateBuffer(object o)
        {

            for (int i = 0; i < _BufferLength; i++)
            {
                if (x >= _BitLength)
                {
                    
                    if (_currentTransmission.Count != 0)
                    {
                        x = 0;
                        _currentTransmission.RemoveAt(0);
                    }
                }
                else { x++; }

                if (_currentTransmission.Count != 0)
                {
                    // We are in the middle of a transmission: there is data to transmit
                    if (_currentTransmission[0]) { _amplitude = highVolume; } else { _amplitude = lowVolume; }
                }
                else if (_nextTransmission.Count != 0)
                {
                    // There is no current transmission, but there is data waiting to be transmitted.
                    _currentTransmission = new List<bool>(_nextTransmission);
                    _nextTransmission.Clear();
                    _amplitude = lowVolume;
                    x = 0;

                }else{
                    // There is no data to transmit at all. Shame. Real Shame.
                    _amplitude = lowVolume;
                }


                _FloatBuffer[i] = _amplitude * Math.Sin(Math.PI * _Phase * 2.0d);
                //System.Diagnostics.Debug.WriteLine(_FloatBuffer[i]);
                _Phase += _timechange;
                
            }
            for (int i = 0; i < _BufferLength; i++)
            {
                short samp = (short)(_FloatBuffer[i] * short.MaxValue);

                _ByteBuffer[i * 2 + 0] = (byte)(samp & 0xFF);
                _ByteBuffer[i * 2 + 1] = (byte)(samp >> 8);
            }
            _dynamicSound.SubmitBuffer(_ByteBuffer);
        }

        /// <summary>
        /// Convert a string to a list of bits (booleans) incl. start bits and stop bits.
        /// </summary>
        /// <param name="toConvert">The string to convert.</param>
        /// <returns></returns>
        private List<bool> convertToBits(string toConvert)
        {

            byte[] theBytes = UTF8Encoding.UTF8.GetBytes(toConvert);
            var result = new List<bool>();
            result.Add(true);
            result.Add(true);
            result.Add(true);

            foreach (byte b in theBytes)
            {
                byte c = b;

                // Start bit
                result.Add(false);
                System.Diagnostics.Debug.WriteLine("Start bit 0");

                // Byte bits
                for (int i = 0; i < 8; i++)
                {
                    if ((c & 1) == 1) { result.Add(true); System.Diagnostics.Debug.WriteLine("bit 1"); } else { result.Add(false); System.Diagnostics.Debug.WriteLine("bit 0"); }
                    c = (byte)(c >> 1);
                }

                // Stop bits
                for (int i = 0; i < _stopBits; i++)
                {
                    result.Add(true);
                    System.Diagnostics.Debug.WriteLine("Stop bit 1");
                }
            }
            /*
            foreach (bool bit in result)
            {
                if (bit)
                {
                    System.Diagnostics.Debug.WriteLine("1");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("0");
                }
            }*/

            return result;
        }



    }
}
