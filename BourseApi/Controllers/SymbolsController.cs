using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BourseApi.Contract;
using Back.DAL.Models;

namespace BourseApi.Controllers
{
    [Route("symbol/")]
    [ApiController]
    [Produces("application/json")]
    public class SymbolsController : Controller
    {
        private ISymbolRepository SymbolRepository { get; set; }

        public SymbolsController(ISymbolRepository symbolRepository)
        {
            SymbolRepository = symbolRepository;
        }

        [Route("getAll")]
        [HttpGet]
        public IEnumerable<Symbol> GetAll() => SymbolRepository.GetAll();

        [Route("getById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var Symbol = SymbolRepository.Find(id);
            if (Symbol == null)
            {
                return new ObjectResult(new Symbol());
            }
            return Ok(Symbol);
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult Insert([FromBody]Symbol value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            SymbolRepository.Add(value);
            return new ContentResult();
        }

        [Route("update/{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody]Symbol value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            var Symbol = SymbolRepository.Find(id);
            if (Symbol == null)
            {
                return NotFound("Symbol record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            SymbolRepository.Update(value);
            return new NoContentResult();
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //SymbolRepository.Remove(id);

            var Symbol = SymbolRepository.Find(id);
            if (Symbol == null)
            {
                return NotFound("Symbol record couldn't be found.");
            }

            SymbolRepository.Remove(id);
            return NoContent();
        }
    }
}