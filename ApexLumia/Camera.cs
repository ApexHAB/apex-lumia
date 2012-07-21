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
        MediaLibrary library = new MediaLibrary();

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        string photoname;

        public Camera()
        {
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true)
            {
                // There is a camera. Which I already know. But useful if I decide to use this in any other apps.
                cam = new Microsoft.Devices.PhotoCamera(CameraType.Primary);

                
            }
        }

        public void start()
        {
            cam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(camInitialized);
            cam.CaptureCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camCaptureCompleted);
            cam.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(camCaptureImageAvailable);
            cam.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(camCaptureThumbnailAvailable);
        }

        public void stop()
        {
            if (cam != null)
            {
                // Dispose camera to minimize power consumption and to expedite shutdown.
                cam.Dispose();

                // Release memory, ensure garbage collection.
                cam.Initialized -= camInitialized;
                cam.CaptureCompleted -= camCaptureCompleted;
                cam.CaptureImageAvailable -= camCaptureImageAvailable;
                cam.CaptureThumbnailAvailable -= camCaptureThumbnailAvailable;

                _isRunning = false;

            }
        }

        void camInitialized(object sender, CameraOperationCompletedEventArgs e)
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
            photoname = Utils.uniqueAlphanumericString();
        }

        void camCaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {

            string photo = photoname + ".jpg";

            try
            {
                // Add to phone's media library camera roll.
                library.SavePictureToCameraRoll(photo, e.ImageStream);
                e.ImageStream.Seek(0, SeekOrigin.Begin);

                // Save as JPEG to IsolatedStorage
                using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream targetStream = isStore.OpenFile(photo, FileMode.Create, FileAccess.Write))
                    {
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }

            }
            finally
            {
                e.ImageStream.Close();
            }

        }

        void camCaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            string thumbnail = photoname + "_th.jpg";

            try
            {
                using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream targetStream = isStore.OpenFile(thumbnail, FileMode.Create, FileAccess.Write))
                    {
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }
            }
            finally
            {
                e.ImageStream.Close();
            }

        }


    }
}
