using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


//content pipeline
//tinyurl.com/XNAEVIL
//tinyurl.com/xnaavatars
using Microsoft.Xna.Framework.Audio;


namespace DynamicSoundDemo
{
    public class SoundManager : INotifyPropertyChanged
    {
        private const int AudioBufferCount = 4;
        private const int AudioBufferSize = 2048;
        private const int ChannelCount = 1;
        private const int BytesPerSample = 2;
        private const double MaxWaveMagnitude = 32000d;
        private const int DefaultSampleRate = 22000;

        object DynamicSoundSync = new object ();


        private byte[][] _audioBufferList;
        private short[] _renderingBuffer;
        private DynamicSoundEffectInstance _dynamicSound;


        public SoundManager()
        {
            UpdateDeltaTime();
            _audioBufferList = new byte[AudioBufferCount][];
            for (int i = 0; i < _audioBufferList.Length; ++i)
                _audioBufferList[i] = new byte[ChannelCount * BytesPerSample * AudioBufferSize];
            _renderingBuffer = new short[ChannelCount*AudioBufferSize];
            SoundFunction = MySin;
            AudioListener x = new AudioListener();
           
            
        }

        double MySin(double time, int channel)
        {
            return Math.Sin(time*Math.PI*2.0d);
        }

        void UpdateDeltaTime()
        {
            DeltaTime = Frequency * (1.0d / SampleRate);
        }


        void FillBuffer()
        {
            if (SoundFunction == null)
                throw new NullReferenceException("SoundFunction");
            byte[] destinationBuffer = _audioBufferList[CurrentFillBufferIndex];
            if (++CurrentFillBufferIndex >= _audioBufferList.Length)
                CurrentFillBufferIndex = 0;
            short result;
            int currentBufferIndex = 0;
            int deltaBufferIndex = ChannelCount * BytesPerSample;

            for (int i = 0; i < destinationBuffer.Length / (ChannelCount * BytesPerSample); ++i)
            {
                int baseIndex = ChannelCount * BytesPerSample * i;
                //currentBufferIndex = 0;
                for (int c = 0; c < ChannelCount; ++c)
                {
                    result = (short)(MaxWaveMagnitude * SoundFunction(_Time, c));

                    #if(MANUAL_COPY)
                    destinationBuffer[baseIndex + currentBufferIndex] = (byte)(0xFF & result);
                    destinationBuffer[baseIndex + currentBufferIndex] = (byte)(0xFF & (result >> 0x8));
                    currentBufferIndex += deltaBufferIndex;
                    #else
                    _renderingBuffer[i * ChannelCount + c] = result;
                    #endif                    
                    
                }
                _Time += _deltaTime;
            }
            #if(!MANUAL_COPY)
            Buffer.BlockCopy(_renderingBuffer, 0, destinationBuffer, 0, _renderingBuffer.Length*sizeof(short));
            #endif
            OnPropertyChanged("Time");
            OnPropertyChanged("PendingBufferCount");
        }

        public void Play()
        {
            lock (DynamicSoundSync)
            {
                if (_dynamicSound == null)
                {
                    IsPlaying = true;
                    Time = 0;
                    CurrentFillBufferIndex = 0;
                    CurrentPlayBufferIndex = 0;
                    _dynamicSound = new DynamicSoundEffectInstance(SampleRate,
                                                                   (ChannelCount == 1)
                                                                       ? AudioChannels.Mono
                                                                       : AudioChannels.Stereo);
                    _dynamicSound.BufferNeeded += BufferNeeded;
                    FillBuffer();
                    BufferHitCount = 0;
                    SubmitBuffer();
                    _dynamicSound.Play();
                }
            }
        }

        public void Stop()
        {
            if (_dynamicSound != null)
            {
                lock (DynamicSoundSync)
                {
                    if (_dynamicSound != null)
                    {
                        IsPlaying = false;
                        _dynamicSound.Stop();
                        _dynamicSound = null;
                    }
                }
            }
        }

        void BufferNeeded(object sender, EventArgs args)
        {
            FillBuffer();
            SubmitBuffer();
            ++BufferHitCount;
        }

        void SubmitBuffer()
        {
            lock (DynamicSoundSync)
            {
                DynamicSoundEffectInstance currentSound = _dynamicSound;
                if (currentSound != null)
                {
                    currentSound.SubmitBuffer(_audioBufferList[CurrentPlayBufferIndex]);
                    if ((++CurrentPlayBufferIndex >= _audioBufferList.Length))
                        CurrentPlayBufferIndex = 0;
                }
            }
            OnPropertyChanged("PendingBufferCount");
        }



        // IsPlaying - generated from ObservableField snippet - Joel Ivory Johnson
        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            private set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged("IsPlaying");
                }
            }
        }
        //-----


        // CurrentFillBufferIndex - generated from ObservableField snippet - Joel Ivory Johnson
        private int _currentFillBufferIndex = 0;
        public int CurrentFillBufferIndex
        {
            get { return _currentFillBufferIndex; }
            private set
            {
                if (_currentFillBufferIndex != value)
                {
                    _currentFillBufferIndex = value;
                    OnPropertyChanged("CurrentFillBufferIndex");
                }
            }
        }
        //-----


        // CurrentPlayBufferIndex - generated from ObservableField snippet - Joel Ivory Johnson
        private int _currentPlayBufferIndex = 0;
        public int CurrentPlayBufferIndex
        {
            get { return _currentPlayBufferIndex; }
            private set
            {
                if (_currentPlayBufferIndex != value)
                {
                    _currentPlayBufferIndex = value;
                    OnPropertyChanged("CurrentPlayBufferIndex");
                }
            }
        }
        //-----
        // SampleRate - generated from ObservableField snippet - Joel Ivory Johnson
        private int _sampleRate = DefaultSampleRate;
        public int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (_sampleRate != value)
                {
                    _sampleRate = value;
                    OnPropertyChanged("SampleRate");
                }
            }
        }
        //-----

        // DeltaTime - generated from ObservableField snippet - Joel Ivory Johnson
        private double _deltaTime = 0;
        public double DeltaTime
        {
            get { return _deltaTime; }
            set
            {
                if (_deltaTime != value)
                {
                    _deltaTime = value;
                    OnPropertyChanged("DeltaTime");
                }
            }
        }
        //-----
        // Time - generated from ObservableField snippet - Joel Ivory Johnson
        private double _Time = 0;
        public double Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }
        //-----

        // SoundFunction - generated from ObservableField snippet - Joel Ivory Johnson
        private Func<double, int, double> _soundFunction;
        public Func<double, int, double> SoundFunction
        {
            get { return _soundFunction; }
            set
            {
                if (_soundFunction != value)
                {
                    _soundFunction = value;
                    OnPropertyChanged("SoundFunction");
                }
            }
        }
        //-----

        // Frequency - generated from ObservableField snippet - Joel Ivory Johnson
        private double _Frequency = 60;
        public double Frequency
        {
            get { return _Frequency; }
            set
            {
                if (_Frequency != value)
                {
                    _Frequency = value;
                    OnPropertyChanged("Frequency");
                    UpdateDeltaTime();
                }
            }
        }
        //-----

                
// BufferHitCount - generated from ObservableField snippet - Joel Ivory Johnson

  private int _bufferHitCount;
  public int BufferHitCount
  {
    get { return _bufferHitCount; }
      set
      {
          if (_bufferHitCount != value)
          {
              _bufferHitCount = value;
              OnPropertyChanged("BufferHitCount");
          }
      }
  }
 //-----

                
// PendingBufferCount - generated from ObservableField snippet - Joel Ivory Johnson


  public int PendingBufferCount
  {
    get
    {
        lock (DynamicSoundSync)
        {
            if (_dynamicSound != null)
                return _dynamicSound.PendingBufferCount;
        }
        return 0;
        
    }

  }
 //-----
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
