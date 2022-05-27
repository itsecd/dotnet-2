using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Gomoku;

using Grpc.Core;

using Microsoft.Extensions.Configuration;

namespace GomokuServer.Services
{
    public sealed class GamingServer
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
        private readonly ConcurrentDictionary<string, Player> _players = new();
        private readonly object _waitingPlayerLock = new();
        private Player? _waitingPlayer;
        private readonly string _filePath;

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
                        case Request.RequestOneofCase.FindOpponentRequest:
                            FindOpponent(player);
                            break;
                        case Request.RequestOneofCase.MakeTurnRequest:
                            player.Session?.MakeTurn(player, requestStream.Current.MakeTurnRequest);
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

        private void FindOpponent(Player player)
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
        }

        private async Task<Player> GetPlayerFromFile(String login, IServerStreamWriter<Reply> responseStream)
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("File " + _filePath + " not found, create new Player");
                return new Player(login, responseStream);
            }
            Player? player;
            await SemaphoreSlim.WaitAsync();
            try
            {
                using var fileReader = new StreamReader(_filePath);
                string jsonString = fileReader.ReadToEnd();
                player = (JsonSerializer.Deserialize<List<Player>>(jsonString) ?? throw new InvalidOperationException()).Find(x => x.Login == login);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
            if (player is null)
            {
                //Console.WriteLine("Create new Player");
                return new Player(login, responseStream);
            }
            //Console.WriteLine("Open older Player");
            return new Player(player.Login, player.CountGames, player.CountWinGames, responseStream);
        }

        private async void SavePlayerToFile(Player player)
        {
            List<Player> players = new();
            if (File.Exists(_filePath))
            {
                using var fileReader = new StreamReader(_filePath);
                string jsonString = fileReader.ReadToEnd();
                players = JsonSerializer.Deserialize<List<Player>>(jsonString) ?? throw new InvalidOperationException();
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

            await SemaphoreSlim.WaitAsync();
            try
            {
                using FileStream createStream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(createStream, players);
                await createStream.DisposeAsync();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}

