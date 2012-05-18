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
        public static class Zune
        {
            //Microsoft.Xna.Framework.Media.UnsafeNativeMethods
            public static int Resume()
            {
                return z("Media_Queue_Resume");
            }
            public static int Stop()
            {
                return z("MediaQueue_Stop");
            }
            public static int Pause()
            {
                return z("Media_Queue_Pause");
            }
            public static int NextSong()
            {
                return z("MediaQueue_MoveNext");
            }
            public static int PrevSong()
            {
                return z("MediaQueue_MovePrev");
            }
            public static int ShutdownZune()
            {
                return z("Media_Shutdown");
            }
            public static int StartZune()
            {
                return z("Media_Initialize");
            }


            private static int z(string method)
            {
                return DllImportCaller.lib.VoidCall("XnaFrameworkCore", method);
            }

            public static int ZuneTaskDo(ZuneTasks task, string args = "")
            {
                return Phone.AppLauncher.LaunchBuiltInApplication(Phone.AppLauncher.Apps.Zune, task.ToString() + args);
            }

            /// <summary>
            /// Only supports video according to Microsoft. This is the native video player.
            /// </summary>
            /// <param name="videourl"></param>
            /// <returns></returns>
            public static int ZunePlayUrl(string videourl)
            {
                return ZuneTaskDo(ZuneTasks._PlayUrl, "?url=" + videourl);
            }

            public static class Radio
            {
                public static bool InOn
                {
                    set
                    {
                        int val = value ? 1 : 0;
                        DllImportCaller.lib.Radio_Toggle((uint)val);
                    }
                }
            }
            public enum ZuneTasks
            {
                MusicAndVideoHub,
                MarketplaceHub,
                GamesMarketplace,
                MusicMarketplaceHub,
                PodcastMarketplace,
                RadioSettings,
                MarketplaceSettings,



                //EXTENDED

                /// <summary>
                /// <!-- url: the name of the file to play (can be music or video). -->
                /// </summary>
                _PlayUrl,

                /// <summary>
                /// <!-- zmi: the ID of the media item to play (can be decimal or hex prefixed with '0x'). Videos ONLY! -->
                /// </summary>
                _PlayZmi,

                /// <summary>
                /// <!-- zmi: the ID of the media item/container to play (can be decimal or hex prefixed with '0x'). -->
                /// </summary>
                _PlayLocalMusic,
                _PlayLocalMusicContainer,
                _PlayLocalAudioPodcastSeries,
                _PlayLocalVideoPodcastSeries,
                _PlayLocalVideoFromBookmark,

                AppMarketplaceHub,

                /// <summary>
                /// <!-- id: the product/Zest ID of the app to show. -->
                /// </summary>
                AppDetails,

                /// <summary>
                /// <!-- id: the product/Zest ID of the app to show. -->
                /// </summary>
                AppReviews,

                RadioNowPlaying,
                MusicNowPlaying,
                VideoNowPlaying,
                PodcastNowPlaying,

                LocalPodcasts,

                /// <summary>
                /// <!--url: where to navigate to, of the form: zune://navigate/?mediatypeID=serviceguid -->
                /// </summary>
                _ShowMarketplaceItem,


                /// <summary>
                ///  <!--
                ///  All search is performed by MarketplaceSearch, using the 'hint' to differentiate
                ///  Parameter "SearchHint" is one of "application", "music", or "podcast"
                ///  Parameter "SearchTerm" is a string containing the terms to find
                ///  -->
                /// </summary>
                MarketplaceSearch,

                /// <summary>
                /// <!--
                ///  Navigates to the application marketplace search page with the search seeded with the given query string.
                ///  query: string to search for
                ///  -->
                /// </summary>
                AppMarketplaceSearch,

                /// <summary>
                /// <!-- AccountSetupReason: numeric reason for being at account setup -->
                /// </summary>
                AccountSetup,
            }
        }
    }
}
