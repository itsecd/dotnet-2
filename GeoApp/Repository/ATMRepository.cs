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

        public ATM InsertATM(ATM ATM)
        {
            ReadFromFile();
            foreach (ATM atm in _ATMs)
            {
                if (ATM.Id == atm.Id)
                    return null;
            }
            _ATMs.Add(ATM);
            WriteToFile();
            return ATM;
        }

        public ATM GetATMById(string id)
        {
            ReadFromFile();
            foreach (ATM ATM in _ATMs)
            {
                if (ATM.Id == id)
                    return ATM;
            }
            return null;
        }

        public ATM DeleteATMById(string id)
        {
            ReadFromFile();
            var DeletedATM = _ATMs.Find(ATM => ATM.Id == id);
            _ATMs.RemoveAll(ATM => ATM.Id == id);
            WriteToFile();
            return DeletedATM;
        }

        public ATM ChangeBalanceById(string id, int balance)
        {
            ReadFromFile();
            var tmp = GetATMById(id);
            if (tmp != null)
            {
                tmp.Balance = balance;
                WriteToFile();
                return tmp;
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
