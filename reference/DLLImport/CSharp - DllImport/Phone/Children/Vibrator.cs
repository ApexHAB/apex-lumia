
namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static class Vibrator
        {
            public static int Vibrate()
            {
                return z("Vibrate");
            }
            public static int Stop()
            {
                return z("Stop");
            }

            private static int z(string method)
            {
                //var o = DllImportCaller.NativeMethodExists("PlatformInterop", "Vibrate");
                return DllImportCaller.lib.VoidCall("PlatformInterop", method);
            }
        }
    }
}
