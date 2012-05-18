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
    public static partial class Phone
    {
        public static partial class TaskHost
        {
            private static impersonateDestruct imp = new impersonateDestruct();

            /// <summary>
            /// Prevents leak of CeImpersonateCurrentProcess, all behind the scenes
            /// </summary>
            private class impersonateDestruct
            {
                public static bool ImpersonateCalled;
                ~impersonateDestruct()
                {
                    if (ImpersonateCalled)
                    {
                        TaskHost.CeRevertToSelf();
                    }
                }
            }

            public static HostInformation GetCurrenHostInfo()
            {
                HostInformation info;
                var t = DllImportCaller.lib.ThisTaskHostInformation("emclient", "GetHostInfo", out info);
                return info;
            }

            public static IntPtr ThisHWND
            {
                get
                {
                    return GetCurrenHostInfo().hHostWnd;
                }
            }

            /// <summary>
            /// Tells "this" TaskHost.exe not to dehydrate on system request. Still alive till killed by new apps.
            /// </summary>
            /// <param name="preventOn">True = prevent, False = dehydrate on system request</param>
            [Obsolete("Does not seems to want to cooperate..", true)]
            public static void PreventDehydrating(bool preventOn)
            {
                DllImportCaller.lib.BoolCall("aygshell", "SHSetAutoDehydratingHostEligibility", preventOn);
            }

            /// <summary>
            /// This function is used to ensure the correct return value for every case. It ensures that only someone with the impersonate privilege is able to do this. It also ensures that after a valid impersonate call, the access rights for the impersonating application are appropriate.
            /// | Call "CeRevertToSelf" after changed privileges
            /// </summary>
            /// <returns>TRUE indicates success. FALSE indicates failure.</returns>
            public static bool CeImpersonateCurrentProcess()
            {
                impersonateDestruct.ImpersonateCalled = true;
                return DllImportCaller.lib.VoidCall("coredll", "CeImpersonateCurrentProcess") == 1;
            }
            /// <summary>
            /// A process should call the RevertToSelf function after finishing any impersonation begun by using the DdeImpersonateClient, ImpersonateDdeClientWindow, ImpersonateLoggedOnUser, ImpersonateNamedPipeClient, ImpersonateSelf, ImpersonateAnonymousToken or SetThreadToken function.
            ///An RPC server that used the RpcImpersonateClient function to impersonate a client must call the RpcRevertToSelf or RpcRevertToSelfEx to end the impersonation.
            ///If RevertToSelf fails, your application continues to run in the context of the client, which is not appropriate. You should shut down the process if RevertToSelf fails.
            /// </summary>
            /// <returns></returns>
            public static bool CeRevertToSelf()
            {
                impersonateDestruct.ImpersonateCalled = false;
                return DllImportCaller.lib.VoidCall("coredll", "CeRevertToSelf") != 0;
            }
            
        }
    }
}
