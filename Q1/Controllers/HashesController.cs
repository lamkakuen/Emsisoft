using Q1.Models;
using Q1.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Q1.Repositories;

namespace Q1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashesController : ControllerBase
    {


        private readonly HashService _hashService;
        private readonly HashRepository _hashRepository;

        public HashesController(HashService hashService, HashRepository hashRepository)
        {
            _hashService = hashService;
            _hashRepository = hashRepository;
        }


        [HttpPost]
        public IActionResult GenerateHashes()
        {
            //return Ok(new { message = "HttpPost called" });
            _hashService.GenerateAndProcessHashes();
            return Ok(new { message = "HttpPost called" });
            //return NoContent();
        }

        [HttpGet]
        public ActionResult<List<HashCount>> GetHashes()
        {
            var hashes = _hashRepository.GetHashesGroupedByDate();
            var hashCounts = hashes.Select(h => new HashCount { Date = h.Date.Date, Count = h.Count }).ToList();
            return Ok(hashCounts);
        }
    }
}
