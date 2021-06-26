using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRMvcChat.Helpers
{
    public interface IStockCode
    {
        Task<string> ProcessCommand(string command);
    }
}
