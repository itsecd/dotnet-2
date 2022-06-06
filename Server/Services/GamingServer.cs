using Grpc.Core;
using Snake;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SnakeServer.Services
{
    public sealed class GamingServer
    {
        private const string XmlStorageFileName = "ATMs.xml";
        private readonly SemaphoreSlim _databaseLock = new(1, 1);
        private readonly ConcurrentDictionary<string, Player> _players = new();
        private List<Player> players = new List<Player>();

        public GamingServer() { }
/*        public GamingServer(IConfiguration config)
        {
            _filePath = config.GetValue<string>("PathPlayers");
        }*/

        public async Task Play(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Reply> responseStream)
        {
            if (!await requestStream.MoveNext())
                return;

            if (requestStream.Current.RequestCase != Request.RequestOneofCase.LoginRequest)
                return;

            var player = Login(requestStream.Current.LoginRequest, responseStream);
            try
            {
                var loginReply = new LoginReply { Success = player is not null };
                await responseStream.WriteAsync(new Reply { LoginReply = loginReply });
                if (player is null)
                    return;


                while (await requestStream.MoveNext())
                {
                    switch (requestStream.Current.RequestCase)
                    {
                        case Request.RequestOneofCase.None:
                            throw new ApplicationException();
                        case Request.RequestOneofCase.LoginRequest:
                            throw new ApplicationException();
                        case Request.RequestOneofCase.SendResultGame:
                            SendResult(player,requestStream.Current.SendResultGame);
                            break;
                        default: throw new ApplicationException();
                    }

                }
            }
            finally
            {
                if (player is not null)
                {
                    SavePlayerToFile(player);
                    _players.TryRemove(player.Login, out _);
                }
            }
        }

        private Player? Login(LoginRequest loginRequest, IServerStreamWriter<Reply> responseStream)
        {
            var player = GetPlayerFromFile(loginRequest.Login, responseStream).Result;
            return _players.TryAdd(player.Login, player) ? player : null;
        }

        private void SendResult(Player player, SendResultGame sendResultGame)
        {
            player.Score = sendResultGame.Score;
            _players.TryAdd(player.Login, player);
            WriteToFile();
        }
        

        private void ReadFromFile()
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

        private async Task<Player> GetPlayerFromFile(String login, IServerStreamWriter<Reply> responseStream)
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

        private async void WriteToFile()
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
        private async void SavePlayerToFile(Player player)
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

