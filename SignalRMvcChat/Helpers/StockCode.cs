using SignalRMvcChat.Common;
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
        private readonly string _stockMessagePattern = "{0} quote is ${1} per share";
        private readonly string _generalErrorMessagePattern = "An error ocurred trying to get the information of the {0}. Please contact your IT support.";
        private readonly string _companyNotFound = "{0} wan't found into the stooq.com api. Please review your stock command.";
        private readonly string _apiUrlPattern = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";
        private HttpClient _httpClient;
        private Statics _statics;

        public StockCode(HttpClient httpClient, Statics statics)
        {
            _httpClient = httpClient;
            _statics = statics;
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
                var resultMessage = string.Empty;
                // 1- Get the stock command of the command with the name of the company
                var companyName = Regex.Replace(command, _stockPattern, String.Empty);
                var url = String.Format(_apiUrlPattern, companyName);

                // 2- Download the File from the Api
                var filePath = _httpClient.Get(url);

                // 3- Parse the file. This is open the file and get the data inside
                resultMessage = ParseFile(filePath, companyName);

                // 4- Return the string

                return resultMessage;
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }

        private string ParseFile(string filePath, string companyName)
        {
            var resultMessage = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(filePath);

                    var columns = SplitData(lines.FirstOrDefault());
                    var data = SplitData(lines.LastOrDefault());
                    var dictionary = new Dictionary<string, string>();

                    for (int i = 0; i < columns.Length; i++)
                    {
                        dictionary.Add(columns[i], data[i]);
                    }

                    // As if there is a value for the quote.
                    var lowValueExist = !(dictionary.Where(x => x.Key.Equals("Low")).FirstOrDefault().Value).Equals("N/D");
                    var closeValueExist = !(dictionary.Where(x => x.Key.Equals("Close")).FirstOrDefault().Value).Equals("N/D");

                    // Return error message if company not found
                    if (lowValueExist || closeValueExist)
                        return String.Format(_companyNotFound, companyName);

                    var lowValue = Convert.ToDouble(dictionary.Where(x => x.Key.Equals("Low")).FirstOrDefault().Value);
                    var closeValue = Convert.ToDouble(dictionary.Where(x => x.Key.Equals("Close")).FirstOrDefault().Value);
                    var quoteValue = (lowValue + closeValue) / 2;

                    //The message that the bot need to return.
                    resultMessage = String.Format(_stockMessagePattern, companyName.ToUpper(), quoteValue);
                }
                else
                {
                    String.Format(_generalErrorMessagePattern, companyName);
                }
                return resultMessage;
            }
            catch(Exception e)
            {
                return String.Format(_generalErrorMessagePattern, companyName);
            }
        }

        private string[] SplitData(string line)
        {
            string[] columns = line.Split(',');// Where ',' the way that the csv is splited.
            return columns;
        }
    }
}