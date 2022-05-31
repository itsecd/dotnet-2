using Microsoft.AspNetCore.Mvc;
using GeoAppATM.Model;
using GeoAppATM.Repository;
using System.Collections.Generic;

namespace GeoAppATM.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ATMController : ControllerBase
    {
        private readonly IAtmRepository _atmRepository;
        public ATMController(IAtmRepository atmRepository)
        {
            _atmRepository = atmRepository;
        }

        /// <summary>
        /// Getting all ATMs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<GeoJsonATM> Get()
        {
            return _atmRepository.GetAllATM();
        }

        /// <summary>
        /// Getting ATM by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<GeoJsonATM> Get(string id)
        {
            var atm = _atmRepository.GetATMByID(id);
            if (atm != null)
            {
                return atm;
            }
            return NotFound();
        }

        /// <summary>
        /// Changing the ATM balance by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<GeoJsonATM> Put(string id, [FromBody] int balance)
        {
            var atm = _atmRepository.ChangeBalanceByID(id, balance);
            if (atm != null)
            {
                return atm;
            }
            return NotFound();
        }
    }
}
