using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grpc.Core;

using Microsoft.Extensions.Configuration;
using Snake;

namespace SnakeServer.Services
{
    public sealed class GamingServer
    {
        private const string XmlStorageFileName = "ATMs.xml";
        private readonly SemaphoreSlim _databaseLock = new(1, 1);
        private readonly ConcurrentDictionary<string, Player> _players = new();
        private readonly object _waitingPlayerLock = new();
        private Player? _waitingPlayer;
        private readonly string _filePath;
        private List<Player> players = new List<Player>();

        public GamingServer() { }
        public GamingServer(IConfiguration config)
        {
            _filePath = config.GetValue<string>("PathPlayers");
        }

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
                            SendResult(player,requestStream.Current.SendResultGame,responseStream);
                            break;
/*                        case Request.RequestOneofCase.FindOpponentRequest:
                            FindOpponent(player);
                            break;
                        case Request.RequestOneofCase.MakeTurnRequest:
                            player.Session?.MakeTurn(player, requestStream.Current.MakeTurnRequest);
                            break;*/
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

        private void SendResult(Player player, SendResultGame sendResultGame, IServerStreamWriter<Reply> responseStream)
        {
            //var igrok = GetPlayerFromFile(player.Login, responseStream).Result;
            player.Score = sendResultGame.Score;
            _players.TryAdd(player.Login, player);
            WriteToFile();
            


        }
        /*        private void FindOpponent(Player player)
                {
                    GamingSession session;
                    lock (_waitingPlayerLock)
                    {
                        if (_waitingPlayer is null)
                        {
                            _waitingPlayer = player;
                            return;
                        }

                        session = new GamingSession(_waitingPlayer, player);
                        _waitingPlayer = null;
                    }
                    session.Start();
                }*/

        /*        private async Task<Player> GetPlayerFromFile1(String login, IServerStreamWriter<Reply> responseStream)
                {
                    if (!File.Exists(_filePath))
                    {
                        return new Player(login, responseStream);
                    }
                    Player? player;
                    await _databaseLock.WaitAsync();
                    try
                    {
                        using var fileReader = new StreamReader(_filePath);
                        string jsonString = fileReader.ReadToEnd();
                        player = (JsonSerializer.Deserialize<List<Player>>(jsonString)
                            ?? throw new InvalidOperationException()).Find(x => x.Login == login);
                    }
                    finally
                    {
                        _databaseLock.Release();
                    }
                    if (player is null)
                    {
                        return new Player(login, responseStream);
                    }
                    return new Player(player.Login, player.CountGames, player.CountWinGames, responseStream);

                }

                private async void SavePlayerToFile1(Player player)
                {
                    //List<Player> players = new();
                    await _databaseLock.WaitAsync();
                    try
                    {

                        if (File.Exists(_filePath))
                        {
                            XmlSerializer formatter = new XmlSerializer(typeof(List<Player>));
                            *//*using var fileReader = new StreamReader(_filePath);
                            string jsonString = fileReader.ReadToEnd();
                            players = JsonSerializer.Deserialize<List<Player>>(jsonString) ?? throw new InvalidOperationException();
                            var currentPlayer = players.Find(x => x.Login == player.Login);*//*
                            await using FileStream fileStream = new FileStream(_filePath, FileMode.OpenOrCreate);
                            players = (List<Player>)formatter.Deserialize(fileStream);
                            var currentPlayer = players.Find(x => x.Login == player.Login);
                            if (players.Find(x => x.Login == player.Login) is null)
                            {
                                player.CountGames++;
                                players.Add(player);
                            }
                            else
                            {
                                currentPlayer!.CountGames++;
                                if (player.CountWinGames != currentPlayer.CountWinGames)
                                    currentPlayer.CountWinGames++;
                            }
                        }
                        else
                        {

                            players.Add(player);
                        }
                        //using FileStream createStream = File.Create(_filePath);
                        *//*                await JsonSerializer.SerializeAsync(createStream, players);*//*
                        var xmlSerializer = new XmlSerializer(typeof(List<Player>));
                        using var fileStream1 = new FileStream(_filePath, FileMode.Create);
                        xmlSerializer.Serialize(fileStream1, players);

                        //await createStream.DisposeAsync();
                    }
                    finally
                    {
                        _databaseLock.Release();
                    }
                }*/


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

