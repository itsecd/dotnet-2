using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public static class DirectMessageSerializer
    {
        public static void SerializeMessage(DirectMessage directMessage)
        {
            var messages = DeserializeMessage(directMessage.Receiver);
            string StorageFileName = "DataBases/DirectDataBases/" + directMessage.Receiver + ".xml";

            messages.Add(directMessage);

            var xmlSerializer = new XmlSerializer(typeof(List<DirectMessage>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, messages);
        }

        public static List<DirectMessage> DeserializeMessage(string receiverName)
        {
            var StorageFileName = "DataBases/DirectDataBases/" + receiverName + ".xml";
            var deserializeMessages = new List<DirectMessage>();

            if (File.Exists(StorageFileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<DirectMessage>));
                using var fileStream = new FileStream(StorageFileName, FileMode.Open);
                deserializeMessages = (List<DirectMessage>)xmlSerializer.Deserialize(fileStream);
                return deserializeMessages;
            }
            return deserializeMessages;
        }
    }
}
