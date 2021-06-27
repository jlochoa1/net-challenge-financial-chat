using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMvcChat.Helpers.RabbitMqHelpers
{
    public class Publisher : IPublisher
    {
        public static IStockCode _stockCode;

        public async Task<string> SendMessageAsync(string message)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = ConnectionFactory.DefaultUser,
                    Password = ConnectionFactory.DefaultPass,
                    Port = AmqpTcpEndpoint.UseDefaultPort
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "chatqueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var resultMessage = await _stockCode.ProcessCommand(message);

                    var body = Encoding.UTF8.GetBytes(resultMessage);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "chatqueue",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(resultMessage);
                    return resultMessage;
                }
            }
            catch(Exception e)
            {
                var error = e.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        public async Task<string> Publish(string message)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                _stockCode = new StockCode(httpClient);
                var messageResult = await SendMessageAsync(message);
                return messageResult;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return string.Empty;
            }
        }
    }
}