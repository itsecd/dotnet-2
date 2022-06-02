using Grpc.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RaceServer.Services
{
    public class GameService
    {

        private readonly ConcurrentDictionary<string, Player> _players = new();
        private readonly SemaphoreSlim _databaseLock = new(1, 1);
        private readonly string _filePath;
        private Player? _waitingPlayer;

        public GameService(IConfiguration config)
        {
            _filePath = config.GetValue<string>("PathPlayers");
        }

        public async Task Connect(
           IAsyncStreamReader<Request> requestStream,
           IServerStreamWriter<Event> responseStream)
        {
            if (!await requestStream.MoveNext())
                return;

            if (requestStream.Current.RequestCase != Request.RequestOneofCase.Login)
                return;

            var player = Login(requestStream.Current.Login, responseStream);
            try
            {
                var loginEvent = new LoginEvent { Success = player is not null };
                await responseStream.WriteAsync(new Event { Login = loginEvent });
                if (player is null)
                    return;

                bool run = true;
                while (run && await requestStream.MoveNext())
                {
                    var request = requestStream.Current;
                    switch (request.RequestCase)
                    {
                        case Request.RequestOneofCase.None:
                            throw new ApplicationException();
                        case Request.RequestOneofCase.Login:
                            throw new ApplicationException();
                        case Request.RequestOneofCase.FindOpponent:
                            FindOpponent(player);
                            break;
                        case Request.RequestOneofCase.WinGame:
                            player.CountWinGames++;
                            player.Session?.ResultEvent(player, request.WinGame);
                            break;
                        case Request.RequestOneofCase.ChangePosition:
                            player.Session?.ChangePositionEvent(player, request.ChangePosition);
                            break;
                        case Request.RequestOneofCase.CloseConnect:
                            run = false;
                            break;
                        default: throw new ApplicationException();
                    }

                }
            }
            finally
            {
                if (player is not null)
                {
                    try
                    {
                        SavePlayerToFile(player);
                    }
                    finally
                    {
                        _players.TryRemove(player.Login, out _);
                    }
                }
            }
        }

        private Player? Login(LoginRequest loginRequest, IServerStreamWriter<Event> responseStream)
        {
            var player = GetPlayerFromFile(loginRequest.Login, responseStream).Result;
            return _players.TryAdd(loginRequest.Login, player) ? player : null;
        }

        private void FindOpponent(Player player)
        {
            GamingSession session;
            if (_waitingPlayer is null)
            {
                _waitingPlayer = player;
                return;
            }

            session = new GamingSession(_waitingPlayer, player);
            _waitingPlayer = null;
            session.Start();
        }

        private async Task<Player> GetPlayerFromFile(String login, IServerStreamWriter<Event> responseStream)
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
                player = (JsonSerializer.Deserialize<List<Player>>(jsonString) ?? throw new InvalidOperationException()).Find(x => x.Login == login);
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

        private async void SavePlayerToFile(Player player)
        {
            List<Player> players = new();
            await _databaseLock.WaitAsync();
            try
            {
                if (File.Exists(_filePath))
                {
                    using var fileReader = new StreamReader(_filePath);
                    string jsonString = fileReader.ReadToEnd();
                    players = JsonSerializer.Deserialize<List<Player>>(jsonString) ?? throw new InvalidOperationException();
                    var currentPlayer = players.Find(x => x.Login == player.Login);
                    if (players.Find(x => x.Login == player.Login) is null)
                    {
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

                using FileStream createStream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(createStream, players);
                await createStream.DisposeAsync();
            }
            finally
            {
                _databaseLock.Release();
            }
        }
    }

}
