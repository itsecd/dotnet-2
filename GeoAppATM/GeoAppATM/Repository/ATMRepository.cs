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
        private List<Atm> _atms = new();
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
        private void ReadFromFile()
        {
            //if (_atms != null)
            //    return;
            if (!File.Exists(_atmBalanceStorageFileName))
                _atmBalances = new List<AtmBalance>();
            else
            {
                using var fileReader = new StreamReader(_atmBalanceStorageFileName);
                string jsonString = fileReader.ReadToEnd();
                _atmBalances = System.Text.Json.JsonSerializer.Deserialize<List<AtmBalance>>(jsonString);
            }
            if (_atmBalances.Count == 0)
            {
                foreach (Atm atms in _atms)
                    _atmBalances.Add(new AtmBalance { Id = atms.Id, Balance = atms.Balance });
                return;
            }
            foreach (AtmBalance atmBalance in _atmBalances)
            {
                _atms.Find(atm => atm.Id == atmBalance.Id).Balance = atmBalance.Balance;
            }
        }
        private void WriteToFile()
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(_atmBalances);
            using var fileWriter = new StreamWriter(_atmBalanceStorageFileName);
            fileWriter.Write(jsonString);
        }
        public Atm ChangeBalanceByID(string id, int balance)
        {
            lock (Locker)
            {
                ReadFromFile();
                var atm = GetAtmByID(id);
                if (atm != null)
                {
                    atm.Balance = balance;
                    _atmBalances.Find(atmBalance => atmBalance.Id == atm.Id).Balance = balance;
                    WriteToFile();
                    return atm;
                }
                return null;
            }
        }
        public Atm GetAtmByID(string id)
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
