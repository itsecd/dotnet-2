using Kanban_board.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Kanban_board.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private const string StorageFileName = "tickets.xml";

        private readonly object _locker = new();

        private List<Ticket> _tickets;

        public Ticket GetTicketById(string id)
        {
            ReadFromFile();
            return _tickets.Find(ticket => ticket.Id == id);
        }

        public Ticket AddTicket(Ticket ticket)
        {
            ReadFromFile();
            if (_tickets.FindIndex(t => t.Id == ticket.Id) != -1) return null;
            _tickets.Add(ticket);
            WriteToFile();
            return ticket;
        }

        public Ticket DeleteTicket(string id)
        {
            ReadFromFile();
            var ticketToDeleteIndex = _tickets.FindIndex(t => t.Id == id);
            if (ticketToDeleteIndex == -1) return null;
            var deletedTicket = _tickets[ticketToDeleteIndex];
            _tickets.RemoveAt(ticketToDeleteIndex);
            WriteToFile();
            return deletedTicket;
        }

        public Ticket EditTicket(Ticket newTicket)
        {
            lock (_locker)
            {
                ReadFromFile();
                var ticketIndex = _tickets.FindIndex(ticket => ticket.Id == newTicket.Id);
                if (ticketIndex != -1)
                {
                    _tickets[ticketIndex] = newTicket;
                    WriteToFile();
                    return newTicket;
                }
                return null;
            }
        }

        public List<Ticket> GetAllTickets()
        {
            ReadFromFile();
            return _tickets;
        }

        private void ReadFromFile()
        {
            if (_tickets != null)
                return;

            if (!File.Exists(StorageFileName))
            {
                _tickets = new List<Ticket>();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<Ticket>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Open);
            _tickets = (List<Ticket>)xmlSerializer.Deserialize(fileStream);
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Ticket>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _tickets);
        }
    }
}