using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework.Audio;


namespace ApexLumia
{
    public class Sound
    {

        DynamicSoundEffectInstance _dynamicSound;

        int _sampleRate;
        double _frequency;
        double _timechange;
        const int _BufferLength = 4096;
        double[] _FloatBuffer = new double[_BufferLength];
        byte[] _ByteBuffer = new byte[_BufferLength * 2];
        double _Phase = 0.0;

        double _amplitude = 0.5;

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        public double amplitude
        {
            get { return _amplitude; }
            set { if (value <= 1.0d || value >= 0.0d) { _amplitude = value; } }
        }

        public Sound(double frequency = 1000, int samplerate = 22000)
        {
            _sampleRate = samplerate;
            _frequency = frequency;
            _timechange = _frequency * (1.0d / _sampleRate);
        }

        public void Start()
        {
            _dynamicSound = new DynamicSoundEffectInstance(_sampleRate, AudioChannels.Mono);
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

        void BufferNeeded(object sender, EventArgs e)
        {
            updateBuffer();
        }

        private void updateBuffer()
        {
            for (int i = 0; i < _BufferLength; i++)
            {
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

    }
}
