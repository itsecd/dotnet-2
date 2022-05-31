using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using SudokuServer.Models;

namespace SudokuServer.Repositories
{
    public class JsonPlayersRepository : IPlayersRepository
    {
        private const string _filePath = "players.json";
        private readonly Mutex _mutex = new();

        public async Task<bool> AddPlayer (Player player)
        {
            _mutex.WaitOne();
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
                _mutex.ReleaseMutex();
            }
        }

        public async Task<Player?> GetPlayer(string login)
        {            
            _mutex.WaitOne();
            try
            {
                var players = await ReadFile();
                return players.Find(p => p.Login == login);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }            
        }

        public async Task RemovePlayer(string login)
        {
            _mutex.WaitOne();
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
                _mutex.ReleaseMutex();
            }           
        }

        private static async Task<List<Player>> ReadFile()
        {
            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
            string jsonString = await File.ReadAllTextAsync(_filePath);
            var players = jsonString.Length !=0 ? JsonSerializer.Deserialize<List<Player>>(jsonString) : new List<Player>();
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
