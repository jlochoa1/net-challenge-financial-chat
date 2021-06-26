using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMvcChat.Helpers
{
    public class HttpClient : IHttpClient
    {
        public string Get(string url)
        {
            try
            {
                var client = new RestClient(url);
                // client.Authenticator = new HttpBasicAuthenticator("username", "password");

                var request = new RestRequest();
                var response = client.Get(request);

                return "Done";
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }
    }
}