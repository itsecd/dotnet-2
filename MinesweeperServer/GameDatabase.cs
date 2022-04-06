using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Configuration;

namespace MinesweeperServer
{
    /// <summary>Класс для хранения игровых данных.</summary>
    public class GameDatabase
    {
        private readonly ConcurrentDictionary<string, User> _users = new();
        private ConcurrentDictionary<string, Player> _players = new();
        private readonly IConfiguration _config;

        public GameDatabase(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>Добавление игрока в список, если не найден.</summary>
        public bool TryAdd(string name)
        {
            if (_players.ContainsKey(name))
                return false;
            return _players.TryAdd(name, new Player());
        }
        /// <summary>Подключен ли игрок к комнате.</summary>
        public bool IsConnected(string name) => _users.ContainsKey(name);
        /// <summary>Присоединение игрока к комнате.</summary>
        public bool Join(string name, IServerStreamWriter<ServerMessage> channel) => _users.TryAdd(name, new User { Channel = channel, State = "lobby" });
        /// <summary>Отсоединение игрока от комнаты.</summary>
        public bool Leave(string name) => _users.TryRemove(name, out var s);
        /// <summary>Установить игрока в состояние готовности.</summary>
        public void Ready(string name) => _users[name].State = "ready";
        /// <summary>Загрузить список игроков из файла.</summary>
        public void Load()
        {
            if (File.Exists(_config["pathPlayers"]))
            {
                var jsonString = File.ReadAllText(_config["pathPlayers"]);
                _players = JsonSerializer.Deserialize<ConcurrentDictionary<string, Player>>(jsonString);
            }
        }
        /// <summary>Выгрузить список игроков в файл.</summary>
        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize(_players, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_config["pathPlayers"], jsonString);
        }
        /// <summary>Загрузить список игроков из файла (асинхронно).</summary>
        public async Task LoadAsync()
        {
            if (File.Exists(_config["pathPlayers"]))
            {
                using FileStream stream = File.Open(_config["pathPlayers"], FileMode.Open);
                _players = await JsonSerializer.DeserializeAsync<ConcurrentDictionary<string, Player>>(stream);
                await stream.DisposeAsync();
            }
        }
        /// <summary>Выгрузить список игроков в файл (асинхронно).</summary>
        public async Task DumpAsync()
        {
            using FileStream stream = File.Create(_config["pathPlayers"]);
            await JsonSerializer.SerializeAsync<ConcurrentDictionary<string, Player>>(stream, _players, new JsonSerializerOptions { WriteIndented = true });
            await stream.DisposeAsync();
        }
        /// <summary>Соответствует ли статус всех игроков определенному значению.</summary>
        public bool AllStates(string state)
        {
            foreach (var player in _users.Values)
            {
                if (player.State != state)
                    return false;
            }
            return true;
        }
        /// <summary>Выставление статусов игроков в соответствии с чьей-то победой.</summary>
        public void DeclareWin(string name)
        {
            foreach (var player in _users)
            {
                if (player.Key == name)
                    player.Value.State = "win";
                else
                    player.Value.State = "lose";
            }
        }
        /// <summary>Установить статус игрока.</summary>
        public void SetPlayerState(string name, string state)
        {
            _users[name].State = state;
        }
        /// <summary>Получить статус игрока.</summary>
        public string GetPlayerState(string name) => _users[name].State;
        /// <summary>Просчитать очки игрока и вернуть его в лобби.</summary>
        public void CalcScore(string name)
        {
            if (_users[name].State == "win")
            {
                _players[name].TotalScore += 10;
                _players[name].WinCount++;
                _players[name].WinStreak++;
            }
            if (_users[name].State == "lose")
            {
                _players[name].TotalScore -= 12;
                if (_players[name].TotalScore < 0)
                    _players[name].TotalScore = 0;
                _players[name].LoseCount++;
                _players[name].WinStreak = 0;
            }
            _users[name].State = "lobby";
            
        }
        /// <summary>Просчитать очки игроков и выставить всем статус "лобби".</summary>
        public void CalcScores()
        {
            foreach (var player in _users)
            {
                if (player.Value.State == "win")
                {
                    _players[player.Key].TotalScore += 10;
                    _players[player.Key].WinCount++;
                    _players[player.Key].WinStreak++;
                }
                if (player.Value.State == "lose")
                {
                    _players[player.Key].TotalScore -= 12;
                    if (_players[player.Key].TotalScore < 0)
                        _players[player.Key].TotalScore = 0;
                    _players[player.Key].LoseCount++;
                    _players[player.Key].WinStreak = 0;
                }
                player.Value.State = "lobby";
            }
        }
        /// <summary>Послать список игроков.</summary>
        public async Task SendPlayers(string name)
        {
            foreach (var player in _users.Where(x => x.Key != name))
            {
                await _users[name].Channel.WriteAsync(new ServerMessage { Text = player.Key, State = _users[player.Key].State });
            }
        }
        /// <summary>Разослать игрокам сообщение.</summary>
        public async Task Broadcast(ServerMessage message, string name = "")
        {
            foreach (var player in _users.Where(x => x.Key != name))
            {
                await _users[player.Key].Channel.WriteAsync(message);
            }
        }
    }
}