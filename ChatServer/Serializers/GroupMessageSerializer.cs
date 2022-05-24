using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ChatServer.Serializers
{
    public static class GroupMessageSerializer
    {
        public static void SerializeMessage(GroupMessage groupMessage)
        {
            var messages = DeserializeMessage(groupMessage.Group);

            messages.Add(groupMessage);
            string jsonString = JsonSerializer.Serialize(messages);
            File.WriteAllText("RoomDataBases/" + groupMessage.Group + ".json", jsonString);
        }

        public static List<GroupMessage> DeserializeMessage(string groupName)
        {
            var fileName = "RoomDataBases/" + groupName + ".json";
            var deserializedMessages = new List<GroupMessage>();
            if (File.Exists(fileName))
            {
                string toDeserialize = File.ReadAllText(fileName);
                if (toDeserialize.Length > 0)
                {
                    deserializedMessages = JsonSerializer.Deserialize<List<GroupMessage>>(toDeserialize);
                }
            }
            
            return deserializedMessages;
        }
    }
}
