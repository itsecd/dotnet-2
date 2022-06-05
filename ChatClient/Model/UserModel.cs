using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Model
{
    public class UserModel
    {
        public string? Username { get; set; }

        public ObservableCollection<MessageModel>? Messages { get; set; } = new();

        public string? LastMessage
        {
            get
            {
                if (Messages != null && Messages.Count != 0)
                    return Messages.Last().Message;
                else
                    return " ";
            }
            set
            {
                LastMessage = value;
            }
        }

    }
}
