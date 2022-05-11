using GeoApp.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace GeoApp.Repository
{
    public class ATMRepository : IATMRepository
    {
        private const string XmlStorageFileName = "ATMs.xml";
        private const string JsonStorageFileName = "atm.geojson";

        private object _locker = new();

        private List<XmlATM> _xmlATMs;
        private List<JsonATM> _jsonATMs;

        public JsonATM GetATMById(string id)
        {
            ReadFromFile();
            return _jsonATMs.Find(atm => atm.Properties.Id == id);
        }

        public JsonATM ChangeBalanceById(string id, int balance)
        {
            lock (_locker)
            {
                ReadFromFile();
                var atm = GetATMById(id);
                if (atm != null)
                {
                    atm.Properties.Balance = balance;
                    _xmlATMs.Find(xmlATM => xmlATM.Id == atm.Properties.Id).Balance = balance;
                    WriteToFile();
                    return atm;
                }
                return null;
            }       
        }

        public List<JsonATM> GetAllATMs()
        {
            ReadFromFile();
            return _jsonATMs;
        }

        private void ReadFromFile()
        {
            if (_jsonATMs != null)
                return;

            if (!File.Exists(XmlStorageFileName))
                _xmlATMs = new List<XmlATM>();
            else
            {
                var xmlSerializer = new XmlSerializer(typeof(List<XmlATM>));
                using var fileStream = new FileStream(XmlStorageFileName, FileMode.Open);
                _xmlATMs = (List<XmlATM>)xmlSerializer.Deserialize(fileStream);
            }

            var stringJsonATMs = File.ReadAllText(JsonStorageFileName);
            _jsonATMs = JsonSerializer.Deserialize<JsonATMList>(stringJsonATMs).ATMs;

            if (_xmlATMs.Count == 0)
            {
                foreach (JsonATM jsonATM in _jsonATMs)
                    _xmlATMs.Add(new XmlATM { Id = jsonATM.Properties.Id, Balance = jsonATM.Properties.Balance });
                return;
            }

            foreach (XmlATM xmlATM in _xmlATMs)
                _jsonATMs.Find(jsonATM => jsonATM.Properties.Id == xmlATM.Id).Properties.Balance = xmlATM.Balance;
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<XmlATM>));
            using var fileStream = new FileStream(XmlStorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _xmlATMs);
        }
    }
}
