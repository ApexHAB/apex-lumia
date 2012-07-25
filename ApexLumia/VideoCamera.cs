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
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using Microsoft.Devices;
using Microsoft.Phone.Controls;


namespace ApexLumia
{
    public class VideoCamera
    {

        // Viewfinder for capturing video.
        private VideoBrush videoRecorderBrush;

        // Source and device for capturing video.
        private CaptureSource captureSource;
        private VideoCaptureDevice videoCaptureDevice;

        // File details for storing the recording.        
        private IsolatedStorageFileStream isoVideoFile;
        private FileSink fileSink;
        private int isoVideoFileName = 0;

        void VideoCamera(VideoBrush video, int videocount)
        {
            videoRecorderBrush = video;
            isoVideoFileName = videocount;
        }

        public void start()
        {
            if (captureSource == null)
            {
                // Create the VideoRecorder objects.
                captureSource = new CaptureSource();
                fileSink = new FileSink();

                videoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();

                // Add eventhandlers for captureSource.
                captureSource.CaptureFailed += new EventHandler<ExceptionRoutedEventArgs>(OnCaptureFailed);

                // Initialize the camera if it exists on the device.
                if (videoCaptureDevice != null)
                {
                    // Create the VideoBrush for the viewfinder.
                    videoRecorderBrush = new VideoBrush();
                    videoRecorderBrush.SetSource(captureSource);

                    // Start video capture and display it on the viewfinder.
                    captureSource.Start();

                }

            }
        }

        public void stop()
        {

        }

        public void timedRecording(int seconds)
        {

        }

    }
}
