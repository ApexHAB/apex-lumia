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
        private Rectangle viewfinderRectangle;

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        bool _isRecording = false;
        public bool isRecording { get { return _isRecording; } }



        public VideoCamera(VideoBrush video, Rectangle viewfinder, int videocount)
        {
            videoRecorderBrush = video;
            isoVideoFileName = videocount;
            viewfinderRectangle = viewfinder;
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
                    videoRecorderBrush.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };
                    viewfinderRectangle.Fill = videoRecorderBrush;
                    
                    // Start video capture and display it on the viewfinder.
                    captureSource.Start();
                    System.Diagnostics.Debug.WriteLine("Started");
                    _isRunning = true;
                }
                
                
            }
        }

        public void stop()
        {
            if (captureSource != null)
            {
                // Stop captureSource if it is running.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();
                }

                // Remove the event handler for captureSource.
                captureSource.CaptureFailed -= OnCaptureFailed;

                // Remove the video recording objects.
                captureSource = null;
                videoCaptureDevice = null;
                fileSink = null;
                videoRecorderBrush = null;
                System.Diagnostics.Debug.WriteLine("Stopped");
                _isRunning = false;

            }
        }

        public void startRecording()
        {
            try
            {
                // Connect fileSink to captureSource.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();
                    fileSink.CaptureSource = captureSource;
                    fileSink.IsolatedStorageFileName = isoVideoFileName.ToString() + ".mp4";
                    System.Diagnostics.Debug.WriteLine(isoVideoFileName.ToString() + ".mp4");
                }

                // Begin recording.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Stopped)
                {
                    captureSource.Start();
                    _isRecording = true;
                    System.Diagnostics.Debug.WriteLine("Recording");
                }

            }
            catch
            { return; }
        }

        public void stopRecording()
        {
            try
            {
                // Stop recording.
                if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();

                    // Disconnect fileSink.
                    fileSink.CaptureSource = null;
                    fileSink.IsolatedStorageFileName = null;

                    _isRecording = false;
                    System.Diagnostics.Debug.WriteLine("Stop Recording");
                    // Display the video on the viewfinder.
                    if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Stopped)
                    {
                            // Add captureSource to videoBrush.
                            videoRecorderBrush.SetSource(captureSource);

                            captureSource.Start();


                    }
                }
            }
            catch
            { return; }

            isoVideoFileName++;

        }

        private void OnCaptureFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // Recording failed :/
            // In a good app, it would probably show an error message and log the error etc.
            System.Diagnostics.Debug.WriteLine("Failed");
        }

    }
}
