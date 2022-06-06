using System;

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
