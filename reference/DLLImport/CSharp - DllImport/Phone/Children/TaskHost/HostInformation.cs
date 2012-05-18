using System;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static partial class TaskHost
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            public struct HostInformation
            {
                public uint ullLastInstanceId;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
                public string szProductId;
                public IntPtr hHostWnd;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szAppInstallFolder;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szAppDataFolder;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szAppIsolatedStorePath;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2084)]
                public string szUri;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2084)]
                public string szTaskPage;
                public uint fRehydrated;
                public uint fDehydrating;

                public override string ToString()
                {
                    return this.DumpToString();
                    //return string.Format(
                    //    "{{ fDehydrating = {0}, fRehydrated = {1}, hHostWnd = {2}, szAppDataFolder = {3}, szAppInstallFolder = {4}, szAppIsolatedStorePath = {5}, szProductId = {6}, szTaskPage = {7}, szUri = {8}, ullLastInstanceId = {9} }}",
                    //    fDehydrating, fRehydrated, hHostWnd, szAppDataFolder, szAppInstallFolder, szAppIsolatedStorePath, szProductId, szTaskPage, szUri, ullLastInstanceId);
                }

                public string GetFileSystemPath(string uri)
                {
                    if (string.IsNullOrEmpty(uri))
                    {
                        throw new ArgumentException("uri");
                    }
                    Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
                    string localPath = null;
                    string appInstallFolder = null;
                    if (uri2.IsAbsoluteUri && (((uri2.Port != -1) || !string.IsNullOrEmpty(uri2.Query)) || !string.IsNullOrEmpty(uri2.Host)))
                    {
                        throw new ArgumentException("uri");
                    }
                    if ((!uri2.IsAbsoluteUri || (string.CompareOrdinal(uri2.Scheme, "appdata") == 0)) || (string.CompareOrdinal(uri2.Scheme, "file") == 0))
                    {
                        appInstallFolder = this.szAppInstallFolder;//.AppInstallFolder;
                    }
                    else
                    {
                        if (string.CompareOrdinal(uri2.Scheme, "isostore") != 0)
                        {
                            throw new ArgumentException("uri");
                        }
                        appInstallFolder = this.szAppIsolatedStorePath;//.AppIsolatedStorePath;
                    }
                    if (uri2.IsAbsoluteUri)
                    {
                        localPath = uri2.LocalPath;
                    }
                    else
                    {
                        localPath = uri2.OriginalString;
                    }
                    if (localPath == null)
                    {
                        throw new ArgumentException("uri");
                    }
                    if (localPath.StartsWith("/") || localPath.StartsWith(@"\"))
                    {
                        localPath = localPath.Substring(1);
                    }
                    localPath = System.IO.Path.Combine(appInstallFolder, localPath);
                    var canonicalPathName = new System.Text.StringBuilder(260 /*MAXPATH C++*/);

                    canonicalPathName.Append(System.IO.Path.GetFullPath(localPath));
                    //if (HostInfoNativeMethods.CeGetCanonicalPathNameW(localPath, canonicalPathName, 260, 0) == 0)
                    //{
                    //    throw new ArgumentException("uri");
                    //}
                    string str3 = canonicalPathName.ToString();
                    if (!str3.StartsWith(appInstallFolder, StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new ArgumentException("uri");
                    }
                    return str3;
                }

            }
        }
    }
}
