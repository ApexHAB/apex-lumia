using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows;
using System.Threading.Tasks;


namespace ApexLumia
{
    public class HTTPRequests
    {
        /// <summary>
        /// Async: Make an HTTP GET request to a URL (with no extra parameters)
        /// </summary>
        /// <param name="url">The URL you would like to send a request to.</param>
        /// <returns></returns>
        public static async Task<String> getRequest(String url)
        {
            String result = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = await request.GetResponseAsync();
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch { return ""; }

            return result;
        }

        /// <summary>
        /// Async: Make an HTTP PUT request to a URL with data
        /// </summary>
        /// <param name="url">The URL you would like to send a request to.</param>
        /// <param name="putData">The data you would like to send to the URL (e.g. JSON)</param>
        /// <returns></returns>
        public static async Task<String> putRequest(String url, String putData)
        {
            String result;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/json";
                byte[] putbytes = System.Text.Encoding.UTF8.GetBytes(putData);
                request.Headers[HttpRequestHeader.ContentLength] = putbytes.Length.ToString();

                Stream putStream = await request.GetRequestStreamAsync();
                putStream.Write(putbytes, 0, putbytes.Length);
                putStream.Close();

                WebResponse response = await request.GetResponseAsync();
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();


            }
            catch { return ""; }


            return result;

        }
    }
}
