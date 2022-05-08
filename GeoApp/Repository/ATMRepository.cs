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

        private List<XmlATM> _XmlATMs;
        private List<JsonATM> _JsonATMs;

        //public XmlATM InsertATM(XmlATM atmToInsert)
        //{
        //    ReadFromFile();
        //    var atm = _XmlATMs.Find(atm => atm.Id == atmToInsert.Id);
        //    if (atm != null)
        //        return null;
        //    _XmlATMs.Add(atmToInsert);
        //    WriteToFile();
        //    return atmToInsert;
        //}

        public JsonATM GetATMById(string id)
        {
            ReadFromFile();
            var atm = _JsonATMs.Find(atm => atm.Properties.Id == id);
            if (atm != null)
                return atm;
            return null;
        }

        //public XmlATM DeleteATMById(string id)
        //{
        //    ReadFromFile();
        //    var deletedATM = _XmlATMs.Find(atm => atm.Id == id);
        //    _XmlATMs.RemoveAll(atm => atm.Id == id);
        //    WriteToFile();
        //    return deletedATM;
        //}

        public JsonATM ChangeBalanceById(string id, int balance)
        {
            ReadFromFile();
            var atm = GetATMById(id);
            if (atm != null)
            {
                atm.Properties.Balance = balance;
                _XmlATMs.Find(xmlATM => xmlATM.Id == atm.Properties.Id).Balance = balance;
                WriteToFile();
                return atm;
            }
            return null;            
        }

        public List<JsonATM> GetAllATMs()
        {
            ReadFromFile();
            return _JsonATMs;
        }

        private void ReadFromFile()
        {
            if (_JsonATMs != null)
                return;

            if (!File.Exists(XmlStorageFileName))
                _XmlATMs = new List<XmlATM>();
            else
            {
                var xmlSerializer = new XmlSerializer(typeof(List<XmlATM>));
                using var fileStream = new FileStream(XmlStorageFileName, FileMode.Open);
                _XmlATMs = (List<XmlATM>)xmlSerializer.Deserialize(fileStream);
            }

            var stringJsonATMs = File.ReadAllText(JsonStorageFileName);
            _JsonATMs = JsonSerializer.Deserialize<JsonATMList>(stringJsonATMs).ATMs;

            if (_XmlATMs.Count == 0)
            {
                foreach (JsonATM jsonATM in _JsonATMs)
                    _XmlATMs.Add(new XmlATM { Id = jsonATM.Properties.Id, Balance = jsonATM.Properties.Balance });
                return;
            }

            foreach (XmlATM xmlATM in _XmlATMs)
                _JsonATMs.Find(jsonATM => jsonATM.Properties.Id == xmlATM.Id).Properties.Balance = xmlATM.Balance;
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<XmlATM>));
            using var fileStream = new FileStream(XmlStorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _XmlATMs);
        }
    }
}
