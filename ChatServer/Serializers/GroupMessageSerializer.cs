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
            //await JsonSerializer.SerializeAsync(fs, messages, typeof(List<GroupMessage>));
            await JsonSerializer.SerializeAsync(fs, messages);
        }

        public static async Task<List<GroupMessage>> DeserializeMessage(string groupName)
        {
            var fileName = "RoomDataBases/" + groupName + ".json";
            var deserializedMessages = new List<GroupMessage>();
            FileInfo fileInf = new FileInfo(fileName);
            if (fileInf.Exists)
            {
                await using var fs = new FileStream(fileName, FileMode.Open);
                deserializedMessages = await JsonSerializer.DeserializeAsync<List<GroupMessage>>(fs);
            }

            return deserializedMessages;
        }
    }
}
