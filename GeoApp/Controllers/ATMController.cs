using GeoApp.Model;
using GeoApp.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
        public List<JsonATM> Get()
        {
            return _atmRepository.GetAllATMs();
        }

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

        // POST api/<ATMController>
        //[HttpPost]
        //public ActionResult<XmlATM> Post([FromBody] XmlATM ATM)
        //{
        //    if (_atmRepository.InsertATM(ATM) != null)
        //        return ATM;
        //    return Conflict();
        //}

        // PUT api/<ATMController>/5
        [HttpPut("{id}")]
        public ActionResult<JsonATM> Put(string id, [FromBody] int balance)
        {
            var tmp = _atmRepository.ChangeBalanceById(id, balance);
            if (tmp != null)
                return tmp;
            return NotFound();
        }

        //// DELETE api/<ATMController>/5
        //[HttpDelete("{id}")]
        //public ActionResult<XmlATM> Delete(string id)
        //{
        //    var tmp = _atmRepository.DeleteATMById(id);
        //    if (tmp != null)
        //        return tmp;
        //    return NotFound();
        //}
    }
}
