namespace ChatServer.Serializers
{
    public class GroupMessage
    {
        public string GroupName { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public GroupMessage() { }

        public GroupMessage(string groupName, string name, string message)
        {
            GroupName = groupName;
            Name = name;
            Message = message;
        }
        public override bool Equals(object obj)
        {
            if (obj is not GroupMessage groupMessage) return false;
            return groupMessage.GroupName == GroupName && groupMessage.Name == Name && groupMessage.Message == Message;
        }
    }
}
