using RestSharp;
using RestSharp.Extensions;
using SignalRMvcChat.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SignalRMvcChat.Helpers
{
    public class HttpClient : IHttpClient
    {
        /// <summary>
        /// Method that get the content of the url
        /// </summary>
        /// <param name="url">Url to call and get the result = Example https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv </param>
        /// <returns>The path of the file with the content of the downloaded file/</returns>
        public async Task<string> Get(string url)
        {
            try
            {
                // 1- Get the data
                var client = new RestClient(url);

                var request = new RestRequest(Method.GET);
                var response = await client.ExecuteGetAsync(request);

                // 2- Save the csv temporal file 
                // Save csv in same folder heirarchy as Lean
                var fullPath = SaveFile(response.RawBytes);

                // 3- return the path of the string.
                return fullPath;
            }
            catch(Exception e)
            {
                return Statics.generalErrorMessagePattern;
            }
        }

        /// <summary>
        /// Method that save an array of bytes into a path.
        /// </summary>
        /// <param name="bytes">Bytes to be save.</param>
        /// <returns></returns>
        public string SaveFile(byte[] bytes)
        {
            try
            {
                // 2- Save the csv temporal file 
                // Save csv in same folder heirarchy as Lean
                var fullPath = String.Format("{0}{1}", Statics.temporalPath, Statics.fileName);

                // Make sure the directory exist before writing
                (new FileInfo(Statics.temporalPath)).Directory.Create();

                // client.DownloadData(request).SaveAs(fullPath);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                return fullPath;
            }
            catch(Exception e)
            {
                return Statics.generalErrorMessagePattern;
            }
        }
    }
}