using GeoAppATM.Model;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace GeoAppATM.Repository
{
    public class AtmRepository : IAtmRepository
    {
        private static readonly object Locker = new();
        private List<AtmBalance> _atmBalances = new();
        private readonly List<Atm> _atms = new();
        private readonly string _atmBalanceStorageFileName;
        public AtmRepository(IConfiguration configuration)
        {
            var atmStorageFileName = configuration.GetSection("AtmFile").Value;
            _atmBalanceStorageFileName = configuration.GetSection("AtmBalanceFile").Value;

            var serializer = GeoJsonSerializer.Create();
            using var reader = File.OpenText(atmStorageFileName);
            using var jsonReader = new JsonTextReader(reader);
            var geoJsonFeatures = serializer.Deserialize<FeatureCollection>(jsonReader);
            foreach (var feature in geoJsonFeatures)
            {
                var point = (Point)feature.Geometry;
                string atmName = null;
                if (feature.Attributes.Exists("name"))
                {
                    atmName = feature.Attributes["name"].ToString();
                }
                else if (feature.Attributes.Exists("operator"))
                {
                    atmName = feature.Attributes["operator"].ToString();
                }
                _atms.Add(new Atm
                {
                    Id = feature.Attributes["id"].ToString(),
                    Name = atmName,
                    Latitude = point.X,
                    Longitude = point.Y,
                    Balance = 0
                }
                );
            }
        }

        public AtmRepository() { }

        private void ReadFromFile()
        {
            if (!File.Exists(_atmBalanceStorageFileName))
                _atmBalances = new List<AtmBalance>();
            else
            {
                using var fileReader = new StreamReader(_atmBalanceStorageFileName);
                var jsonString = fileReader.ReadToEnd();
                _atmBalances = System.Text.Json.JsonSerializer.Deserialize<List<AtmBalance>>(jsonString);
            }
            if (_atmBalances == null)
            {
                return;
            }
            if (_atmBalances.Count == 0)
            {
                foreach (var atms in _atms)
                    _atmBalances.Add(new AtmBalance { Id = atms.Id, Balance = atms.Balance });
                return;
            }
            foreach (var atmBalance in _atmBalances)
            {
                var atm = _atms.Find(atm => atm.Id == atmBalance.Id);
                if (atm != null)
                {
                    atm.Balance = atmBalance.Balance;
                }
            }
        }
        private void WriteToFile()
        {
            var jsonString = System.Text.Json.JsonSerializer.Serialize(_atmBalances);
            using var fileWriter = new StreamWriter(_atmBalanceStorageFileName);
            fileWriter.Write(jsonString);
        }
        public Atm ChangeBalanceById(string id, int balance)
        {
            lock (Locker)
            {
                ReadFromFile();
                var atm = GetAtmById(id);
                if (atm != null)
                {
                    atm.Balance = balance;
                    var tmpAtm = _atmBalances.Find(atmBalance => atmBalance.Id == atm.Id);
                    if (tmpAtm != null)
                    {
                        tmpAtm.Balance = balance;
                    }
                    WriteToFile();
                    return atm;
                }
                return null;
            }
        }
        public Atm GetAtmById(string id)
        {
            ReadFromFile();
            return _atms.Find(atm => atm.Id == id);
        }

        public List<Atm> GetAtms()
        {
            ReadFromFile();
            return _atms;
        }
    }
}
