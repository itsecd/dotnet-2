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

        public ATMController(IATMRepository ATMrepository)
        {
            _atmRepository = ATMrepository;
        }

        // GET: api/<ATMController>
        [HttpGet]
        public IEnumerable<ATM> Get()
        {
            return _atmRepository.GetAllATMs();
        }

        // GET api/<ATMController>/5
        [HttpGet("{id}")]
        public ActionResult<ATM> Get(string id)
        {
            var tmp = _atmRepository.GetATMById(id);
            if (tmp != null)
            {
                return tmp;
            }
            return NotFound();
        }

        // POST api/<ATMController>
        [HttpPost]
        public ActionResult<ATM> Post([FromBody] ATM ATM)
        {
            if (_atmRepository.InsertATM(ATM) != null)
                return ATM;
            return Conflict();
        }

        // PUT api/<ATMController>/5
        [HttpPut("{id}")]
        public ActionResult<ATM> Put(string id, [FromBody] int balance)
        {
            var tmp = _atmRepository.ChangeBalanceById(id, balance);
            if (tmp != null)
                return tmp;
            return NotFound();
        }

        //// DELETE api/<ATMController>/5
        [HttpDelete("{id}")]
        public ActionResult<ATM> Delete(string id)
        {
            var tmp = _atmRepository.DeleteATMById(id);
            if (tmp != null)
                return tmp;
            return NotFound();
        }
    }
}
