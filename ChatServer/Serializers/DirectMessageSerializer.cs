using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ChatServer.Serializers
{
    public static class DirectMessageSerializer
    {
        public static void SerializeMessage(DirectMessage directMessage)
        {
            var messages = DeserializeMessage(directMessage.Receiver);

            messages.Add(directMessage);
            string jsonString = JsonSerializer.Serialize(messages);
            File.WriteAllText("DirectDataBases/" + directMessage.Receiver + ".json", jsonString);
        }

        public static List<DirectMessage> DeserializeMessage(string receiverName)
        {
            var fileName = "DirectDataBases/" + receiverName + ".json";
            var deserializedMessages = new List<DirectMessage>();
            if (File.Exists(fileName))
            {
                string toSerialize = File.ReadAllText(fileName);
                if (toSerialize.Length > 0)
                {
                    deserializedMessages = JsonSerializer.Deserialize<List<DirectMessage>>(toSerialize);
                }
            }

            return deserializedMessages;
        }
    }
}
