using Kanban_board.Model;
using Kanban_board.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Kanban_board.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("Ticket")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly ITicketsRepository _repository;

        public TicketController(ITicketsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Получить все задачи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Ticket> Get()
        {
            return _repository.GetAllTickets();
        }

        /// <summary>
        /// Получить задачу по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Ticket> Get(string id)
        {
            var ticket = _repository.GetTicketById(id);
            if (ticket != null)
            {
                return ticket;
            }
            return NotFound();
        }

        /// <summary>
        /// Добавить новую задачу
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Ticket> Post([FromBody] Ticket ticket)
        {
            var addedTicket = _repository.AddTicket(ticket);
            if (addedTicket != null)
            {
                return addedTicket;
            }
            return Conflict();
        }

        /// <summary>
        /// Редактировать существующую задачу
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult<Ticket> Put([FromBody] Ticket ticket)
        {
            var editedTicket = _repository.EditTicket(ticket);
            if (editedTicket != null) return editedTicket;
            return NotFound();
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<Ticket> Delete(string id)
        {
            var deletedTicket = _repository.DeleteTicket(id);
            if (deletedTicket != null) return deletedTicket;
            return NotFound();
        }
    }
}
