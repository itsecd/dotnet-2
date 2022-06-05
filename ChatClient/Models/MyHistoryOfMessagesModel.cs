using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Models
{
    public class MyHistoryOfMessagesModel
    {
        public string User;
        public string Message;
        public DateTime data;
        public string FormatNameMessage { get => User + ">>>  " + Message + "    " + data.ToString(); }
    }
}
