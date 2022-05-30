using GeoAppATM.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GeoAppATM.Repository
{
    public class ATMRepository : IATMRepository
    {
        private const string GeoJsonStorageFileName = "atm.geojson";
        private const string JsonStorageFileName = "ATM.json";
        private static readonly object _locker = new();

        private List<JsonATM> _jsonATM;
        private List<GeoJsonATM> _geojsonATM;

        private void ReadFromFile()
        {
            if (_geojsonATM != null)
                return;
            if(!File.Exists(JsonStorageFileName))
                _jsonATM = new List<JsonATM>();
            else
            {
                using var fileReader = new StreamReader(JsonStorageFileName);
                string jsonString = fileReader.ReadToEnd();
                _jsonATM = JsonSerializer.Deserialize<List<JsonATM>>(jsonString);
            }

            var stringGeoJsonATM = File.ReadAllText(GeoJsonStorageFileName);
            _geojsonATM = JsonSerializer.Deserialize<GeoJsonATMList>(stringGeoJsonATM).ATMs;

            if(_jsonATM.Count == 0)
            {
                foreach (GeoJsonATM geoJsonATM in _geojsonATM)
                    _jsonATM.Add(new JsonATM { Id = geoJsonATM.Properties.Id, Balance = geoJsonATM.Properties.Balance });
                return;
            }
            foreach (JsonATM jsonATM in _jsonATM)
            {
                _geojsonATM.Find(geojsonATM => geojsonATM.Properties.Id == jsonATM.Id).Properties.Balance = jsonATM.Balance;
            }
        }
        private void WriteToFile()
        {
            string jsonString = JsonSerializer.Serialize(_jsonATM);
            using var fileWriter = new StreamWriter(JsonStorageFileName);
            fileWriter.Write(jsonString);
        }
        public GeoJsonATM ChangeBalanceByID(string id, int balance)
        {
            lock (_locker)
            {
                ReadFromFile();
                var atm = GetATMByID(id);
                if (atm != null)
                {
                    atm.Properties.Balance = balance;
                    _jsonATM.Find(jsonATM => jsonATM.Id == atm.Properties.Id).Balance = balance;
                    WriteToFile();
                    return atm;
                }
                return null;
            }
        }

        public List<GeoJsonATM> GetAllATM()
        {
            ReadFromFile();
            return _geojsonATM;
        }

        public GeoJsonATM GetATMByID(string id)
        {
            ReadFromFile();
            return _geojsonATM.Find(atm => atm.Properties.Id == id);
        }
    }
}
