using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace ApexLumia
{
    public class Twitter
    {
        private bool _status = false;
        public bool status { get { return _status; } }

        private string _username;
        private string _consumerkey;
        private string _consumersecret;
        private string _accesstoken;
        private string _accesssecret;
        
        /// <summary>
        /// Constructor: Set important info.
        /// </summary>
        /// <param name="user">Twitter Username</param>
        /// <param name="consumerkey">Twitter App Consumer Key</param>
        /// <param name="consumersecret">Twitter App Consumer Secret</param>
        /// <param name="accesstoken">Twitter User Access Token</param>
        /// <param name="accesssecret">Twitter User Access Token Secret</param>
        public Twitter(string user, string consumerkey, string consumersecret, string accesstoken, string accesssecret)
        {
            _username = user;
            _consumerkey = consumerkey;
            _consumersecret = consumersecret;
            _accesstoken = accesstoken;
            _accesssecret = accesssecret;            

        }

        /// <summary>
        /// Async: Post a new tweet to the previously defined twitter account.
        /// </summary>
        /// <param name="tweet">The tweet to post. Max. 140 chars</param>
        /// <param name="latitude">Optional: latitude for geotagging</param>
        /// <param name="longitude">Optional: longitude for geotagging</param>
        public async void newStatusAsync(string tweet, string latitude = "", string longitude = "")
        {
            string url = "https://api.twitter.com/1/statuses/update.json";

            var parameters = new Dictionary<string, string>();
            parameters.Add("status",Utils.UrlEncodeRelaxed(tweet));
            parameters.Add("display_coordinates", "true");
            if (latitude != "") { parameters.Add("lat", Utils.UrlEncodeRelaxed(latitude)); }
            if (longitude != "") { parameters.Add("long", Utils.UrlEncodeRelaxed(longitude)); }

            string authorization = generateAuthorizationHeader(url, "POST", parameters);
            string content = string.Format("status={0}",Utils.UrlEncodeRelaxed(tweet));
            content += string.Format("&display_coordinates={0}", "true");
            if (latitude != "") { content += string.Format("&lat={0}", Utils.UrlEncodeRelaxed(latitude)); }
            if (longitude != "") { content += string.Format("&long={0}", Utils.UrlEncodeRelaxed(longitude)); }

            string result = await HTTPRequests.postRequestAsync(url, content, authorization);
            if (result == "") { _status = false; } else { _status = true; }
        }

        /// <summary>
        /// Generate the OAuth HTTP Authorization header for twitter requests
        /// </summary>
        /// <param name="url">The URL the request will be made to.</param>
        /// <param name="method">The method of the request.</param>
        /// <param name="otherparams">Any other parameters specific to the request.</param>
        /// <returns></returns>
        public string generateAuthorizationHeader(string url, string method, Dictionary<string, string> otherparams)
        {

            var parameters = new Dictionary<string, string>();
            parameters.Add("oauth_version", "1.0");
            parameters.Add("oauth_consumer_key", _consumerkey);
            parameters.Add("oauth_nonce", Utils.uniqueAlphanumericString());
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_timestamp", Utils.unix_timestamp().ToString());
            parameters.Add("oauth_token", _accesstoken);

            foreach (var item in otherparams)
            {
                parameters[item.Key] = item.Value;
            }

            parameters = parameters.OrderBy(x => x.Key).ToDictionary(v => v.Key, v => v.Value);
            

            string outputString = method + "&" + Utils.UrlEncodeRelaxed(url) + "&";
            foreach (var parameter in parameters)
            {
                outputString += Utils.UrlEncodeRelaxed(parameter.Key + "=" + parameter.Value + "&");
            }
            outputString = outputString.Substring(0, outputString.Length - 3);

            string signingKey = Utils.UrlEncodeRelaxed(_consumersecret) + "&" + Utils.UrlEncodeRelaxed(_accesssecret);
            byte[] hmacsha1 = Utils.hmacsha1ify(outputString, signingKey);
            string signature = Convert.ToBase64String(hmacsha1);

            string header = "OAuth ";
            header += "oauth_consumer_key=\"" + Utils.UrlEncodeRelaxed(parameters["oauth_consumer_key"]) + "\", ";
            header += "oauth_nonce=\"" + Utils.UrlEncodeRelaxed(parameters["oauth_nonce"]) + "\", ";
            header += "oauth_signature=\"" + Utils.UrlEncodeRelaxed(signature) + "\", ";
            header += "oauth_signature_method=\"HMAC-SHA1\", ";
            header += "oauth_timestamp=\"" + Utils.UrlEncodeRelaxed(parameters["oauth_timestamp"]) + "\", ";
            header += "oauth_token=\"" + Utils.UrlEncodeRelaxed(parameters["oauth_token"]) + "\", ";
            header += "oauth_version=\"1.0\"";

            return header;
        }

        

    }
}
