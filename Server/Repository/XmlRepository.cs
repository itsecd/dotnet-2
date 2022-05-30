using Grpc.Core;
using Microsoft.Extensions.Configuration;
using SnakeServer;
using SnakeServer.Database;
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
        private List<Player> _players;
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string _fileName;
        public XmlRepository()
        {
            _players = new List<Player>();
        }

        public XmlRepository(IConfiguration configuration)
        {
            _fileName = configuration.GetValue<string>("PathPlayers");
        }

        public async Task<Player> GetPlayerFromFile(string login, IServerStreamWriter<ServerMessage> responseStream)
        {
            if (!File.Exists(_fileName))
            {
                return new Player(login, responseStream);
            }
            Player? player;
            await SemaphoreSlim.WaitAsync();
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Player>));
                await using FileStream fileStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                player = ((List<Player>)formatter.Deserialize(fileStream) 
                    ?? throw new InvalidOperationException()).Find(x => x.Login == login);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
            if (player is null)
            {
                return new Player(login, responseStream);
            }
            return new Player(player.Login, responseStream);

        }
        private async Task ReadFileWithPlayersAsync()
        {
            if (!File.Exists(_fileName))
            {
                _players = new List<Player>();
                return;
            }
            await SemaphoreSlim.WaitAsync();
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Player>));
                await using FileStream fileStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                _players = (List<Player>)formatter.Deserialize(fileStream);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
        public async void SavePlayerToFile(Player player)
        {
            List<Player> players = new();
            await SemaphoreSlim.WaitAsync();
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Player>));
                if (File.Exists(_fileName))
                {
                    await using FileStream fileStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                    players = (List<Player>)formatter.Deserialize(fileStream);
                    var currentPlayer = players.Find(x => x.Login == player.Login);
                    if (players.Find(x => x.Login == player.Login) is null)
                    {
                        players.Add(player);
                    }
                    else
                    {
                        //
                    }
                }
                else
                {
                    players.Add(player);
                }
                using FileStream createStream = File.Create(_fileName);
              //  XmlSerializer formatter = new XmlSerializer(typeof(List<Player>));
                formatter.Serialize(createStream, players);
                await createStream.DisposeAsync();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
      


        public bool CompareResult()
        {
            throw new System.NotImplementedException();
        }

        Task IXmlRepository.ReadFileWithPlayersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
