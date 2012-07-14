using System;
using System.Net;
using System.Windows;
using Microsoft.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;

namespace ApexLumia
{
    public class Camera
    {

        PhotoCamera cam;

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }



        public Camera()
        {
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true)
            {
                // There is a camera. Which I already know. But useful if I decide to use this in any other apps.
                cam = new Microsoft.Devices.PhotoCamera(CameraType.Primary);
                //cam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(camInitialized);
                cam.CaptureCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camCaptureCompleted);
                //cam.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(camCaptureImageAvailable);
                //cam.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(camCaptureThumbnailAvailable);
                
            }
        }

        void cam_Initialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded) { _isRunning = true; }
        }

        public bool takePhoto()
        {
            if (cam == null) { return false; }
            try
            {
                cam.CaptureImage();
            }
            catch
            {
                // Probably still taking a previous photo.
                return false;
            }

            return true;
        }

        void camCaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {

        }


    }
}
