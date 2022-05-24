using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SnakeServer.Database
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
        public bool TryAddPlayer(string name) => _players.TryAdd(name, new Player());
        public void ReadFile()// redo under xml
        {
            if (File.Exists(_config["PathPlayers"]))
            {
                string jsonString = File.ReadAllText(_config["PathPlayers"]);
                _players = JsonSerializer.Deserialize<ConcurrentDictionary<string, Player>>(jsonString);
            }
        }
        public async Task WriteFile()// redo under xml
        {
            await using FileStream stream = File.Create(_config["pathPlayers"]);
            await JsonSerializer.SerializeAsync<ConcurrentDictionary<string, Player>>(stream, _players, new JsonSerializerOptions { WriteIndented = true });
        }

    }
}