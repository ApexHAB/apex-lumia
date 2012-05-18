using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace DynamicSoundDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SoundManager _soundManager;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _soundManager = new SoundManager();
            DataContext = _soundManager;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (!_soundManager.IsPlaying)
            {
                _soundManager.Play();
                
            }
            else
            {
                _soundManager.Stop();
                
            }

        }
    }
}