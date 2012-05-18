
using System;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System.ComponentModel;
namespace CSharp___DllImport
{
    public static partial class Phone
    {
        //All are extern partial internal under map "Children"

        /// <summary>
        /// Hardware lock key pressed or system lock timeout
        /// </summary>
        /// <returns></returns>
        public static bool GetIsLocked()
        {
            var t = DllImportCaller.lib.VoidCall("aygshell", "SHIsLocked");
            return t == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">(000) 000-000-000 or any format +46 123-456-78 etc.</param>
        /// <returns>True if the call was made</returns>
        public static bool /*sucess*/ PhoneMakeCall(string number)
        {
            if (number == null) throw new ArgumentNullException("number");
            return DllImportCaller.lib.PhoneMakeCall7(number) == 0;
        }

        public static partial class Screen
        {
            public static byte[] GetCaptureBytes()
            {
                string res = "";
                var a = DllImportCaller.lib.Crnch(ref res);
                var ptr = new IntPtr(int.Parse(res));

                byte[] buffer = new byte[480 * 800 * 4];
                MarshalBypass.Copy(ptr, buffer, 0, buffer.Length);

                return buffer;
            }

            public static void init()
            {

            }

            //public static System.Windows.Controls.Image threadDisp = new System.Windows.Controls.Image();
            public static WriteableBitmap bmp = new System.Windows.Media.Imaging.WriteableBitmap(480, 800);
            //public static System.Windows.Media.Imaging.WriteableBitmap CaptureScreen()
            //{
            //   // threadDisp.Dispatcher.BeginInvoke(() => {  });
            //    var buffer = GetCaptureBytes();
               

            //    int x = 0, 
            //        y = 0, 
            //        i = 0,
            //        pixW = bmp.PixelWidth;

            //    while(x < 480)
            //    {
            //        while(y < 800)
            //        {
            //            var r = buffer[i++];
            //            var g = buffer[i++];
            //            var b = buffer[i++];
            //            //var alp = buffer[i++];
            //            i++;
            //            //if (alp != 0) //alp always 0
                        

            //            var pix = y * pixW + x;

            //            bmp.Pixels[pix] = /*alp << 24 *//* 0 | */r << 16 | g << 8 | b;
            //            y++;
            //        }
            //        y = 0;
            //        x++;
            //    }
            //    return bmp;
            //}
            public static System.Windows.Media.Imaging.WriteableBitmap CaptureScreen()
            {
                IntPtr handle;
                IntPtr bits;
                int size;

                DllImportCaller.lib.CaptureScreen(out handle, out size, out bits);
                byte[] buffer = new byte[size];
                MarshalBypass.Copy(bits, buffer, 0, size);
                DllImportCaller.lib.DeleteObject(handle);

                int bufferPos = 0;
                for (int i = 0; i < 384000; i++)
                {
                    // colors stored as BGR
                    int pixel = buffer[bufferPos++];
                    pixel |= buffer[bufferPos++] << 8;
                    pixel |= buffer[bufferPos++] << 16;
                    bmp.Pixels[i] = pixel;
                }

                return bmp;
            }
            public static System.Windows.Controls.Image CaptureScreenAsImage()
            {
                var bmp = CaptureScreen();
                
                var img = new System.Windows.Controls.Image();
                img.Source = bmp;

                return img;
            }

            /// <param name="quality">0-100 jpg quality</param>
            /// <returns>Captured picture name</returns>
            public static Capture CaptureScreenToPictures(int quality = 100)
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var bmp = CaptureScreen();

                string name;
                long size;
                Picture pic;

                using (var ms = new MemoryStream(480 * 800))
                {
                    System.Windows.Media.Imaging.Extensions.SaveJpeg(bmp, ms, 480, 800, 0, quality);
                    size = ms.Position;
                    ms.Seek(0, SeekOrigin.Begin);

                    name = string.Format("ScreenDump_{0}", DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss:ffff tt"));
                    pic = new Microsoft.Xna.Framework.Media.MediaLibrary().SavePicture(name, ms);
                }
                sw.Stop();

                return new Capture(name, sw.Elapsed, size);
            }

            public class CaptureData
            {
                public Picture Picture;
                public string PictureName;
                public long Size;
            }

            public static void AsyncCaptureScreenToPictures(CaptureEventCallback callback, int quality = 100)
            {
                var bw = new BackgroundWorker();
                bw.DoWork += (o, e) =>
                {
                    var objA = (object[])e.Argument;
                    var capture = CaptureScreenToPictures((int)objA[0]);

                    var callB = objA[1] as CaptureEventCallback;
                    if (callB != null)
                    {
                        callB(capture);
                    }
                };

                bw.RunWorkerAsync(new object[] { quality, callback });

            }
            private delegate void callQCall(int q, CaptureEventCallback call); 
            public delegate void CaptureEventCallback(Capture args);

            public class Capture : EventArgs
            {
                public string PictureName { get; private set; }
                public TimeSpan TimeElapsed { get; private set; }
                public long PictureByteSize { get; private set; }

                public Capture(string picName, TimeSpan elapsed, long pictureByteSize)
                {
                    this.PictureName = picName;
                    this.TimeElapsed = elapsed;
                    this.PictureByteSize = pictureByteSize;
                }
                public override string ToString()
                {
                    return string.Format("{{ PictureName = {0}, TimeElapsed = {1}ms, PictureByteSize = {2}}}", PictureName, TimeElapsed.TotalMilliseconds, PictureByteSize);
                }
            }
        }
    }
    public static class bitmapextensions
    {
        public static void setPixel(this System.Windows.Media.Imaging.WriteableBitmap wbm, int x, int y, System.Windows.Media.Color c)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1)
                return;

            if (y < 0 || x < 0)
                return;

            wbm.Pixels[y * wbm.PixelWidth + x] = c.A << 24 | c.R << 16 | c.G << 8 | c.B;
        }

        public static System.Windows.Media.Color getPixel(this System.Windows.Media.Imaging.WriteableBitmap wbm, int x, int y)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1)
                return System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

            if (y < 0 || x < 0)
                return System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

            byte[] ARGB = BitConverter.GetBytes(wbm.Pixels[y * wbm.PixelWidth + x]);

            return System.Windows.Media.Color.FromArgb(ARGB[3], ARGB[2], ARGB[1], ARGB[0]);
        }
    }
}
