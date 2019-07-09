using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BourseApi.Contract;
using Back.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace BourseApi.Controllers
{
    [Route("user/")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private IUserContract UserContract { get; set; }
        private IAuthenticationContract AuthenticationContract { get; set; }

        public UsersController(IUserContract userRepository, IAuthenticationContract authenticationContract)
        {
            UserContract = userRepository;
            AuthenticationContract = authenticationContract;
        }

        [Authorize]
        [Produces("application/json")]
        [Route("getAll")]
        [HttpGet]
        public IEnumerable<User> GetAllUsers() => UserContract.GetAll();

        [Authorize]
        [Produces("application/json")]
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

        [Route("addUser")]
        [HttpPost]
        public IActionResult Insert(string name, string family, string username, string password, string email, DateTime birthday)
        {
            if (username is null || password is null)
            {
                return BadRequest("value is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Tuple<AuthenticationResult, User> authResult = UserContract.AddUser(name, family, username, password, email, birthday);
            if (authResult.Item1.Code != AuthenticationResultCode.AuthenticationSuccess)
            {
                return BadRequest(
                    new FailedLoginResponseModel()
                    {
                        code = authResult.Item1.Code,
                        authenticationResult = AuthenticationContract.GetAuthenticationResultMessage(authResult.Item1.Code, "fa-IR"),
                        additionalInformation = authResult.Item1.AdditionalErrorMessage
                    }
                    );
            }
            return CreatedAtRoute("GetUser", new { controller = "User", id = authResult.Item2.Id }, (User)authResult.Item2);
        }

        [Authorize]
        [Produces("application/json")]
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