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
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace CSharp___DllImport
{
    public class HackedShellTileEnumerator : IEnumerable<HackedShellTile>, System.Collections.IEnumerable, IEnumerator<HackedShellTile>, IDisposable, System.Collections.IEnumerator
    {
        private ITokenIDEnum tokenIdEnum;
        private bool first;
        private HackedShellTile current;

        public void Reset()
        {
            this.first = true;
            HackedShellTile.TokenManager.GetTokenIDEnum2(HackedShellTile.AppId, out this.tokenIdEnum);
        }

        public HackedShellTileEnumerator()
        {
            Reset();
        }

        public IEnumerator<HackedShellTile> GetEnumerator()
        {
            return this;
        }
        object System.Collections.IEnumerator.Current
        {
            get
            {
                return this.current;
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            /*BufferHandle*/
            IntPtr handle;
            if ((this.tokenIdEnum.get_NextTokenID(out handle) != 0) || handle == IntPtr.Zero)//.IsInvalid)
            {
                return false;
            }
            string invocationUri = MarshalBypass.PtrToStringUni(handle); //Marshal.PtrToStringUni(handle.DangerousGetHandle());
            if (this.first)
            {
                this.first = false;
                this.current = new HackedShellTile(invocationUri, true);
            }
            else
            {
                this.current = new HackedShellTile(invocationUri);
            }
            //handle.Dispose(); => Leaks goes here :P (Cant dispose due hacked)
            return true;
        }
        public HackedShellTile Current
        {
            get
            {
                return this.current;
            }
        }

        public void Dispose() { }
    }

    public class HackedShellTileValues : Dictionary<TOKEN_PROPERTY_TYPE, string> { }

    public class HackedShellTile
    {
        public string id;
        public Uri NavigationUri;

        public HackedShellTile(string invocationUri, bool isDefault = false)
        {
            if (isDefault)
            {
                NavigationUri = DEFAULT;
            }
            else
            {
                NavigationUri = new Uri(invocationUri, UriKind.Relative);
            }

            this.id = invocationUri;
        }

        internal static byte[] HardConvertUnicodeToASCIIByte(string unicode)
        {
            byte[] bytes = new UnicodeEncoding().GetBytes(unicode);
            byte[] buffer2 = new byte[bytes.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((i % 2) == 0)
                {
                    buffer2[i / 2] = bytes[i];
                }
            }
            return buffer2;
        }

        private static bool IsRemoteUri(Uri uriTobeExamine)
        {
            return (uriTobeExamine.IsAbsoluteUri && (uriTobeExamine.Scheme == "http"));
        }
        private static bool IsRelativeUri(Uri uriTobeExamine)
        {
            return !uriTobeExamine.IsAbsoluteUri;
        }
        private static bool IsResourceUri(Uri uriTobeExamine)
        {
            return (uriTobeExamine.OriginalString.IndexOf("res://", StringComparison.CurrentCultureIgnoreCase) >= 0);
        }
        internal static string GetLocalFilePath(Uri imageUri)
        {
            if (imageUri.OriginalString == string.Empty)
            {
                return (Uri.UriSchemeFile + Uri.SchemeDelimiter);
            }

            var info = Phone.TaskHost.GetCurrenHostInfo();
            //HostInfo info = new HostInfo();
            string fileSystemPath = info.GetFileSystemPath(imageUri.OriginalString);
            if ((imageUri.IsAbsoluteUri && (string.CompareOrdinal(imageUri.Scheme, "isostore") == 0)) && !fileSystemPath.StartsWith(info.szAppIsolatedStorePath + @"\Shared\ShellContent\", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException(imageUri.ToString());
            }
            return (Uri.UriSchemeFile + Uri.SchemeDelimiter + fileSystemPath);
        }

        private static bool IsTileContainsAnyRemoteURL(Microsoft.Phone.Shell.StandardTileData data)
        {
            bool flag = false;
            if (((data.BackgroundImage != null) && data.BackgroundImage.IsAbsoluteUri) && (data.BackgroundImage.Scheme == "http"))
            {
                flag = true;
            }
            if ((!flag && (data.BackBackgroundImage != null)) && (data.BackBackgroundImage.IsAbsoluteUri && (data.BackBackgroundImage.Scheme == "http")))
            {
                flag = true;
            }
            return flag;
        }



        /*
         * MS Usage:
           [Tile(PropertyId=TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_COUNT)]
           public int? Count { [CompilerGenerated] get; [CompilerGenerated] set; }
             
         */
        [AttributeUsage(AttributeTargets.Property)]
        public class TileAttribute : Attribute
        {
            // Fields
            private string defaultValue;
            private TOKEN_PROPERTY_TYPE propertyId;
            private string valueConverter;

            // Properties
            public string DefaultValue { get; set; }
            public TOKEN_PROPERTY_TYPE PropertyId { get; set; }
            public string ValueConverter { get; set; }
        }




        internal static void SerializeToToken(Microsoft.Phone.Shell.StandardTileData data /*Due static extracted this is here*/, IToken token)
        {
            foreach (System.Reflection.PropertyInfo info in data.GetType().GetProperties())
            {
                foreach (Attribute attribute in info.GetCustomAttributes(true))
                {
                    TileAttribute attribute2 = attribute as TileAttribute;
                    //if ((attribute2 != null) && data.TemplateTypeProperties.Contains<TOKEN_PROPERTY_TYPE>(attribute2.PropertyId))
                    //{
                    //    object obj2 = info.GetValue(data, null);
                    //    string defaultValue = attribute2.DefaultValue;
                    //    if (obj2 != null)
                    //    {
                    //        if (attribute2.ValueConverter == null)
                    //        {
                    //            defaultValue = string.Format("{0}", obj2);
                    //        }
                    //        else
                    //        {
                    //            var converter = (System.Windows.Data.IValueConverter)Activator.CreateInstance(Type.GetType(attribute2.ValueConverter));
                    //            defaultValue = (string)converter.Convert(obj2, typeof(string), null, CultureInfo.InvariantCulture);
                    //        }
                    //    }
                    //    if (defaultValue != null)
                    //    {
                    //        uint num = (uint)(5 /*5 = default*//*data.TemplateType*/ << 0x10);
                    //        uint propertyId = (uint)(((TOKEN_PROPERTY_TYPE)num) | attribute2.PropertyId);
                    //        if (defaultValue.Length > 0x200)
                    //        {
                    //            throw new ArgumentException("The serialized property " + info.Name + " is too long");
                    //        }
                    //        token.set_Property(propertyId, defaultValue);
                    //    }
                    //}
                }
            }
        }

        [Obsolete("Not assigning ShellTileData correctly yet", true)]
        public static void Create(Uri navigationUri, Microsoft.Phone.Shell.ShellTileData initialData)
        {
            if (navigationUri == null)
            {
                throw new ArgumentNullException("navigationUri");
            }
            if (initialData == null)
            {
                throw new ArgumentNullException("initialData");
            }
            if (DEFAULT.Equals(navigationUri))
            {
                throw new ArgumentOutOfRangeException("navigationUri");
            }
            if (navigationUri.IsAbsoluteUri)
            {
                throw new ArgumentException("navigationUri cannot be absolute");
            }
            if ((navigationUri.ToString().IndexOf('/') != 0) && (navigationUri.ToString().IndexOf('?') != 0))
            {
                throw new UriFormatException();
            }
            int hr = 0;
            var data = (Microsoft.Phone.Shell.StandardTileData)initialData;
            if ((data != null) && IsTileContainsAnyRemoteURL(data))
            {
                var xml = ConstructTokenXmlString(data, navigationUri.ToString());
                byte[] tokenXml = HardConvertUnicodeToASCIIByte(xml);
                var schedule = new PM_LIVETOKEN_SCHEDULE();
                schedule.recurrenceType = PM_LIVETOKEN_RECURRENCE_TYPE.PM_LIVETOKEN_RECURRENCE_TYPE_INSTANT;
                schedule.fUserNoneIdle = true;
                hr = TokenManager.CreateTokenWithLiveTokenSubscription(AppId, navigationUri.ToString(), ((data.BackgroundImage != null) && IsRemoteUri(data.BackgroundImage)) ? data.BackgroundImage.OriginalString.ToString() : null, ((data.BackBackgroundImage != null) && IsRemoteUri(data.BackBackgroundImage)) ? data.BackBackgroundImage.OriginalString.ToString() : null, ref schedule, tokenXml, (uint)tokenXml.Length);
            }
            else
            {
                IToken token = null;
                string taskParameters = navigationUri.ToString();
                TokenManager.MakeToken(AppId, navigationUri.ToString(), 5 /* 5 = default*/ /*initialData.TemplateType*/, out token);
                /*initialData.*/
                //SerializeToToken(data, token);
                if (navigationUri.ToString().IndexOf('/') == 0)
                {
                    taskParameters = "#" + navigationUri.ToString();
                }
                // Dllimport => 8dc5214e-88fa-4c2d-a379-2cd74fe24b72
                token.set_InvocationInfo("_default{7C288D6D-AC47-4ac8-A849-9147FF693061}", taskParameters);
                uint num2 = (uint)(5/*initialData.TemplateType*/ << 0x10);
                uint propertyId = num2 | 0x25;
                token.set_Property(propertyId, "");
                hr = TokenManager.AddToken(token);
            }
            ThrowExceptionFromHResult(hr, new InvalidOperationException());
        }

        public static void CreateFreedomTile(
            Guid productId,
            string navigationUri,
            string taskName = "_default",
            Func<HackedShellTileValues> creationProperties = null,
            int tileTemplate = 5)
        {
            //calc = {5B04B775-356B-4AA0-AAF8-6491FFEA5603}

            var appid = TokenmanagerSingleton.GetAppIDFromProductID(productId);
            IToken token = null;
            TokenManager.MakeToken(appid, navigationUri, tileTemplate /* 5 = default*/ /*initialData.TemplateType*/, out token);
            token.set_InvocationInfo(taskName, navigationUri); //"decompiled default => _default{7C288D6D-AC47-4ac8-A849-9147FF693061}"
            uint num2 = (uint)(tileTemplate/*initialData.TemplateType*/ << 0x10);
            uint propertyId = num2 | 0x25;
            token.set_Property(propertyId, "");

            if (creationProperties != null)
            {
                var enumKeyPair = creationProperties();
                foreach (var property in enumKeyPair)
                {
                    uint val = (uint)(((TOKEN_PROPERTY_TYPE)(uint)(tileTemplate << 0x10)) | property.Key);

                    token.set_Property(val, property.Value);
                }
            }

            var hr = TokenManager.AddToken(token);

            ThrowExceptionFromHResult(hr, new InvalidOperationException());
        }
        public static void CreateFreedomTile(
           Guid productId,
           string navigationUri,
           string taskName = "_default",
           HackedTileInterface creationProperties = null)
        {
            CreateFreedomTile(productId, navigationUri, taskName, creationProperties.GenerateTileValues, creationProperties.TemplateID);
        }
        internal static void ThrowExceptionFromHResult(int hr, Exception defaultException)
        {
            switch (hr)
            {
                case -2147024882:
                case -2147024871:
                    throw new InvalidOperationException();

                case -2147467263:
                    throw new NotSupportedException();

                case -2147024890:
                case -2147024809:
                    throw new ArgumentException("E_INVALIDARG");

                case -2130509545:
                    throw new ArgumentException("ShellInvalidUri");

                case 0:
                case 1:
                    return;

                case -2130509534:
                    throw new InvalidOperationException("CreateTileRestricted");


                //Added by fiinix
                case -2147024713:
                    throw new Exception("A tile matching exact already exist");
            }
            throw defaultException;
        }
        private static string ConstructTokenXmlString(Microsoft.Phone.Shell.StandardTileData tile, string TokenId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.AppendLine("<PushNotification>");
            if (TokenId != null)
            {
                builder.AppendLine(string.Format("<Token TokenID=\"{0}\">", TokenId));
            }
            else
            {
                builder.AppendLine("<Token xmlns=\"\" TokenID=\"\">");
            }
            builder.AppendLine("<TemplateType5>");
            if (tile.Count.HasValue)
            {
                builder.AppendLine(string.Format("<Count>{0}</Count>\r\n", tile.Count));
            }
            if (tile.Title != null)
            {
                builder.AppendLine(string.Format("<Title>{0}</Title>\r\n", tile.Title));
            }
            if (tile.BackgroundImage != null)
            {
                if (IsRemoteUri(tile.BackgroundImage))
                {
                    builder.AppendLine("<BackgroundImageURI IsRelative=\"false\" IsResource=\"false\">DummyURI</BackgroundImageURI>\r\n");
                }
                else
                {
                    builder.AppendLine(string.Format("<BackgroundImageURI IsRelative=\"{0}\" IsResource=\"{1}\">{2}</BackgroundImageURI>\r\n", IsRelativeUri(tile.BackgroundImage), IsResourceUri(tile.BackgroundImage), GetLocalFilePath(tile.BackgroundImage)));
                }
            }
            if (tile.BackContent != null)
            {
                builder.AppendLine(string.Format("<BackContent>{0}</BackContent>\r\n", tile.BackContent));
            }
            if (tile.BackTitle != null)
            {
                builder.AppendLine(string.Format("<BackTitle>{0}</BackTitle>\r\n", tile.BackTitle));
            }
            if (tile.BackBackgroundImage != null)
            {
                if (IsRemoteUri(tile.BackBackgroundImage))
                {
                    builder.AppendLine("<BackBackgroundImageURI IsRelative=\"false\" IsResource=\"false\">DummyURI</BackBackgroundImageURI>\r\n");
                }
                else
                {
                    builder.AppendLine(string.Format("<BackBackgroundImageURI IsRelative=\"{0}\" IsResource=\"{1}\">{2}</BackBackgroundImageURI>\r\n", IsRelativeUri(tile.BackBackgroundImage), IsResourceUri(tile.BackBackgroundImage), GetLocalFilePath(tile.BackBackgroundImage)));
                }
            }
            builder.AppendLine("</TemplateType5>");
            builder.AppendLine("</Token>");
            builder.AppendLine("</PushNotification>");
            return builder.ToString();
        }

        public static ITokenManager TokenManager
        {
            get
            {
                return TokenmanagerSingleton.Instance;
            }
        }

        public static ulong AppId
        {
            get
            {
                return TokenmanagerSingleton.AppId;
            }
        }

        private static readonly Uri DEFAULT = new Uri("/", UriKind.Relative);

        public static IEnumerable<HackedShellTile> ActiveTiles
        {
            get
            {
                return new HackedShellTileEnumerator();
            }
        }
    }

    public abstract class HackedTileInterface
    {
        public readonly int TemplateID;
        public HackedTileInterface(int templateID)
        {
            this.TemplateID = templateID;
        }

        public abstract HackedShellTileValues GenerateTileValues();

        protected void AddIfNotNull(HackedShellTileValues vals, TOKEN_PROPERTY_TYPE type, string value)
        {
            if (value != null)
            {
                vals.Add(type, value);
            }
        }
    }
    public sealed class HackedWidePictureAnimator : HackedTileInterface
    {
        public string[] Images;
        public HackedWidePictureAnimator(params string[] images)
            : base(14)
        {
            if (images.Length > 18)
            {
                throw new ArgumentException("No more than 18 images allowed");
            }

            this.Images = images;
        }

        public override HackedShellTileValues GenerateTileValues()
        {
            var vals = new HackedShellTileValues();

            var current = TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_PHOTO01IMAGEURI;
            for (int i = 0; i < Images.Length; i++)
            {
                vals.Add(current, Images[i]);
                current++;
            }

            return vals;
        }
    }
    public sealed class HackedCalendarTile : HackedTileInterface
    {
        public string AppointmentTitle { get; private set; }
        public string AppointmentDescription { get; private set; }
        public string AppointmentLocation { get; private set; }
        public HackedCalendarTile(string appointmentTitle, string appointmentDescription, string appointmentLocation)
            : base(4)
        {
            this.AppointmentTitle = appointmentTitle;
            this.AppointmentDescription = appointmentDescription;
            this.AppointmentLocation = appointmentLocation;
        }

        public override HackedShellTileValues GenerateTileValues()
        {
            var vals = new HackedShellTileValues();

            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_APPOINTMENTTITLE, AppointmentTitle);
            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_APPOINTMENTDESCRIPTION, AppointmentDescription);
            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_APPOINTMENTLOCATION, AppointmentLocation);

            return vals;
        }
    }
    public sealed class HackedIconTile : HackedTileInterface
    {
        public string IconImageUri { get; private set; }
        public string Title { get; private set; }
        public HackedIconTile(string iconImageUri, string title)
            : base(1)
        {
            this.IconImageUri = iconImageUri;
            this.Title = title;
        }

        public override HackedShellTileValues GenerateTileValues()
        {
            var vals = new HackedShellTileValues();

            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_ICONIMAGEURI, IconImageUri);
            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_TITLE, Title);

            return vals;
        }
    }
    public sealed class HackedWideIconTile : HackedTileInterface
    {
        public string BackgroundImageURI { get; private set; }
        public string Title { get; private set; }
        public HackedWideIconTile(string backgroundImageURI, string title)
            : base(6)
        {
            this.BackgroundImageURI = backgroundImageURI;
            this.Title = title;
        }
        public override HackedShellTileValues GenerateTileValues()
        {
            var vals = new HackedShellTileValues();

            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_BACKGROUNDIMAGEURI, BackgroundImageURI);
            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_TITLE, Title);

            return vals;
        }
    }
    public sealed class HackedXboxTile : HackedTileInterface
    {
        public string Title { get; private set; }
        public HackedXboxTile(string title)
            : base(3)
        {
            this.Title = title;
        }

        public override HackedShellTileValues GenerateTileValues()
        {
            //HackedShellTile.CreateFreedomTile(new Guid("{8dc5214e-88fa-4c2d-a379-2cd74fe24b72}"), "?id=" + rand(), "_default", () =>
            //{
            //    return new HackedShellTileValues
            //    {
            //       { TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_IMAGEURI, "res://StartMenu!token.xboxlive.png" },
            //       { TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_BACKGROUNDIMAGEURI, "res://StartMenu!token.xboxlive.background.png" },
            //       { TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_TITLE, "@BrowsuiRes.dll,-12251" }
            //    };
            //}, 3);

            var vals = new HackedShellTileValues();

            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_IMAGEURI, "res://StartMenu!token.xboxlive.png");
            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_BACKGROUNDIMAGEURI, "res://StartMenu!token.xboxlive.background.png");
            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_TITLE, Title);

            return vals;
        }
    }
    public sealed class HackedSimpleTextTile : HackedTileInterface
    {
        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public HackedSimpleTextTile(string title, string subTitle)
            : base(9)
        {
            this.Title = title;
            this.SubTitle = subTitle;
        }

        public override HackedShellTileValues GenerateTileValues()
        {
            var vals = new HackedShellTileValues();

            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_SUBTITLE, SubTitle);
            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_TITLE, Title);

            return vals;
        }
    }
    public sealed class HackedSeparatorTitle : HackedTileInterface
    {
        public HackedSeparatorTitle()
            : base(15) { }

        public override HackedShellTileValues GenerateTileValues()
        {
            return new HackedShellTileValues();
        }
    }
    public sealed class HackedPeopleHubTitle : HackedTileInterface
    {
        public string[] Images;
        public string Title { get; private set; }
        public HackedPeopleHubTitle(string title, params string[] images)
            : base(10)
        {
            if (images.Length > 18)
            {
                throw new ArgumentException("No more than 18 images allowed");
            }

            this.Images = images;

            this.Title = title;
        }

        public override HackedShellTileValues GenerateTileValues()
        { 
            var vals = new HackedShellTileValues();

            AddIfNotNull(vals, TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_TITLE, Title);

            var current = TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_PHOTO01IMAGEURI;
            for (int i = 0; i < Images.Length; i++)
            {
                vals.Add(current, Images[i]);
                current++;
            }

            return vals;
        }
    }
    public sealed class HackedTrippleRowTitle : HackedTileInterface
    {
        public string[] Images;
        public HackedTrippleRowTitle(params string[] zero_to_threeImages)
            : base(12)
        {
            if (zero_to_threeImages.Length > 3)
            {
                throw new ArgumentException("No more than 3 images allowed");
            }

            this.Images = zero_to_threeImages;
        }

        public override HackedShellTileValues GenerateTileValues()
        {
            var vals = new HackedShellTileValues();

            var current = TOKEN_PROPERTY_TYPE.TOKEN_PROPERTY_PHOTO01IMAGEURI;
            for (int i = 0; i < Images.Length; i++)
            {
                vals.Add(current, Images[i]);
                current++;
            }

            return vals;
        }
    }


    public class TokenmanagerSingleton // decompiled internal "Microsoft.Phone.Shell.ShellTile+TokenmanagerSingleton"
    {
        public static ITokenManager Instance = null;
        private static ulong myAppId;

        public static ulong GetAppIDFromProductID(Guid product)
        {
            ulong ret;
            Instance.GetAppIDFromProductID(product, out ret);
            return ret;
        }

        static TokenmanagerSingleton()
        {
            var last = Registrer.Register("pacmanclient.dll", "248DD447-4295-4888-BC5A-5D87F3705F74");
            var c = new TokenManager();
            var i = (ITokenManager)c;

            var info = Phone.TaskHost.GetCurrenHostInfo(); //info => COM hack throughpassed

            Instance = i;

            //Guid productId = new Guid(info.szProductId); // native decompile of "return this._hostInformation.szProductId;"
            //Instance.GetAppIDFromProductID(productId, out myAppId);
            myAppId = GetAppIDFromProductID(new Guid(info.szProductId));
        }

        public static ulong AppId
        {
            get
            {
                return myAppId;
            }
        }
    }

    /// <summary>
    /// pacmanclient class
    /// </summary>
    [ComImport, Guid("248DD447-4295-4888-BC5A-5D87F3705F74"), ClassInterface(ClassInterfaceType.None)]
    public class TokenManager
    {
    }

    /// <summary>
    /// pacmanclient interface
    /// </summary>
    [ComImport, Guid("AD864139-4547-45e8-8981-C91F7C40AD69"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITokenManager
    {
        void GetAppIDFromProductID(Guid productId, out ulong appId);
        void MakeToken(ulong appID, string tokenId, int templateType, out IToken token);
        [PreserveSig]
        int AddToken(IToken token);
        void UpdateToken(IToken token);
        void DeleteToken(ulong appID, string tokenID);
        void GetToken(ulong appID, string tokenID, out IToken token);
        void GetTokenIDEnum(ulong appID, out ITokenIDEnum tokenEnum);
        void GetTokenIDEnum2(ulong appID, out ITokenIDEnum tokenEnum);
        [PreserveSig]
        int CreateTokenWithLiveTokenSubscription(ulong appID, string tokenId, string remoteImageUrl, string remoteBackImageUrl, ref PM_LIVETOKEN_SCHEDULE schedule, [In, MarshalAs(UnmanagedType.LPArray)] byte[] tokenXml, uint tokenXmlSize);
    }

    [ComImport, Guid("C66A047A-F755-4515-A573-F059C29736E2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IToken
    {
        [PreserveSig]
        int set_Property(uint propertyId, string propValue);
        [PreserveSig]
        int set_InvocationInfo(string TaskName, string taskParameters);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("09C3BA72-DCE3-4203-B67D-54669E33A8E8")]
    public interface ITokenIDEnum
    {
        [PreserveSig]
        int get_NextTokenID(out /*BufferHandle as typeof SafeHandle (managed IntPtr handle)*/ IntPtr tokenid);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PM_LIVETOKEN_SCHEDULE
    {
        public PM_LIVETOKEN_RECURRENCE_TYPE recurrenceType;
        public PM_LIVETOKEN_INTERVAL_KIND intervalKind;
        public uint runForever;
        public uint maximumRunCount;
        public uint runCount;
        public uint reserved;
        public ulong startTime;
        public ulong nextTime;
        public bool fUserNoneIdle;
    }
    [Flags]
    public enum PM_LIVETOKEN_INTERVAL_KIND
    {
        PM_LIVETOKEN_INTERVAL_KIND_EVERY_HOUR,
        PM_LIVETOKEN_INTERVAL_KIND_EVERY_DAY,
        PM_LIVETOKEN_INTERVAL_KIND_EVERY_WEEK,
        PM_LIVETOKEN_INTERVAL_KIND_EVERY_MONTH
    }
    [Flags]
    public enum PM_LIVETOKEN_RECURRENCE_TYPE
    {
        PM_LIVETOKEN_RECURRENCE_TYPE_INSTANT,
        PM_LIVETOKEN_RECURRENCE_TYPE_ONETIME,
        PM_LIVETOKEN_RECURRENCE_TYPE_INTERVAL
    }
    public enum TOKEN_PROPERTY_TYPE
    {
        TOKEN_PROPERTY_ANIMATION = 0x35,
        TOKEN_PROPERTY_APPOINTMENTDESCRIPTION = 9,
        TOKEN_PROPERTY_APPOINTMENTLOCATION = 10,
        TOKEN_PROPERTY_APPOINTMENTTITLE = 8,
        TOKEN_PROPERTY_AVATARIMAGEURI = 0x24,
        TOKEN_PROPERTY_BACKBACKGROUNDIMAGEURI = 0x29,
        TOKEN_PROPERTY_BACKCONTENT = 0x31,
        TOKEN_PROPERTY_BACKGROUNDIMAGEURI = 1,
        TOKEN_PROPERTY_BACKTITLE = 0x30,
        TOKEN_PROPERTY_CONFLICTINDICATOR = 11,
        TOKEN_PROPERTY_CONTACTPHOTOIMAGEURI = 14,
        TOKEN_PROPERTY_COUNT = 12,
        TOKEN_PROPERTY_CROPTOTOP = 40,
        TOKEN_PROPERTY_DAYNAME = 7,
        TOKEN_PROPERTY_DAYNUMBER = 6,
        TOKEN_PROPERTY_ICON2IMAGEURI = 0x34,
        TOKEN_PROPERTY_ICONIMAGEURI = 3,
        TOKEN_PROPERTY_IMAGEURI = 0x23,
        TOKEN_PROPERTY_INVALID = 0,
        TOKEN_PROPERTY_MESSAGE = 15,
        TOKEN_PROPERTY_MESSAGEPHOTOIMAGEURI = 0x22,
        TOKEN_PROPERTY_MESSAGETITLE = 0x33,
        TOKEN_PROPERTY_NOTIFICATIONTITLE = 50,
        TOKEN_PROPERTY_NUMBER = 2,
        TOKEN_PROPERTY_PHOTO01IMAGEURI = 0x10,
        TOKEN_PROPERTY_PHOTO02IMAGEURI = 0x11,
        TOKEN_PROPERTY_PHOTO03IMAGEURI = 0x12,
        TOKEN_PROPERTY_PHOTO04IMAGEURI = 0x13,
        TOKEN_PROPERTY_PHOTO05IMAGEURI = 20,
        TOKEN_PROPERTY_PHOTO06IMAGEURI = 0x15,
        TOKEN_PROPERTY_PHOTO07IMAGEURI = 0x16,
        TOKEN_PROPERTY_PHOTO08IMAGEURI = 0x17,
        TOKEN_PROPERTY_PHOTO09IMAGEURI = 0x18,
        TOKEN_PROPERTY_PHOTO10IMAGEURI = 0x19,
        TOKEN_PROPERTY_PHOTO11IMAGEURI = 0x1a,
        TOKEN_PROPERTY_PHOTO12IMAGEURI = 0x1b,
        TOKEN_PROPERTY_PHOTO13IMAGEURI = 0x1c,
        TOKEN_PROPERTY_PHOTO14IMAGEURI = 0x1d,
        TOKEN_PROPERTY_PHOTO15IMAGEURI = 30,
        TOKEN_PROPERTY_PHOTO16IMAGEURI = 0x1f,
        TOKEN_PROPERTY_PHOTO17IMAGEURI = 0x20,
        TOKEN_PROPERTY_PHOTO18IMAGEURI = 0x21,
        TOKEN_PROPERTY_PRIMARYTOKEN = 0x26,
        TOKEN_PROPERTY_REPLACETOKEN = 0x27,
        TOKEN_PROPERTY_SENDER = 0x36,
        TOKEN_PROPERTY_SHOWONCREATE = 0x25,
        TOKEN_PROPERTY_SUBTITLE = 5,
        TOKEN_PROPERTY_TIME = 13,
        TOKEN_PROPERTY_TITLE = 4
    }
}
