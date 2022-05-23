using Kanban_board.Model;
using Kanban_board.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Kanban_board.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("Status")]
    [ApiController]
    public class StatusController : Controller
    {
        private readonly IStatusesRepository _repository;

        public StatusController(IStatusesRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Получить все статусы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Status> Get()
        {
            return _repository.GetAllStatuses();
        }

        /// <summary>
        /// Получить статус по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Status> Get(string id)
        {
            var status = _repository.GetStatusById(id);
            if (status != null)
            {
                return status;
            }
            return NotFound();
        }

        /// <summary>
        /// Добавить новый статус
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Status> Post([FromBody] Status status)
        {
            var addedStatus = _repository.AddStatus(status);
            if (addedStatus != null)
            {
                return addedStatus;
            }
            return Conflict();
        }

        /// <summary>
        /// Редактировать существующий статус
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult<Status> Put([FromBody] Status status)
        {
            var editedStatus = _repository.EditStatus(status);
            if (editedStatus != null) return editedStatus;
            return NotFound();
        }

        /// <summary>
        /// Удалить статус
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<Status> Delete(string id)
        {
            var deletedStatus = _repository.DeleteStatus(id);
            if (deletedStatus != null) return deletedStatus;
            return NotFound();
        }
    }
}
