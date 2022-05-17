using Lab2.Model;
using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Lab2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController: ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public ActionResult<List<Player>> Get() => _playerRepository.ListPlayers();

        [HttpPost]
        public IActionResult Post(Player player)
        {
            try
            {
                _playerRepository.Add(player);
                return CreatedAtAction(nameof(Post), player);
            }
            catch
            {
                return Problem();
            }
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            try
            {
                _playerRepository.Clear();
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }

        [HttpDelete("{name:string}")]
        public IActionResult Delete(string name)
        {
            try
            {
                _playerRepository.Remove(name);
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }
    }
}