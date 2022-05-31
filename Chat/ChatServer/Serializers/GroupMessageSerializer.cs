using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public static class GroupMessageSerializer
    {
        public static void SerializeMessage(GroupMessage groupMessage)
        {
            var messages = DeserializeMessage(groupMessage.GroupName);
            string StorageFileName = "DataBases/GroupDataBases/" + groupMessage.GroupName + ".xml";

            messages.Add(groupMessage);

            var xmlSerializer = new XmlSerializer(typeof(List<GroupMessage>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, messages);
        }

        public static List<GroupMessage> DeserializeMessage(string groupName)
        {
            var StorageFileName = "DataBases/GroupDataBases/" + groupName + ".xml";
            var deserializeMessages = new List<GroupMessage>();

            if (File.Exists(StorageFileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<GroupMessage>));
                using var fileStream = new FileStream(StorageFileName, FileMode.Open);
                deserializeMessages = (List<GroupMessage>)xmlSerializer.Deserialize(fileStream);
                return deserializeMessages;
            }
            return deserializeMessages;
        }

    }
}
