using System.Collections.Generic;

namespace ChatServer.Serializers
{
    public class GroupList
    {
        public string GroupName { get; set; }

        public string Name { get; set; }

        public GroupList() { }

        public GroupList(string name, string groupName)
        {
            GroupName = groupName;
            Name = name;
        }
    }
}
