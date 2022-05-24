using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Serializers
{
    public class GroupMessage
    {
        public string Group { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public GroupMessage()
        {
            Group = "Unknown";
            Name = "Unknown";
            Message = "empty";
        }

        public GroupMessage(string group, string name, string message)
        {
            Group = group;
            Name = name;
            Message = message;
        }

#pragma warning disable CS0659
        public override bool Equals(object obj)
        {
            if (obj is not GroupMessage groupMessage) return false;
            return groupMessage.Group == Group && groupMessage.Name == Name && groupMessage.Message == Message;
        }
#pragma warning restore CS0659
    }
}
