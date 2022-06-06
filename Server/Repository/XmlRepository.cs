using Grpc.Core;
using Snake;
using SnakeServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server.Repository
{
    public class XmlRepository : IXmlRepository
    {
        private List<Player> players = new List<Player>();
        private const string XmlStorageFileName = "test.xml";
        private readonly SemaphoreSlim _databaseLock = new(1, 1);
        public void ReadFromFile()
        {
            if (!File.Exists(XmlStorageFileName))
                players = new List<Player>();
            else
            {
                var xmlSerializer = new XmlSerializer(typeof(List<Player>));
                using var fileStream = new FileStream(XmlStorageFileName, FileMode.Open);
                players = (List<Player>)xmlSerializer.Deserialize(fileStream);
            }
        }

        public async Task<Player> GetPlayerFromFile(String login, IServerStreamWriter<Reply> responseStream)
        {
            if (!File.Exists(XmlStorageFileName))
            {
                return new Player(login, responseStream);
            }
            await _databaseLock.WaitAsync();
            try
            {
                ReadFromFile();

            }
            finally { _databaseLock.Release(); }

            return players.Find(x => x.Login == login);

        }

        public async void WriteToFile()
        {
            await _databaseLock.WaitAsync();
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(List<Player>));
                using var fileStream = new FileStream(XmlStorageFileName, FileMode.Create);
                xmlSerializer.Serialize(fileStream, players);
            }
            finally
            {
                _databaseLock.Release();
            }

        }

        public async void SavePlayerToFile(Player player)
        {
            await _databaseLock.WaitAsync();
            try
            {
                if (File.Exists(XmlStorageFileName))
                {
                    ReadFromFile();
                    players.Add(player);
                }
                else
                {
                    players.Add(player);
                }
            }
            finally
            {
                _databaseLock.Release();
            }
            WriteToFile();
        }

    }
}
