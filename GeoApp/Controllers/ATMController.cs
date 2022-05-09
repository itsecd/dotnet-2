using GeoApp.Model;
using GeoApp.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GeoApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ATM")]
    [ApiController]
    public class ATMController : ControllerBase
    {
        private readonly IATMRepository _atmRepository;

        public ATMController(IATMRepository atmRepository)
        {
            _atmRepository = atmRepository;
        }

        /// <summary>
        /// Получение всех банкоматов
        /// </summary>
        /// <returns></returns>
        // GET: api/<ATMController>
        [HttpGet]
        public List<JsonATM> Get()
        {
            return _atmRepository.GetAllATMs();
        }

        /// <summary>
        /// Получение банкомата по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<ATMController>/5
        [HttpGet("{id}")]
        public ActionResult<JsonATM> Get(string id)
        {
            var tmp = _atmRepository.GetATMById(id);
            if (tmp != null)
            {
                return tmp;
            }
            return NotFound();
        }

        /// <summary>
        /// Изменение баланса банкомата по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        // PUT api/<ATMController>/5
        [HttpPut("{id}")]
        public ActionResult<JsonATM> Put(string id, [FromBody] int balance)
        {
            var tmp = _atmRepository.ChangeBalanceById(id, balance);
            if (tmp != null)
                return tmp;
            return NotFound();
        }
    }
}
