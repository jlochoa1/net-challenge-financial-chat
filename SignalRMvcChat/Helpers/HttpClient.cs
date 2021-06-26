using RestSharp;
using RestSharp.Extensions;
using SignalRMvcChat.Common;
using System;
using System.IO;

namespace SignalRMvcChat.Helpers
{
    public class HttpClient : IHttpClient
    {  
        [Obsolete]
        public string Get(string url)
        {
            try
            {
                // 1- Get the data
                var client = new RestClient(url);

                var request = new RestRequest(Method.GET);
                var response = client.Get(request);

                // 2- Save the csv temporal file 
                // Save csv in same folder heirarchy as Lean
                var fullPath = String.Format("{0}{1}", Statics.temporalPath, Statics.fileName);

                // Make sure the directory exist before writing
                (new FileInfo(Statics.temporalPath)).Directory.Create();

                client.DownloadData(request).SaveAs(fullPath);

                // 3- return the path of the string.
                return fullPath;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}