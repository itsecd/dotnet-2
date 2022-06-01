using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public static class GroupListSerializer
    {
        public static void SerializeGroup(GroupList groupMember)
        {
            var groupList = DeserializeGroup(groupMember.GroupName);
            string storageFileName = "DataBases/GroupDataBases/" + groupMember.GroupName + ".List.xml";
            foreach (GroupList user in groupList)
            {
                if (user.Name == groupMember.GroupName) return;
            }

            groupList.Add(groupMember);

            var xmlSerializer = new XmlSerializer(typeof(List<GroupList>));
            using var fileStream = new FileStream(storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, groupList);
        }

        public static List<GroupList> DeserializeGroup(string groupName)
        {
            var storageFileName = "DataBases/GroupDataBases/" + groupName + ".List.xml";
            var deserializeGroup = new List<GroupList>();

            if (File.Exists(storageFileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<GroupList>));
                using var fileStream = new FileStream(storageFileName, FileMode.Open);
                deserializeGroup = (List<GroupList>)xmlSerializer.Deserialize(fileStream);
                return deserializeGroup;
            }
            return deserializeGroup;
        }
    }
}
