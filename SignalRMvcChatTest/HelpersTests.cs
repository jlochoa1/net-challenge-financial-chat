using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MockFiles;
using SignalRMvcChat.Common;
using SignalRMvcChat.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SignalRMvcChatTest
{
    [TestClass]
    public class HelpersTests
    {
        private readonly string _file1 = "aapl.us.csv";

        [TestMethod]
        public async Task ProcessCommandIsNotNullAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                var stockCode = new StockCode(httpClient); 
                var companyName = "aapl.us";
                var stockCommand = $"/stock={companyName}";
                var result = await stockCode.ProcessCommand(stockCommand);

                Assert.IsNotNull(result);
            }
            catch(Exception e)
            {
                var error = e.Message;
                throw e;
            }
        }

        [TestMethod]
        public async Task ProcessCommandCompanyDontExistAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                var stockCode = new StockCode(httpClient);
                var companyName = "test.cr";
                var stockCommand = $"/stock={companyName}";
                var result = await stockCode.ProcessCommand(stockCommand);
                var expectedResult = String.Format(Statics.companyNotFound, companyName);

                Assert.AreEqual(result, expectedResult);
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw e;
            }
        }

        [TestMethod]
        public async Task ProcessCommandBlankCompanyNameAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                var stockCode = new StockCode(httpClient);
                var companyName = "";
                var stockCommand = $"/stock={companyName}";
                var result = await stockCode.ProcessCommand(stockCommand);
                var expectedResult = String.Format(Statics.generalErrorMessagePattern, "company");

                Assert.AreEqual(result, expectedResult);
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw e;
            }
        }

        [TestMethod]
        public async Task ProcessCommandNotStockTokenAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                var stockCode = new StockCode(httpClient);
                var companyName = "";
                var stockCommand = $"/stoc={companyName}"; // is stock not stoc
                var result = await stockCode.ProcessCommand(stockCommand);
                var expectedResult = String.Format(Statics.generalErrorMessagePattern, "company");

                Assert.AreEqual(result, expectedResult);
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw e;
            }
        }

        [TestMethod]
        public async Task GetIsNotNullAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = "https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv";
                var result = await httpClient.Get(url);

                Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw e;
            }
        }

        [TestMethod]
        public async Task GetFailAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = "thisIsaTest.com";
                var companyName = "";
                var result = await httpClient.Get(url);
                var expectedResult = Statics.generalErrorMessagePattern;

                Assert.AreEqual(result, expectedResult);
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw e;
            }
        }

        [TestMethod]
        public void SaveIsNotNull()
        {
            try
            {
                var httpClient = new HttpClient();
                string path = Directory.GetCurrentDirectory();
                var fileName = _file1;
                var fullPath = $"{path}\\{fileName}";
                var bytes = File.ReadAllBytes(fullPath);
                var fullPathResult = httpClient.SaveFile(bytes);
                var expectedPath = $"{Statics.temporalPath}{Statics.fileName}";

                Assert.AreEqual(fullPathResult, expectedPath);
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw e;
            }
        }
    }
}
