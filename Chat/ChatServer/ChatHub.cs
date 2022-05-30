using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Connections = new();

        public Task SendMessage(string user, string message)
        {
            return Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        public Task Enter(string user)
        {
            User _user = new User(user);
            Connections[_user.Name] = Context.ConnectionId;
            Serializers.UserSerializer.SerializeUser(_user);

            var deserializedDirectMessages = Serializers.DirectMessageSerializer.DeserializeMessage(user);
            foreach(var directMessage in deserializedDirectMessages)
            {
                Clients.Client(Connections[user]).SendAsync("ReceiveDirectMessage", directMessage.Name, directMessage.Message);
            }

            Console.WriteLine($"{user} is connected");

            return Clients.Others.SendAsync("ReceiveMessage", user, $"{user} is connected");
        }

        public async Task JoinGroup(string user, string groupName)
        {
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
            return Clients.Group(groupName).SendAsync("ReceiveMessageFromGroup", groupName, user, message);
        }

        public Task SendMessageToUser(string user, string message, string receiver)
        {
            Serializers.DirectMessageSerializer.SerializeMessage(new Serializers.DirectMessage(receiver, user, message));
            return Clients.Client(Connections[receiver]).SendAsync("ReceiveDirectMessage", user, message);
        }

    }
}
