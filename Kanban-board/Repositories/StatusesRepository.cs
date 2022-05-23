using Kanban_board.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Kanban_board.Repositories
{
    public class StatusesRepository : IStatusesRepository
    {
        private const string StorageFileName = "statuses.xml";

        private readonly object _locker = new();

        private List<Status> _statuses;

        public Status GetStatusById(string id)
        {
            ReadFromFile();
            return _statuses.Find(status => status.Id == id);
        }

        public Status AddStatus(Status status)
        {
            ReadFromFile();
            if (_statuses.FindIndex(s => s.Id == status.Id) != -1) return null;
            _statuses.Add(status);
            WriteToFile();
            return status;
        }

        public Status DeleteStatus(string id)
        {
            ReadFromFile();
            var statusToDeleteIndex = _statuses.FindIndex(t => t.Id == id);
            if (statusToDeleteIndex == -1) return null;
            var deletedStatus = _statuses[statusToDeleteIndex];
            _statuses.RemoveAt(statusToDeleteIndex);
            WriteToFile();
            return deletedStatus;
        }

        public Status EditStatus(Status newStatus)
        {
            lock (_locker)
            {
                ReadFromFile();
                var statusIndex = _statuses.FindIndex(status => status.Id == newStatus.Id);
                if (statusIndex != -1)
                {
                    _statuses[statusIndex] = newStatus;
                    WriteToFile();
                    return newStatus;
                }
                return null;
            }
        }

        public List<Status> GetAllStatuses()
        {
            ReadFromFile();
            return _statuses;
        }

        private void ReadFromFile()
        {
            if (_statuses != null)
                return;

            if (!File.Exists(StorageFileName))
            {
                _statuses = new List<Status>();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<Status>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Open);
            _statuses = (List<Status>)xmlSerializer.Deserialize(fileStream);
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Status>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _statuses);
        }
    }
}