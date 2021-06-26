﻿using SignalRMvcChat.Common;
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

        private HttpClient _httpClient;

        #endregion

        #region Constructor
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
                var resultMessage = string.Empty;
                // 1- Get the stock command of the command with the name of the company
                var companyName = Regex.Replace(command, Statics.stockPattern, String.Empty);
                var url = String.Format(Statics.apiUrlPattern, companyName);

                // 2- Download the File from the Api
                var filePath = _httpClient.Get(url);

                // 3- Parse the file. This is open the file and get the data inside
                resultMessage = ParseFile(filePath, companyName);

                // 4- Return the string
                return resultMessage;
            }
            catch(Exception e)
            {
                return String.Format(Statics.generalErrorMessagePattern, "company");
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
                    if (CheckData(dictionary))
                    {
                        var lowValue = Convert.ToDouble(dictionary.Where(x => x.Key.Equals(Statics.lowColumn)).FirstOrDefault().Value);
                        var closeValue = Convert.ToDouble(dictionary.Where(x => x.Key.Equals(Statics.closeColumn)).FirstOrDefault().Value);
                        var quoteValue = (lowValue + closeValue) / 2;

                        //The message that the bot need to return.
                        resultMessage = String.Format(Statics.stockMessagePattern, companyName.ToUpper(), quoteValue);
                    }
                    else
                    {
                        return String.Format(Statics.companyNotFound, companyName);
                    }
                }
                else
                {
                    return String.Format(Statics.generalErrorMessagePattern, companyName);
                }
                return resultMessage;
            }
            catch(Exception e)
            {
                return String.Format(Statics.generalErrorMessagePattern, companyName);
            }
        }

        /// <summary>
        /// Method that valid if a dictionary have the correct data to continue.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private bool CheckData(Dictionary<string, string> dictionary)
        {
            try
            {
                // As if there is a value for the quote.
                var lowValueExist = !(dictionary.Where(x => x.Key.Equals(Statics.lowColumn)).FirstOrDefault().Value).Equals(Statics.emptyColumn);
                var closeValueExist = !(dictionary.Where(x => x.Key.Equals(Statics.closeColumn)).FirstOrDefault().Value).Equals(Statics.emptyColumn);

                // Return error message if company not found
                if (!lowValueExist || !closeValueExist)
                    return false;

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private string[] SplitData(string line)
        {
            string[] columns = line.Split(',');// Where ',' the way that the csv is splited.
            return columns;
        }
    }
}