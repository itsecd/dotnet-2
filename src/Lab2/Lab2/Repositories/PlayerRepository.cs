using Lab2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    public class PlayerRepository: IPlayerRepository
    {
        private const string FileName = "Player.xml";
        private List<Player> _players;

        public void ReadFileXml()
        {
            if (_players != null) return;
            if (!File.Exists(FileName))
            {
                _players = new List<Player>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Player>));
            using var fileStream = new FileStream(FileName, FileMode.Open);
            _players = (List<Player>)(xmlSerializer.Deserialize(fileStream));
        }

        public void WriteFileXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Player>));
            using var fileStream = new FileStream(FileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _players);
        }

        public void Add(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            ReadFileXml();
            _players.Add(player);
            WriteFileXml();
        }

        public void Clear()
        {
            ReadFileXml();
            _players.Clear();
            WriteFileXml();
        }

        public List<Player> GetPlayers()
        {
            ReadFileXml();
            return _players;
        }
       
        public void RemoveAt(int index)
        {
            ReadFileXml();
            _players.RemoveAt(index);
            WriteFileXml();
        }
    }
}