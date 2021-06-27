using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMvcChat.Helpers.RabbitMqHelpers
{
    public class Receiver : IReceiver
    {
        public async Task<string> ReceiveMessageAsync()
        {
            try
            {
                var resultMessage = string.Empty;
                var factory = new ConnectionFactory() { HostName = "localhost" };
                string queueName = "chatqueue";
                var rabbitMqConnection = factory.CreateConnection();
                var rabbitMqChannel = rabbitMqConnection.CreateModel();

                rabbitMqChannel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                rabbitMqChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                int messageCount = Convert.ToInt16(rabbitMqChannel.MessageCount(queueName));
                Console.WriteLine(" Listening to the queue. This channels has {0} messages on the queue", messageCount);

                var consumer = new AsyncEventingBasicConsumer(rabbitMqChannel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    resultMessage += message + "\n";
                    Console.WriteLine(" Location received: " + message);
                    rabbitMqChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                rabbitMqChannel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);

                return resultMessage;
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
        public async Task<string> Receive()
        {
            try
            {
                var resultMessage = await ReceiveMessageAsync();
                return resultMessage;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return string.Empty;
            }
        }
    }
}