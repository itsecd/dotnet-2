namespace ChatServer.Serializers
{
    public class DirectMessage
    {
        public string Receiver { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public DirectMessage()
        {
            Receiver = "Unknown";
            Name = "Unknown";
            Message = "empty";
        }

        public DirectMessage(string receiver, string name, string message)
        {
            Receiver = receiver;
            Name = name;
            Message = message;
        }

#pragma warning disable CS0659
        public override bool Equals(object obj)
        {
            if (obj is not DirectMessage directMessage) return false;
            return directMessage.Receiver == Receiver && directMessage.Name == Name && directMessage.Message == Message;
        }
#pragma warning restore CS0659
    }
}
