using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRMvcChat.Helpers
{
    public interface IHttpClient
    {

        /// <summary>
        /// Method that get the content of the url
        /// </summary>
        /// <param name="url">Url to call and get the result = Example https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv </param>
        /// <returns>The path of the file with the content of the downloaded file/</returns>
        string Get(string url);
    }
}
