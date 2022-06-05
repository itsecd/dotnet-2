using ChatServer.Serializers;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> Connections = new();

        public Task SendMessage(string user, string message)
        {
            return Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        public Task Enter(string user)
        {
            Connections[user] = Context.ConnectionId;

            UserSerializer.SerializeUser(new User(user));

            var deserializedDirectMessages = DirectMessageSerializer.DeserializeMessage(user);
            foreach (var directMessage in deserializedDirectMessages)
            {
                Clients.Client(Connections[user]).SendAsync("ReceiveDirectMessage", directMessage.Name, directMessage.Message);
            }

            return Clients.Others.SendAsync("ReceiveMessage", user, $"{user} is connected");
        }

        public async Task JoinGroup(string user, string groupName)
        {
            var deserializedGroupMessages = GroupMessageSerializer.DeserializeMessage(groupName);
            foreach (var groupMessage in deserializedGroupMessages)
            {
                await Clients.Client(Connections[user]).SendAsync("ReceiveMessageFromGroup", groupMessage.GroupName, groupMessage.Name, groupMessage.Message);
            }

            GroupList groupMember = new GroupList(groupName, user);
            GroupListSerializer.SerializeGroup(groupMember);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessageFromGroup", groupName, user, "has joined the group");
        }

        public async Task LeaveGroup(string user, string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessageFromGroup", groupName, user, "has left the group");
        }

        public Task SendMessageToGroup(string groupName, string user, string message)
        {
            GroupMessageSerializer.SerializeMessage(new GroupMessage(groupName, user, message));
            return Clients.Group(groupName).SendAsync("ReceiveMessageFromGroup", groupName, user, message);
        }

        public Task SendMessageToUser(string user, string message, string receiver)
        {
            DirectMessageSerializer.SerializeMessage(new DirectMessage(receiver, user, message));
            return Clients.Client(Connections[receiver]).SendAsync("ReceiveDirectMessage", user, message);
        }

    }
}
