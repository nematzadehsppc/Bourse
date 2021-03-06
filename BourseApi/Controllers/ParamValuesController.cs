﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BourseApi.Contract;
using Back.DAL.Models;
using System.IO;
using System.Net.Http.Headers;
using System;
using System.Data;
using ExcelDataReader;
using System.Net;
using System.Diagnostics;
using ClosedXML.Excel;

namespace BourseApi.Controllers
{
    [Route("paramvalue")]
    [ApiController]
    [Produces("application/json")]
    public class ParamValuesController : Controller
    {
        private IParamValueContract ParamValueContract { get; set; }

        public ParamValuesController(IParamValueContract paramValueRepository)
        {
            ParamValueContract = paramValueRepository;
        }

        [Route("getAll")]
        [HttpGet]
        public IEnumerable<ParamValue> GetAll() => ParamValueContract.GetAll();

        [Route("getById/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var ParamValue = ParamValueContract.Find(id);
            if (ParamValue == null)
            {
                return new ObjectResult(new ParamValue());
            }
            return Ok(ParamValue);
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult Insert([FromBody]ParamValue value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ParamValueContract.Add(value);
            return CreatedAtRoute("GetParamValue", new { controller = "ParamValue", id = value.Id }, value);
        }

        [Route("update/{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody]ParamValue value)
        {
            if (value is null)
            {
                return BadRequest("value is null.");
            }

            var ParamValue = ParamValueContract.Find(id);
            if (ParamValue == null)
            {
                return NotFound("ParamValue record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ParamValueContract.Update(value);
            return new NoContentResult();
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //ParamValueRepository.Remove(id);

            var ParamValue = ParamValueContract.Find(id);
            if (ParamValue == null)
            {
                return NotFound("ParamValue record couldn't be found.");
            }

            ParamValueContract.Remove(id);
            return NoContent();
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
                    {
                        var workbook = new XLWorkbook(file.OpenReadStream());
                        var worksheet = workbook.Worksheet(1);

                        if (ParamValueContract.readExcelFile(worksheet))
                            return Ok();

                        //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                        //Stream stream = file.OpenReadStream();
                        //IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                        //DataSet dataSet = reader.AsDataSet();

                        //if (ParamValueRepository.readExcelFile(dataSet))
                        //    return Ok();
                    }

                    else if (fileName.EndsWith(".html") || fileName.EndsWith(".htm"))
                    {
                    }

                    return BadRequest();

                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}