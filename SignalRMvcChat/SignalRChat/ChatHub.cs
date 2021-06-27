using Microsoft.AspNet.SignalR;
using SignalRMvcChat.Common;
using SignalRMvcChat.Helpers;
using SignalRMvcChat.Helpers.RabbitMqHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMvcChat.SignalRChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string name, string message)
        {
            try
            {
                var lengthOfTheCommand = message.Split('=');

                // 1.1 Check if the command has the correct format
                if (message.Contains(Statics.stockFormat) &&
                    lengthOfTheCommand.Length == Statics.lengthOfTheCommand &&
                    !String.IsNullOrWhiteSpace(lengthOfTheCommand.LastOrDefault()))
                {
                    await ExecuteBot("Chat Bot", message);
                }
                else
                {
                    // Call the addNewMessageToPage method to update clients.
                    Clients.All.addNewMessageToPage(name, message);
                }
            }
            catch(Exception e)
            {
                var error = e.Message;
            }
        }

        public async Task ExecuteBot(string name, string message)
        {
            try
            {
                Publisher publisher = new Publisher();
                var resultPush = await publisher.Publish(message);

                Receiver receiver = new Receiver();
                var resultMessage = await receiver.Receive();

                // Call the addNewMessageToPage method to update clients.
                if(!String.IsNullOrEmpty(resultPush))
                    Clients.All.addNewMessageToPage(name, resultPush);
            }
            catch(Exception e)
            {
                var error = e.Message;
            }
        }
    }
}