using Lab2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Service
{
    public class PlayerSerializer
    {
        public List<Player> DeserializerPlayer()
        {
            var FileName = "Player.xml";
            var _player = new List<Player>();
            var xmlSerializer = new XmlSerializer(typeof(List<Player>));  
            using var fileStream = new FileStream(FileName, FileMode.Open);
            _player = (List<Player>)(xmlSerializer.Deserialize(fileStream) ?? throw new InvalidOperationException());
            return _player; 
        }

        public void SerializerPlayer(Player player)
        {
            var _player = DeserializerPlayer();
            var FileName = "Player.xml";
            if (player == null) return;
            if (_player.Any(players => player.UserName == players.UserName && player.PlayerPosition == players.PlayerPosition && player.ConnectionId == players.ConnectionId))
            {
                return;
            }
            _player!.Add(player);
            var xmlSerializer = new XmlSerializer(typeof(List<Player>));
            using var fileStream = new FileStream(FileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _player); 
        }
    }
}
