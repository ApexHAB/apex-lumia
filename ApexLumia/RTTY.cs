using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ApexLumia
{
    public class RTTY
    {

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

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        bool _isReady = true;
        public bool isReady { get { return _isReady; } }

        int _baudrate;

        int _shift;
        double lowVolume;
        double highVolume;

        int _stopBits;
        string _preamble;

        public RTTY(double frequency = 1000, int samplerate = 42000, int baud = 300, int shift = 425, int stopbits = 2, string preamble = "UUUUUUUUU")
        {
            _sampleRate = samplerate;
            _frequency = frequency;
            _timechange = _frequency * (1.0d / _sampleRate);
            _baudrate = baud;
            _shift = shift;
            _stopBits = stopbits;
            _preamble = preamble;

            shiftToVolumes();

            _dynamicSound = new DynamicSoundEffectInstance(_sampleRate, AudioChannels.Mono);

            _BufferLength = (int)((1/_baudrate) * 11 * _sampleRate * 2);
            _BitLength = _BufferLength / 11;
            _FloatBuffer = new double[_BufferLength];
            _ByteBuffer = new byte[_BufferLength * 2];

        }

        public void Start()
        {
            
            _dynamicSound.BufferNeeded += BufferNeeded;
            updateBuffer();
            updateBuffer();
            updateBuffer();
            _dynamicSound.Play();
            _isRunning = true;
        }

        public void Stop()
        {
            _dynamicSound.Stop();
            _isRunning = false;
        }



        public void transmitSentence(string sentence)
        {
            if (_nextTransmission.Count != 0) { return; }

            sentence = sentence + "\n";
            _nextTransmission = convertToBits(sentence);

        }

        void BufferNeeded(object sender, EventArgs e)
        {
            updateBuffer();
        }

        private List<bool> _nextTransmission;
        private List<bool> _currentTransmission;
        private int x;

        private void updateBuffer()
        {
            for (int i = 0; i < _BufferLength; i++)
            {
                if (x >= _BitLength && _currentTransmission.Count != 0)
                {
                    x = 0;
                    _currentTransmission.RemoveAt(0);
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
                    _currentTransmission = _nextTransmission;
                    _nextTransmission.Clear();
                    _amplitude = lowVolume;

                }else{
                    // There is no data to transmit at all. Shame. Real Shame.
                    _amplitude = lowVolume;
                }

                _FloatBuffer[i] = _amplitude * Math.Sin(Math.PI * _Phase * 2.0d);
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
        /// Convert a string to a list of bits incl. start bits and stop bits.
        /// </summary>
        /// <param name="toConvert">The string to convert.</param>
        /// <returns></returns>
        private List<bool> convertToBits(string toConvert)
        {

            byte[] theBytes = UTF8Encoding.UTF8.GetBytes(toConvert);
            var result = new List<bool>();
            
            foreach (byte b in theBytes)
            {
                byte c = b;

                // Start bit
                result.Add(false); 

                // Byte bits
                for (int i = 0; i < 8; i++)
                {
                    if ((c & 1) == 1) { result.Add(true); } else { result.Add(false); }
                    c = (byte)(c >> 1);
                }

                // Stop bits
                for (int i = 0; i < _stopBits; i++)
                {
                    result.Add(true);
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
                _amplitude = lowVolume;
                highVolume = 0.7;
            }
        }


    }
}
