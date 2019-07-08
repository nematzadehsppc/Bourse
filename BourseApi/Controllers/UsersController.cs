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
        private IUserContract UserContract { get; set; }
        
        public UsersController(IUserContract userRepository)
        {
            UserContract = userRepository;
        }

        [Authorize]
        [Produces("application/json")]
        [Route("getAll")]
        [HttpGet]
        public IEnumerable<User> GetAllUsers() => UserContract.GetAll();

        [Route("getById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var User = UserContract.Find(id);
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

            UserContract.Add(value);
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

            var User = UserContract.Find(id);
            if (User == null)
            {
                return NotFound("User record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            UserContract.Update(value);
            return new NoContentResult();
        }

        [Authorize]
        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //UserRepository.Remove(id);

            var User = UserContract.Find(id);
            if (User == null)
            {
                return NotFound("User record couldn't be found.");
            }

            UserContract.Remove(id);
            return NoContent();
        }

    }
}