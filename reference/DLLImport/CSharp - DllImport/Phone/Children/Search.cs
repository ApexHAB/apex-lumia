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
        public class Search
        {
            /*
         * All @ IAFapi.dll
         * 
         * GetPageFilterString 
         * InitializeInAppFiltering 
         * LaunchOnlineSearch 
         * RegisterSearchablePage 
         * SetActiveFilterable 
         * SetFilteringCallback 
         * SetOnlineQuery 
         * StartPageFiltering 
         * UninitializeInAppFiltering 
         * UnregisterSearchablePage 
         */
            private static string bindedPage;
            public static int BindSearchButtonToPage(string pageurl)
            {
                if (pageurl == null) throw new ArgumentNullException("pageurl");
                if (!pageurl.EndsWith(".xaml")) throw new ArgumentException("pageurl does not end with \".xaml\". Try \"MyPages/[Page].xaml\"");

                if (bindedPage != null)
                {
                    UnBindSearchableButtonToPage();
                }

                bindedPage = pageurl;
                var val = DllImportCaller.lib.StringCall("IAFapi", "RegisterSearchablePage", pageurl);

                return val;
            }
            public static int UnBindSearchableButtonToPage()
            {
                if (bindedPage != null)
                {
                    var val = DllImportCaller.lib.VoidCall("IAFapi", "UnregisterSearchablePage");
                    return val;
                }
                return 0;
            }

            public static int SearchFor(string value)
            {
                if(value == null) throw new ArgumentNullException("value; Cant search null");

                return Phone.AppLauncher.LaunchBuiltInApplication(AppLauncher.Apps.SearchHome, "SearchResults?QueryString=" + value);
            }
            public static int OpenSearch()
            {
                return DllImportCaller.lib.StringCall("IAFapi", "LaunchOnlineSearch", "");
            }
        }
    }
}
