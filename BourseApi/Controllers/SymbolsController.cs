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
        private ISymbolContract SymbolContract { get; set; }

        public SymbolsController(ISymbolContract symbolContract)
        {
            SymbolContract = symbolContract;
        }

        [Route("getAll")]
        [HttpGet]
        public IEnumerable<Symbol> GetAll() => SymbolContract.GetAll();

        [Route("getById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var Symbol = SymbolContract.Find(id);
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

            SymbolContract.Add(value);
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

            var Symbol = SymbolContract.Find(id);
            if (Symbol == null)
            {
                return NotFound("Symbol record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            SymbolContract.Update(value);
            return new NoContentResult();
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //SymbolContract.Remove(id);

            var Symbol = SymbolContract.Find(id);
            if (Symbol == null)
            {
                return NotFound("Symbol record couldn't be found.");
            }

            SymbolContract.Remove(id);
            return NoContent();
        }
    }
}