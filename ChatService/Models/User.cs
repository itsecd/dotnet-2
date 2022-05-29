using Grpc.Core;

namespace ChatService.Models
{
    public class User
    {
        public string Name { get; set; }
        public IServerStreamWriter<Message> Connect { get; set; }

    }
}
