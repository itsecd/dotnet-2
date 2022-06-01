namespace ChatServer.Serializers
{
    public class User
    {
        public string Name { get; set; }

        public User() { }

        public User(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is not User user) return false;
            return user.Name == Name;
        }
    }
}
