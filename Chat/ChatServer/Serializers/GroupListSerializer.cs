using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public class GroupListSerializer
    {
        public static void SerializeGroup(GroupList groupMember)
        {
            var groupList = DeserializeGroup(groupMember.GroupName);
            string StorageFileName = "DataBases/GroupDataBases/" + groupMember.GroupName + ".List.xml";
            foreach (GroupList user in groupList)
            {
                if (user.Name == groupMember.GroupName) return;
            }

            groupList.Add(groupMember);

            var xmlSerializer = new XmlSerializer(typeof(List<GroupList>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, groupList);
        }

        public static List<GroupList> DeserializeGroup(string groupName)
        {
            var StorageFileName = "DataBases/GroupDataBases/" + groupName + ".List.xml";
            var deserializeGroup = new List<GroupList>();

            if (File.Exists(StorageFileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<GroupList>));
                using var fileStream = new FileStream(StorageFileName, FileMode.Open);
                deserializeGroup = (List<GroupList>)xmlSerializer.Deserialize(fileStream);
                return deserializeGroup;
            }
            return deserializeGroup;
        }
    }
}
