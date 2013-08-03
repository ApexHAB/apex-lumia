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
        public static async Task<String> getRequestAsync(string url)
        {
            string result = "";

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
        public static async Task<String> putRequestAsync(string url, string putData)
        {
            string result;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
                byte[] putbytes = System.Text.Encoding.UTF8.GetBytes(putData);
                request.Headers[HttpRequestHeader.ContentLength] = putbytes.Length.ToString();

                Stream putStream = await request.GetRequestStreamAsync();
                await putStream.WriteAsync(putbytes, 0, putbytes.Length);
                putStream.Close();

                WebResponse response = await request.GetResponseAsync();
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();


            }
            catch { return ""; }


            return result;

        }

        public static async Task<String> postRequestAsync(string url, string postData, string authorization = "", bool Expect100Continue = true)
        {
            string result;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
               
                byte[] postbytes = System.Text.Encoding.UTF8.GetBytes(postData);
                request.Headers[HttpRequestHeader.ContentLength] = postbytes.Length.ToString();
                request.Headers[HttpRequestHeader.Authorization] = authorization;

                Stream postStream = await request.GetRequestStreamAsync();
                await postStream.WriteAsync(postbytes, 0, postbytes.Length);
                postStream.Close();

                WebResponse response = await request.GetResponseAsync();
                result = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

            }
            catch { return ""; }


            return result;
        }

    }
}
