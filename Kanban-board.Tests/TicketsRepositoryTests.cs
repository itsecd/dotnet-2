using Kanban_board.Model;
using Kanban_board.Repositories;
using System.Collections.Generic;
using Xunit;

namespace Kanban_board.Tests
{
    public class TicketsRepositoryTests
    {
        [Fact]
        public void GetTicketById()
        {
            var ticket = new Ticket
            {
                Id = "1",
                Title = "Сделать завтрак",
                Description = "Приготовить вкусный сытный завтрак",
                StatusId = "2",
            };
            TicketsRepository repository = new();
            repository.AddTicket(ticket);
            var returnedTicket = repository.GetTicketById("1");
            Assert.True(returnedTicket.Equals(ticket));
            Assert.Null(repository.GetTicketById("wrongId"));

            repository.DeleteTicket("1");
        }

        [Fact]
        public void GetTickets()
        {
            var ticket1 = new Ticket
            {
                Id = "1",
                Title = "2",
                Description = "3",
                StatusId = "4",
            };
            var ticket2 = new Ticket
            {
                Id = "5",
                Title = "6",
                Description = "7",
                StatusId = "8",
            };
            var ticket3 = new Ticket
            {
                Id = "9",
                Title = "10",
                Description = "11",
                StatusId = "12",
            };
            var addedTickets = new List<Ticket>
            {
                ticket1,
                ticket2,
                ticket3
            };
            TicketsRepository repository = new();
            repository.AddTicket(ticket1);
            repository.AddTicket(ticket2);
            repository.AddTicket(ticket3);
            var returnedTickets = repository.GetAllTickets();
            for (var i = 0; i < addedTickets.Count; i++)
            {
                Assert.True(returnedTickets[i].Equals(addedTickets[i]));
            }

            repository.DeleteTicket("1");
            repository.DeleteTicket("5");
            repository.DeleteTicket("9");
        }

        [Fact]
        public void EditTicket()
        {
            var ticket = new Ticket
            {
                Id = "1",
                Title = "2",
                Description = "3",
                StatusId = "yellow",
            };
            TicketsRepository repository = new();
            repository.AddTicket(ticket);
            var returnedTicket = repository.EditTicket(new Ticket
            {
                Id = "1",
                Title = "2",
                Description = "3",
                StatusId = "red",
            });
            ticket.StatusId = "red";
            Assert.True(returnedTicket.Equals(ticket));

            ticket.Id = "wrongId";
            Assert.Null(repository.EditTicket(ticket));

            repository.DeleteTicket("1");
        }

        [Fact]
        public void DeleteTicket()
        {
            var ticket = new Ticket
            {
                Id = "1",
                Title = "2",
                Description = "3",
                StatusId = "4",
            };
            TicketsRepository repository = new();
            repository.AddTicket(ticket);
            var returnedTicket = repository.DeleteTicket(ticket.Id);

            Assert.True(returnedTicket.Equals(ticket));
            Assert.Null(repository.DeleteTicket("wrongId"));

            repository.DeleteTicket("1");
        }

        [Fact]
        public void AddTicket()
        {
            var ticket = new Ticket
            {
                Id = "1",
                Title = "2",
                Description = "3",
                StatusId = "4",
            };
            TicketsRepository repository = new();
            var returnedTicket = repository.AddTicket(ticket);

            Assert.True(returnedTicket.Equals(ticket));
            Assert.Null(repository.AddTicket(ticket));

            repository.DeleteTicket("1");
        }
    }
}
