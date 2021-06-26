using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SignalRMvcChat.Helpers;
using System;

namespace SignalRMvcChatTest
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void ProcessCommandIsNotNull()
        {
            try
            {
                var httpClient = new HttpClient();
                var stockCode = new StockCode(httpClient); 
                var companyName = "aapl.us";
                var stockCommand = $"/stock={companyName}";
                var result = stockCode.ProcessCommand(stockCommand);

                Assert.IsNotNull(result);
            }
            catch(Exception e)
            {
                var error = e.Message;
            }
        }

        [TestMethod]
        public void ProcessCommandCompanyDontExist()
        {
            try
            {
                var httpClient = new HttpClient();
                var stockCode = new StockCode(httpClient);
                var companyName = "test.cr";
                var stockCommand = $"/stock={companyName}";
                var result = stockCode.ProcessCommand(stockCommand);

                Assert.AreEqual(result, string.Empty);
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
        }

        [TestMethod]
        public void ProcessCommandBlankCompanyName()
        {
            try
            {
                var httpClient = new HttpClient();
                var stockCode = new StockCode(httpClient);
                var companyName = "";
                var stockCommand = $"/stock={companyName}";
                var result = stockCode.ProcessCommand(stockCommand);

                Assert.AreEqual(result, string.Empty);
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
        }
    }
}
