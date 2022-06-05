using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using SudokuServer.Models;

namespace SudokuServer.Services
{
    public class FieldGeneratorService
    {
        private readonly string _precompiledFile = "fields.json";
        private readonly List<int> _generatedFildsId = new();

        public Field? GenerateField()
        {
            string jsonString = File.ReadAllText(_precompiledFile);
            var fields = JsonSerializer.Deserialize<List<Field>>(jsonString);
            if (fields is null)
                return null;
            else
            {
                if (_generatedFildsId.Count == fields.Count)
                    return null;
                var random = new Random();
                var field = fields[random.Next(fields.Count)];
                while (_generatedFildsId.Contains(field.Id))
                    field = fields[random.Next(fields.Count)];
                _generatedFildsId.Add(field.Id);
                return field;
            }
        }
    }
}
