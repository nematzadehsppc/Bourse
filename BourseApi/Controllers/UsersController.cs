using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BourseApi.Contract;
using Back.DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace BourseApi.Controllers
{
    [Route("user/")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private IUserRepository UserRepository { get; set; }
        
        public UsersController(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }
        
        [Route("getAll")]
        [HttpGet]
        public IEnumerable<User> GetAllUsers() => UserRepository.GetAll();

        [Route("getById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var User = UserRepository.Find(id);
            if (User == null)
            {
                return new ObjectResult(new User());
            }
            return Ok(User);
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult Insert([FromBody]User value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            UserRepository.Add(value);
            return CreatedAtRoute("GetUser", new { controller = "User", id = value.Id }, value);
        }

        [Route("update/{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody]User value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            var User = UserRepository.Find(id);
            if (User == null)
            {
                return NotFound("User record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            UserRepository.Update(value);
            return new NoContentResult();
        }

        [Authorize]
        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //UserRepository.Remove(id);

            var User = UserRepository.Find(id);
            if (User == null)
            {
                return NotFound("User record couldn't be found.");
            }

            UserRepository.Remove(id);
            return NoContent();
        }
    }
}