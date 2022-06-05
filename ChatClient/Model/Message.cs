using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Model
{
    public class MessageModel
    {
        public string Sender { get; }
        public string Message { get; }

        public MessageModel(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }
}
