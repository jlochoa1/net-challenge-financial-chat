using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SignalRMvcChat.Helpers
{
    public class StockCode : IStockCode
    {
        #region Fields

        private readonly string _stockPattern = "(/stock=\\.?)";
        private readonly string _apiUrlPattern = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";
        private HttpClient _httpClient;

        public StockCode(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion

        /// <summary>
        /// Download the file from the API url
        /// </summary>
        /// <param name="url">url of the Api to connect and download the file with the content of the company. For example: https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv </param >
        /// <returns></returns>
        public byte[] DownloadFile(string url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string ProcessCommand(string command)
        {
            try
            {
                // 1- Get the stock command of the command with the name of the company
                var companyName = Regex.Replace(command, _stockPattern, String.Empty);
                var url = String.Format(_apiUrlPattern, companyName);

                // 2- Download the File from the Api
                _httpClient.Get(url); ;

                // 3- Parse the file

                // 4- Return the string

                return "Done";
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }
    }
}