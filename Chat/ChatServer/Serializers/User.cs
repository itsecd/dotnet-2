using System;

namespace ChatServer
{
    [Serializable]
    public class User
    {
        public string Name { get; set; }

        public User() { }

        public User(string name)
        {
            Name = name;
        }
    }
}
