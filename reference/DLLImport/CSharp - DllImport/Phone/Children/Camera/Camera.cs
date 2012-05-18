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

namespace CSharp___DllImport
{
    //http://dotnet.dzone.com/articles/not-your-regular-photo-and?mz=27249-windowsphone7
    
    public static partial class Phone
    {
        //public static class Camera
        //{
        //    static Camera()
        //    {
        //        var res = DllImportCaller.lib.VoidCall("PhotoEntLib", "CapMan_InitializeApi");
        //        Microsoft.Phone.Shell.PhoneApplicationService.Current.Closing += (o, e) => 
        //        {
        //            DllImportCaller.lib.IntCall("PhotoEntLib", "CapMan_Disconnect", 0);
        //        };
        //    }

        //    //http://madhukaudantha.blogspot.com/2010/09/capturing-photo-by-windows-7-phone.html
        //    //[Obsolete("Not working (yet).", true)]
        //    public static bool FlashLighOn
        //    {
                
        //        set
        //        {
        //            if (value)
        //            {
        //                DllImportCaller.lib.IntDualCall("PhotoEntLib", " CapMan_SetVideoLampEnabled", 0, 1);
        //            }
        //            else
        //            {
        //                DllImportCaller.lib.IntDualCall("PhotoEntLib", " CapMan_SetVideoLampEnabled", 0, 0);
        //            }
        //        }
        //    }
        //}
    }
}
