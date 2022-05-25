using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ChatServer.Serializers
{
    public static class GroupMessageSerializer
    {
        public static async void SerializeMessage(GroupMessage groupMessage)
        {
            var fileName = "RoomDataBases/" + groupMessage.Group + ".json";
            var messages = await DeserializeMessage(groupMessage.Group);
            messages.Add(groupMessage);

            await using var fs = new FileStream(fileName, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, messages);
        }

        public static async Task<List<GroupMessage>> DeserializeMessage(string groupName)
        {
            var fileName = "RoomDataBases/" + groupName + ".json";
            await using var fs = new FileStream(fileName, FileMode.OpenOrCreate);
            var deserializedMessages = await JsonSerializer.DeserializeAsync<List<GroupMessage>>(fs);

            return deserializedMessages;
        }
    }
}
