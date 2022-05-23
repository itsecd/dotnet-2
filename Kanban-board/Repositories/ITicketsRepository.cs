using Kanban_board.Model;
using System.Collections.Generic;

namespace Kanban_board.Repositories
{
    public interface ITicketsRepository
    {
        Ticket AddTicket(Ticket ticket);
        Ticket DeleteTicket(string id);
        Ticket EditTicket(Ticket newTicket);
        List<Ticket> GetAllTickets();
        Ticket GetTicketById(string id);
    }
}