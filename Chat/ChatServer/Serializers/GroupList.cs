namespace ChatServer.Serializers
{
    public class GroupList
    {
        public string GroupName { get; set; }

        public string Name { get; set; }

        public GroupList() { }

        public GroupList(string groupName, string name)
        {
            GroupName = groupName;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is not GroupList groupList)
            {
                return false;
            }

            return groupList.GroupName == GroupName;
        }
    }
}
