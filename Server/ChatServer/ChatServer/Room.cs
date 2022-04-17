using Grpc.Core;
using System;
using System.Collections.Concurrent;

namespace ChatServer
{
    struct Room
    {
        public ConcurrentDictionary<string, IServerStreamWriter<Message>> online;
        private ConcurrentBag<User> _users;
        private ConcurrentDictionary<DateTime, Message> _history;
    }
}
