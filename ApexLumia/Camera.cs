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
using System.Windows.Media;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Maps;

namespace ApexLumia
{
    public class Camera
    {

        PhotoCamera cam;
        MediaLibrary library = new MediaLibrary();

        bool _isRunning = false;
        public bool isRunning { get { return _isRunning; } }

        int photoname;

        VideoBrush videobrush;

        public List<string> takenPhotos = new List<string>();

        public Camera(VideoBrush video, int photocount)
        {
            

            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true)
            {
                // There is a camera. Which I already know. But useful if I decide to use this in any other apps.

                cam = new Microsoft.Devices.PhotoCamera(CameraType.Primary);
                photoname = photocount;
                videobrush = video;

            }
        }

        public void start()
        {
            cam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(camInitialized);
            cam.CaptureCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camCaptureCompleted);
            cam.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(camCaptureImageAvailable);
            cam.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(camCaptureThumbnailAvailable);
            cam.AutoFocusCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camAutoFocusCompleted);
            videobrush.SetSource(cam);
            videobrush.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };
            _isRunning = true;
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

        void camInitialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Initialized");
            cam.FlashMode = FlashMode.Off;
            setToLargestRes();        
        }

        private void setToLargestRes()
        {
            IEnumerable<Size> resList = cam.AvailableResolutions;
            int resCount = resList.Count<Size>();
            cam.Resolution = resList.ElementAt<Size>(resCount - 1);
        }

        public bool takePhoto()
        {
            System.Diagnostics.Debug.WriteLine("takePhoto");
            if (cam == null) { return false; }
            try
            {
                cam.FocusAtPoint(0.5,0.5);
            }
            catch
            {
                // Probably still taking a previous photo.
                System.Diagnostics.Debug.WriteLine("fail");
                return false;
            }

            return true;
        }

        void camAutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            try
            {
                cam.CaptureImage();
                System.Diagnostics.Debug.WriteLine("CaptureImage()");
            }
            catch
            {
                
            }
        }

        void camCaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            photoname++;
            System.Diagnostics.Debug.WriteLine("CaptureComplete");
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
                System.Diagnostics.Debug.WriteLine("Saved");

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

                        takenPhotos.Add(targetStream.Name);
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
