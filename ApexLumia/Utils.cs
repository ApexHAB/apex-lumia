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
        static private string base64ify(string thing)
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
        static private string sha256ify(string thing)
        {
            byte[] thebytesofthing = UTF8Encoding.UTF8.GetBytes(thing);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] sha256ifiedbytes = algorithm.ComputeHash(thebytesofthing);

            string hex = "";
            foreach (byte x in sha256ifiedbytes) { hex += String.Format("{0:x2}", x); }

            return hex;

        }

        /// <summary>
        /// Guess what: it gives you a UNIX timestamp.
        /// </summary>
        /// <returns></returns>
        public int unix_timestamp()
        {
            TimeSpan unix_time = (System.DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (int)unix_time.TotalSeconds;
        }
    }
}
