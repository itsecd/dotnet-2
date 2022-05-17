using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotServer.Enums;

namespace TelegramBotServer.Model
{
    public class CallbackData
    {
        public CallbackDataType Type { get; set; }

        public string Data { get; set; }
    }
}
