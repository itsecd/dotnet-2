using Lab2.Model;
using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public ActionResult<List<Player>> Get() => _playerRepository.GetPlayers();

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

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int index)
        {
            try
            {
                _playerRepository.RemoveAt(index);
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }
    }
}