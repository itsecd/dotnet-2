using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class ChatService : IChatService
    {
        private readonly Dictionary<string, string> _loginToId = new();
        private readonly Dictionary<string, string> _idToLogin = new();

        public bool LoginToIdContainsKey(string client)
        {
            if (_loginToId.ContainsKey(client))
                return true;
            return false;
        }

        public string[] LoginToIdArrayKeys()
        {
            return _loginToId.Keys.ToArray();
        }
        public void LoginToIdAdd(string login, string connectionId)
        {
            _loginToId.Add(login, connectionId);
        }
        public void IdToLoginAdd(string connectionId, string login)
        {
            _idToLogin.Add(connectionId, login);
        }
        public string ResolveCallerLogin(string connectionId)
        {
            return _idToLogin[connectionId];
        }
        public string ResolveConnectionId(string login)
        {
            return _loginToId[login];
        }
    }
}
