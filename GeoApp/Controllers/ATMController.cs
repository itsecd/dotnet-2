using GeoApp.Model;
using GeoApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GeoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ATMController : ControllerBase
    {
        private readonly IATMRepository _ATMRepository;

        public ATMController(IATMRepository ATMrepository)
        {
            _ATMRepository = ATMrepository;
        }

        // GET: api/<ATMController>
        [HttpGet]
        public IEnumerable<ATM> Get()
        {
            return _ATMRepository.GetAllATMs();
        }

        // GET api/<ATMController>/5
        [HttpGet("{id}")]
        public ActionResult<ATM> Get(string id)
        {
            var tmp = _ATMRepository.GetATMById(id);
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
            _ATMRepository.InsertATM(ATM);
            return ATM;
        }

        // PUT api/<ATMController>/5
        [HttpPut("{id}")]
        public ActionResult<ATM> Put(string id, [FromBody] int balance)
        {
            var tmp = _ATMRepository.ChangeBalanceById(id, balance);
            if (tmp != null)
                return tmp;
            return NotFound();
        }

        //// DELETE api/<ATMController>/5
        [HttpDelete("{id}")]
        public ActionResult<ATM> Delete(string id)
        {
            var tmp = _ATMRepository.DeleteATMById(id);
            if (tmp != null)
                return tmp;
            return NotFound();
        }
    }
}
