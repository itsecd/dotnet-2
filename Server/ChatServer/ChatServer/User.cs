namespace ChatServer
{
    public class User
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public User() { }
        public User(string name, int id)
        {
            Name = name;
            ID = id;
        }
    }
}
