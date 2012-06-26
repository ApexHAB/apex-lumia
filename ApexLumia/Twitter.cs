using System;
using System.Net;
using System.Windows;

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

        
        

        public Twitter(string user, string consumerkey, string consumersecret, string accesstoken, string accesssecret)
        {
            _username = user;
            _consumerkey = consumerkey;
            _consumersecret = consumersecret;
            _accesstoken = accesstoken;
            _accesssecret = accesssecret;            

        }

        public async void newStatus(string tweet)
        {

            string url = "https://api.twitter.com/1/statuses/update.json";
            string authorization = generateAuthorizationHeader(url, "POST", tweet);
            string content = string.Format("status={0}&include_entities=true",Utils.UrlEncodeRelaxed(tweet));

            string result = await HTTPRequests.postRequestAsync(url, content, authorization);
            System.Diagnostics.Debug.WriteLine(result);
        }

        public string generateAuthorizationHeader(string url, string method, string status = "")
        {
            string oauth_nonce = Utils.UrlEncodeRelaxed(Utils.uniqueAlphanumericString());
            string oauth_consumer_key = Utils.UrlEncodeRelaxed(_consumerkey);
            string oauth_timestamp = Utils.UrlEncodeRelaxed(Utils.unix_timestamp().ToString());
            string oauth_token = Utils.UrlEncodeRelaxed(_accesstoken);
            status = Utils.UrlEncodeRelaxed(status);
            url = Utils.UrlEncodeRelaxed(url);

            string parameterString = "";
            parameterString += "include_entities=true";
            parameterString += "&oauth_consumer_key=" + oauth_consumer_key;
            parameterString += "&oauth_nonce=" + oauth_nonce;
            parameterString += "&oauth_signature_method=HMAC-SHA1";
            parameterString += "&oauth_timestamp=" + oauth_timestamp;
            parameterString += "&oauth_token=" + oauth_token;
            parameterString += "&oauth_version=1.0";
            if (status != "") { parameterString += "&status=" + status; }

            string outputString = method + "&" + url + "&" + Utils.UrlEncodeRelaxed(parameterString);

            string signingKey = Utils.UrlEncodeRelaxed(_consumersecret) + "&" + Utils.UrlEncodeRelaxed(_accesssecret);

            string hmacsha1 = Utils.hmacsha1ify(outputString, signingKey);
            string signature = Utils.base64ify(hmacsha1);

            string header = "OAuth ";
            header += "oauth_consumer_key=\"" + oauth_consumer_key + "\", ";
            header += "oauth_nonce=\"" + oauth_nonce + "\", ";
            header += "oauth_signature=\"" + Utils.UrlEncodeRelaxed(signature) + "\", ";
            header += "oauth_signature_method=\"HMAC-SHA1\", ";
            header += "oauth_timestamp=\"" + oauth_timestamp + "\", ";
            header += "oauth_token=\"" + oauth_token + "\", ";
            header += "oauth_version=\"1.0\"";

            return header;
        }

        

    }
}
