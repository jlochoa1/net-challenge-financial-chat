using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRMvcChat.Helpers.RabbitMqHelpers
{
    public interface IReceiver
    {
        Task<string> Receive();
    }
}
