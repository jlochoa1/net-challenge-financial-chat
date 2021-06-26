using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRMvcChat.Common
{
    public class Statics
    {
        /// <summary>
        /// This is the pattern for the stock command. Example: "/stock=appl.us".
        /// </summary>
        public const string stockPattern = "(/stock=\\.?)";

        /// <summary>
        /// This is the result message for a command. Example: "APPL.US quote is $93.42 per share".
        /// </summary>
        public const string stockMessagePattern = "{0} quote is ${1} per share";

        /// <summary>
        /// The general error message if something goind wrong.
        /// </summary>
        public const string generalErrorMessagePattern = "An error ocurred trying to get the information of the {0}. Please contact your IT support.";

        /// <summary>
        /// The error message if the company wan't found or has no information.
        /// </summary>
        public const string companyNotFound = "{0} wasn't found into the stooq.com api. Please review your stock command.";

        /// <summary>
        /// The Api to make the httpClient call.
        /// </summary>
        public const string apiUrlPattern = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";

        /// <summary>
        /// The file path to store the temporal file.
        /// </summary>
        public const string temporalPath = @"C:\Windows\Temp\";

        /// <summary>
        /// The name of the temporal file with the company data.
        /// </summary>
        public const string fileName = "tempFile.csv";

        /// <summary>
        /// The name of the low column.
        /// </summary>
        public const string lowColumn = "Low";

        /// <summary>
        /// The name of the close column.
        /// </summary>
        public const string closeColumn = "Close";

        /// <summary>
        /// The name of the close column.
        /// </summary>
        public const string emptyColumn = "N/D";
    }
}
