using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public static class RoomListSerializer
    {
        public static void SerializeRoomList(RoomList roomMember)
        {
            var roomList = DeserializeRoomList(roomMember.Name);
            string storageFileName = "DataBases/RoomDataBases";
            if (!Directory.Exists(storageFileName))
            {
                Directory.CreateDirectory(storageFileName);
            }
            storageFileName = "DataBases/RoomDataBases/" + roomMember.Name + "List.xml";

            roomList.Add(roomMember);

            var xmlSerializer = new XmlSerializer(typeof(List<RoomList>));
            using var fileStream = new FileStream(storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, roomList);
        }

        public static List<RoomList> DeserializeRoomList(string roomName)
        {
            var storageFileName = "DataBases/RoomDataBases/" + roomName + "List.xml";
            var deserializeRoomList = new List<RoomList>();

            if (File.Exists(storageFileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<RoomList>));
                using var fileStream = new FileStream(storageFileName, FileMode.Open);
                deserializeRoomList = (List<RoomList>)xmlSerializer.Deserialize(fileStream);
                return deserializeRoomList;
            }
            return deserializeRoomList;
        }
    }
}
