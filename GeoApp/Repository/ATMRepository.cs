using GeoApp.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace GeoApp.Repository
{
    public class ATMRepository : IATMRepository
    {
        private const string StorageFileName = "ATMs.xml";

        private List<ATM> _ATMs;

        public ATM InsertATM(ATM atmToInsert)
        {
            ReadFromFile();
            var atm = _ATMs.Find(atm => atm.Id == atmToInsert.Id);
            if (atm != null)
                return null;
            _ATMs.Add(atmToInsert);
            WriteToFile();
            return atmToInsert;
        }

        public ATM GetATMById(string id)
        {
            ReadFromFile();
            var atm = _ATMs.Find(atm => atm.Id == id);
            if (atm != null)
                return atm;
            return null;
        }

        public ATM DeleteATMById(string id)
        {
            ReadFromFile();
            var deletedATM = _ATMs.Find(atm => atm.Id == id);
            _ATMs.RemoveAll(atm => atm.Id == id);
            WriteToFile();
            return deletedATM;
        }

        public ATM ChangeBalanceById(string id, int balance)
        {
            ReadFromFile();
            var atm = GetATMById(id);
            if (atm != null)
            {
                atm.Balance = balance;
                WriteToFile();
                return atm;
            }
            return null;            
        }

        public List<ATM> GetAllATMs()
        {
            ReadFromFile();
            return _ATMs;
        }

        private void ReadFromFile()
        {
            if (_ATMs != null)
                return;

            if (!File.Exists(StorageFileName))
            {
                _ATMs = new List<ATM>();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<ATM>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Open);
            _ATMs = (List<ATM>)xmlSerializer.Deserialize(fileStream);
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<ATM>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _ATMs);
        }
    }
}
