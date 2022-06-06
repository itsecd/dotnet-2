using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using SudokuServer.Models;

namespace SudokuServer.Repositories
{
    public class JsonPlayersRepository : IPlayersRepository
    {
        private const string _filePath = "players.json";
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public async Task<bool> AddPlayer(Player player)
        {
            await _semaphore.WaitAsync();
            try
            {
                var players = await ReadFile();
                if (players.Contains(player))
                    return false;
                players.Add(player);
                WriteFile(players);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Player?> GetPlayer(string login)
        {
            await _semaphore.WaitAsync();
            try
            {
                var players = await ReadFile();
                return players.Find(p => p.Login == login);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public async Task<bool> UpdatePlayer(Player updatedPlayer)
        {
            await _semaphore.WaitAsync();
            try
            {
                var players = await ReadFile();
                var player = players.Find(p => p.Login == updatedPlayer.Login);
                if (player is null)
                    return false;
                players.Remove(player);
                players.Add(updatedPlayer);
                WriteFile(players);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public async Task RemovePlayer(string login)
        {
            await _semaphore.WaitAsync();
            try
            {
                var players = await ReadFile();
                var player = players.Find(p => p.Login == login);
                if (player is null)
                    return;
                players.Remove(player);
                WriteFile(players);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static async Task<List<Player>> ReadFile()
        {
            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
            string jsonString = await File.ReadAllTextAsync(_filePath);
            var players = jsonString.Length != 0 ? JsonSerializer.Deserialize<List<Player>>(jsonString) : new List<Player>();
            if (players is null)
                throw new IOException($"File {_filePath} cannot be read");
            return players;
        }

        private static async void WriteFile(List<Player> players)
        {
            using FileStream createStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(createStream, players);
            await createStream.DisposeAsync();
        }
    }
}
