using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BourseApi.Contract;
using Back.DAL.Models;

namespace BourseApi.Controllers
{
    [Route("Item/")]
    [ApiController]
    [Produces("application/json")]
    public class ItemsController : Controller
    {
        private IItemRepository ItemRepository { get; set; }

        public ItemsController(IItemRepository itemRepository)
        {
            ItemRepository = itemRepository;
        }

        [Route("getAll")]
        [HttpGet]
        public IEnumerable<Item> GetAll() => ItemRepository.GetAll();

        [Route("getById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var Item = ItemRepository.Find(id);
            if (Item == null)
            {
                return new ObjectResult(new Item());
            }

            return Ok(Item);
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult Insert([FromBody]Item value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ItemRepository.Add(value);
            return CreatedAtRoute("GetItem", new { controller = "Item", id = value.Id }, value);
        }

        [Route("update/{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody]Item value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            var Item = ItemRepository.Find(id);
            if (Item == null)
            {
                return NotFound("Item record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            ItemRepository.Update(value);
            return new NoContentResult();
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //ItemRepository.Remove(id);

            var Item = ItemRepository.Find(id);
            if (Item == null)
            {
                return NotFound("The Item record couldn't be found.");
            }

            ItemRepository.Remove(id);
            return NoContent();
        }
    }
}