using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRMvcChat.Helpers
{
    public interface IStockCode
    {
        string ProcessCommand(string command);

        byte[] DownloadFile(string url);
    }
}
