using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BourseApi.Contract;
using Back.DAL.Models;

namespace BourseApi.Controllers
{
    [Route("paramType/")]
    [ApiController]
    [Produces("application/json")]
    public class ParamTypesController : Controller
    {
        private IParamTypeRepository ParamTypeRepository { get; set; }

        public ParamTypesController(IParamTypeRepository paramTypeRepository)
        {
            ParamTypeRepository = paramTypeRepository;
        }

        [Route("getAll")]
        [HttpGet]
        public IEnumerable<ParamType> GetAll() => ParamTypeRepository.GetAll();

        [Route("getById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var ParamType = ParamTypeRepository.Find(id);
            if (ParamType == null)
            {
                return new ObjectResult(new ParamType());
            }

            return Ok(ParamType);
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult Insert([FromBody]ParamType value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ParamTypeRepository.Add(value);
            return CreatedAtRoute("GetParamType", new { controller = "ParamType", id = value.Id }, value);
        }

        [Route("update/{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody]ParamType value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            var ParamType = ParamTypeRepository.Find(id);
            if (ParamType == null)
            {
                return NotFound("ParamType record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            ParamTypeRepository.Update(value);
            return new NoContentResult();
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //ParamTypeRepository.Remove(id);

            var ParamType = ParamTypeRepository.Find(id);
            if (ParamType == null)
            {
                return NotFound("The ParamType record couldn't be found.");
            }

            ParamTypeRepository.Remove(id);
            return NoContent();
        }
    }
}