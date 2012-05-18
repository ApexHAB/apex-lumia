
using System;
namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static class Clipboard
        {
            public enum ClipboardFormats : int
            {
                Text = 1,
                Bitmap = 2,
                SymbolicLink = 4,
                Dif = 5,
                Tiff = 6,
                OemText = 7,
                Dib = 8,
                Palette = 9,
                PenData = 10,
                Riff = 11,
                WaveAudio = 12,
                UnicodeText = 13
            }

            private static int GetOpenClipboardWindow()
            {
                return DllImportCaller.lib.VoidCall("coredll", "GetOpenClipboardWindow");
            }

            private static bool OpenClipboard(int window)
            {
                return DllImportCaller.lib.IntCall("coredll", "OpenClipboard", window) == 1;
            }

            private static bool EmptyClipboard()
            {
                return DllImportCaller.lib.VoidCall("coredll", "EmptyClipboard") == 1;
            }

            private static bool CloseClipboard()
            {
                return DllImportCaller.lib.VoidCall("coredll", "CloseClipboard") == 1;
            }

            private static int SetClipboardData(ClipboardFormats type, IntPtr h)
            {
                return DllImportCaller.lib.IntDualCall("coredll", "SetClipboardData", (int)type, h.ToInt32());
            }

            /// <summary>
            /// Seems like all "CLIPFORMAT" return false
            /// </summary>
            /// <param name="format"></param>
            /// <returns></returns>
            public static bool IsClipboardFormatAvailable(CLIPFORMAT format)
            {
                int _format = (int)format;
                return DllImportCaller.lib.IntCall("coredll", "IsClipboardFormatAvailable", _format) != 0;
            }

            /*
            var ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_TEXT);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_BITMAP);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_METAFILEPICT);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_SYLK);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_DIF);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_TIFF);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_OEMTEXT);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_DIB);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_PALETTE);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_PENDATA);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_RIFF);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_WAVE);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_UNICODETEXT);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_ENHMETAFILE);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_HDROP);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_LOCALE);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_MAX);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_OWNERDISPLAY);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_DSPTEXT);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_DSPBITMAP);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_DSPMETAFILEPICT);
            ea = Phone.Clipboard.IsClipboardFormatAvailable(Phone.Clipboard.CLIPFORMAT.CF_DSPENHMETAFILE);
             */

            //[DllImport("user32.dll", CharSet = CharSet.Auto)]
            //public static extern bool SetClipboardData(int type, IntPtr h);

            /// <summary>
            /// String value to set as clipboard text, null to clear clipboard.
            /// </summary>
            [Obsolete("fails", true)]
            public static string Value
            {
                get
                {
                    string a = "111111111111111111111";
                    DllImportCaller.lib.Clipboard_GET(ref a);
                    
                    return a;
                }
                set
                {

                    var t = DllImportCaller.lib.Clipboard_SET("helo");
                    //var w = GetOpenClipboardWindow();
                    //var o = OpenClipboard(w);

                    //if (o)
                    //{
                    //    if (value == null) //clear
                    //    {
                    //        var r = EmptyClipboard();
                    //    }
                    //    else
                    //    {

                    //        var t = value;

                    //        var ptr = DllImportCaller.lib.GenerateIntPtrString(t);


                    //        var i = SetClipboardData(ClipboardFormats.UnicodeText, (IntPtr)ptr);
                    //    }
                    //}

                    //var c = CloseClipboard();
                    

                    //DllImportCaller.lib.Clipboard_SET(value);
                }
            }

            public enum CLIPFORMAT : int
            {
                CF_TEXT = 1,
                CF_BITMAP = 2,
                CF_METAFILEPICT = 3,
                CF_SYLK = 4,
                CF_DIF = 5,
                CF_TIFF = 6,
                CF_OEMTEXT = 7,
                CF_DIB = 8,
                CF_PALETTE = 9,
                CF_PENDATA = 10,
                CF_RIFF = 11,
                CF_WAVE = 12,
                CF_UNICODETEXT = 13,
                CF_ENHMETAFILE = 14,
                CF_HDROP = 15,
                CF_LOCALE = 16,
                CF_MAX = 17,
                CF_OWNERDISPLAY = 0x80,
                CF_DSPTEXT = 0x81,
                CF_DSPBITMAP = 0x82,
                CF_DSPMETAFILEPICT = 0x83,
                CF_DSPENHMETAFILE = 0x8E,
            } 
        }
    }
}
