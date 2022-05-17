using Lab2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    public class PlayerRepository: IPlayerRepository
    {
        private const string FileName = "Player.xml";
        private List<Player>? _players;

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
            _players = (List<Player>)(xmlSerializer.Deserialize(fileStream) ?? throw new InvalidOperationException());
        }

        public void WriteFileXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Player>));
            using var fileStream = new FileStream(FileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _players);
        }

        public string Add(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            ReadFileXml();
            _players!.Add(player);
            WriteFileXml();
            return player.UserName;
        }

        public void Clear()
        {
            ReadFileXml();
            _players!.Clear();
            WriteFileXml();
        }

        public List<Player> ListPlayers()
        {
            ReadFileXml();
            return _players!;
        }

        public string Remove(string name)
        {
            ReadFileXml();
            if (name == "") throw new Exception();
            var player = GetPlayer(name);
            if (player == null) throw new Exception();
            _players!.Remove(player);
            WriteFileXml();
            return player.UserName;
        }

        public Player GetPlayer(string name)
        {
            ReadFileXml();
            var player = _players!.FirstOrDefault(player => player.UserName == name);
            if (player is null) throw new DirectoryNotFoundException();
            return player; 
        }
    }
}