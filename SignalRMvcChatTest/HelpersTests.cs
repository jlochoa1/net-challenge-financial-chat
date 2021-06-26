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
                var stockCommand = "/stock=aapl.us";
                var result = stockCode.ProcessCommand(stockCommand);

                Assert.AreEqual(result, "Done");
            }
            catch(Exception e)
            {
                var error = e.Message;
            }
        }
    }
}
