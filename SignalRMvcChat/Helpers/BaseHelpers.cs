using SignalRMvcChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMvcChat.Helpers
{
    public class BaseHelpers
    {
        private Statics _statics;

        public BaseHelpers(Statics statics)
        {
            _statics = statics;
    }
    }
}