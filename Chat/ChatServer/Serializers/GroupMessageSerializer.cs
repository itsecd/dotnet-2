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
            string storageFileName = "DataBases/GroupDataBases/" + groupMessage.GroupName + ".xml";

            messages.Add(groupMessage);

            var xmlSerializer = new XmlSerializer(typeof(List<GroupMessage>));
            using var fileStream = new FileStream(storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, messages);
        }

        public static List<GroupMessage> DeserializeMessage(string groupName)
        {
            var storageFileName = "DataBases/GroupDataBases/" + groupName + ".xml";
            var deserializeMessages = new List<GroupMessage>();

            if (File.Exists(storageFileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<GroupMessage>));
                using var fileStream = new FileStream(storageFileName, FileMode.Open);
                deserializeMessages = (List<GroupMessage>)xmlSerializer.Deserialize(fileStream);
                return deserializeMessages;
            }
            return deserializeMessages;
        }

    }
}
