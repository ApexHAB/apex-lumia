using System;
using System.Net;
using System.Windows;
using System.Security.Cryptography;
using System.Text;


namespace ApexLumia
{
    public static class Utils
    {
        /// <summary>
        /// Encode a string into a Base64 string version.
        /// </summary>
        /// <param name="thing">The string to encode into a Base64 version.</param>
        /// <returns></returns>
        static public string base64ify(string thing)
        {
            byte[] thebytesofthing = UTF8Encoding.UTF8.GetBytes(thing);
            string base64ified = Convert.ToBase64String(thebytesofthing);
            return base64ified;
        }

        /// <summary>
        /// Hash a string using SHA256
        /// </summary>
        /// <param name="thing">The string to hash.</param>
        /// <returns></returns>
        static public string sha256ify(string thing)
        {
            byte[] thebytesofthing = UTF8Encoding.UTF8.GetBytes(thing);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] sha256ifiedbytes = algorithm.ComputeHash(thebytesofthing);

            string hex = "";
            foreach (byte x in sha256ifiedbytes) { hex += String.Format("{0:x2}", x); }

            return hex;

        }

        static public byte[] hmacsha1ify(string thing, string key)
        {
            byte[] thebytesofthing = UTF8Encoding.UTF8.GetBytes(thing);
            byte[] thebytesofkey = UTF8Encoding.UTF8.GetBytes(key);

            HMACSHA1 algorithm = new HMACSHA1(thebytesofkey);
            byte[] buff = algorithm.ComputeHash(thebytesofthing);

            return buff;
        }

        /// <summary>
        /// Guess what: it gives you a UNIX timestamp.
        /// </summary>
        /// <returns></returns>
        static public int unix_timestamp()
        {
            TimeSpan unix_time = (System.DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (int)unix_time.TotalSeconds;
        }

        static public string uniqueAlphanumericString()
        {
            Guid g = Guid.NewGuid(); // Not random or secure, but still unique.
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Replace("/", "");
            return GuidString;

        }

        /// <summary>
        /// The set of characters that are unreserved in RFC 2396 but are NOT unreserved in RFC 3986.
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/846487/how-to-get-uri-escapedatastring-to-comply-with-rfc-3986" />
        private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };

        private static readonly string[] UriRfc3968EscapedHex = new[] { "%21", "%2A", "%27", "%28", "%29" };

        /// <summary>
        /// URL encodes a string based on section 5.1 of the OAuth spec.
        /// Namely, percent encoding with [RFC3986], avoiding unreserved characters,
        /// upper-casing hexadecimal characters, and UTF-8 encoding for text value pairs.
        /// </summary>
        /// <param name="value">The value to escape.</param>
        /// <returns>The escaped value.</returns>
        /// <remarks>
        /// The <see cref="Uri.EscapeDataString"/> method is <i>supposed</i> to take on
        /// RFC 3986 behavior if certain elements are present in a .config file.  Even if this
        /// actually worked (which in my experiments it <i>doesn't</i>), we can't rely on every
        /// host actually having this configuration element present.
        /// </remarks>
        /// <seealso cref="http://oauth.net/core/1.0#encoding_parameters" />
        /// <seealso cref="http://stackoverflow.com/questions/846487/how-to-get-uri-escapedatastring-to-comply-with-rfc-3986" />
        public static string UrlEncodeRelaxed(string value)
        {
            // Start with RFC 2396 escaping by calling the .NET method to do the work.
            // This MAY sometimes exhibit RFC 3986 behavior (according to the documentation).
            // If it does, the escaping we do that follows it will be a no-op since the
            // characters we search for to replace can't possibly exist in the string.
            StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(value));

            // Upgrade the escaping to RFC 3986, if necessary.
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                string t = UriRfc3986CharsToEscape[i];
                escaped.Replace(t, UriRfc3968EscapedHex[i]);
            }

            // Return the fully-RFC3986-escaped string.
            return escaped.ToString();
        }

    }
}
