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
    }
}
