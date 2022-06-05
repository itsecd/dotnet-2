
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using SudokuServer.Models;

namespace SudokuServer.Services
{
    public class FieldGeneratorService
    {
        private readonly string _precompiledFile = "fields.json";

        public Field GenerateField()
        {



            string jsonString = File.ReadAllText(_precompiledFile);
            var fields = JsonSerializer.Deserialize<List<Field>>(jsonString);
            var tmp = fields[0];
            return tmp;
        }

    }
}
