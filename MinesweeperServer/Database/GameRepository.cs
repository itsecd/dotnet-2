using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MinesweeperServer.Database
{
    public class GameRepository : IGameRepository
    {
        private ConcurrentDictionary<string, Player> _players = new();
        private readonly IConfiguration _config;
        public Player this[string name] => _players[name];

        public GameRepository(IConfiguration config)
        {
            _config = config;
        }
        public bool TryAdd(string name) => _players.TryAdd(name, new Player());
        public void Load()
        {
            if (File.Exists(_config["PathPlayers"]))
            {
                string jsonString = File.ReadAllText(_config["PathPlayers"]);
                _players = JsonSerializer.Deserialize<ConcurrentDictionary<string, Player>>(jsonString);
            }
        }
        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize(_players, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText(_config["PathPlayers"], jsonString);
        }
        public async Task LoadAsync()
        {
            if (File.Exists(_config["pathPlayers"]))
            {
                using FileStream stream = File.Open(_config["pathPlayers"], FileMode.Open);
                _players = await JsonSerializer.DeserializeAsync<ConcurrentDictionary<string, Player>>(stream);
                await stream.DisposeAsync();
            }
        }
        public async Task DumpAsync()
        {
            using FileStream stream = File.Create(_config["pathPlayers"]);
            await JsonSerializer.SerializeAsync<ConcurrentDictionary<string, Player>>(stream, _players, new JsonSerializerOptions { WriteIndented = true });
            await stream.DisposeAsync();
        }
        public bool CalcScore(string name, string state)
        {
            switch (state)
            {
                case "win":
                    _players[name].TotalScore += int.Parse(_config["WinPoints"]);
                    _players[name].WinCount++;
                    _players[name].WinStreak++;
                    break;
                case "lose":
                    _players[name].TotalScore -= int.Parse(_config["LosePoints"]);
                    if (_players[name].TotalScore < 0)
                        _players[name].TotalScore = 0;
                    _players[name].LoseCount++;
                    _players[name].WinStreak = 0;
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}