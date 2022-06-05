namespace ChatServer.Serializers
{
    public class RoomList
    {
        public string Name { get; set; }

        public string Room {get;set;}

        public RoomList() { }

        public RoomList(string name, string room)
        {
            Name = name;
            Room = room;
        }
}
}
