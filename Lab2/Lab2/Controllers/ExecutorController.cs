using Lab2.Models;
using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Lab2.Controllers
{
    /// <summary>
    /// Контроллер для исполнителей задач
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutorController : ControllerBase
    {
        /// <summary>
        /// Репозиторий исполнителей задач
        /// </summary>
        private readonly IExecutorRepository _executorRepository;

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        public ExecutorController(IExecutorRepository executorRepository)
        {
            _executorRepository = executorRepository;
        }

        /// <summary>
        /// Получение всех исполнителей задач
        /// </summary>
        /// <returns>All Executors</returns>
        [HttpGet]
        public ActionResult<List<Executor>> Get() => _executorRepository.GetExecutors();

        /// <summary>
        /// Получение исполнителя задачи по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Исполнитель задач</returns>
        [HttpGet("{id:int}")]
        public ActionResult<Executor> Get(int id)
        {
            try
            {
                if (id < -1)
                {
                    return NotFound();
                }

                var executor = _executorRepository.Get(id);
                return executor;
            }
            catch
            {
                return Problem();
            }
        }


        /// <summary>
        /// Добавление исполнителя задачи
        /// </summary>
        /// <param name="executor">Новый исполнитель задач</param>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Executor executor)
        {
            try
            {

                return _executorRepository.AddExecutor(executor);
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Изменение параметров исполнителя задачи по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="executor">Новый исполнитель задач</param>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Executor executor)
        {
            try
            {
                return _executorRepository.UpdateExecutor(id, executor);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Удаление всех исполнителей задач
        /// </summary>
        [HttpDelete]
        public void Delete()
        {
            _executorRepository.RemoveAllExecutors();
        }

        /// <summary>
        /// Удаление исполнителя задачи по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _executorRepository.RemoveExecutor(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }

    }
}
