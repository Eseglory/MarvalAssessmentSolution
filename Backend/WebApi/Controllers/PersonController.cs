using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Common.Core.Models.Person;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Common.Entities;
using Microsoft.Extensions.Logging;
using Common.Core.Helpers;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Linq;
using CsvHelper;
using System.Globalization;

namespace MarvalWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : BaseController
    {
        private IRepositoryWrapper _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IWebHostEnvironment hostEnvironment, ILogger<PersonController> logger, IRepositoryWrapper repository)
        {
            _webHostEnvironment = hostEnvironment;
            _repository = repository;
            _logger = logger;
        }

        //[Authorize]
        [HttpPost("upload-person-csv-file")]
        public async Task<IActionResult> UploadCscFile(IFormFile file)
        {
            try
            {
                List<Person> persons = new List<Person>();
                var fileextension = Path.GetExtension(file.FileName);
                var filename = Guid.NewGuid().ToString() + fileextension;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "csvfiles", filename);
                using (FileStream fs = System.IO.File.Create(filepath))
                {
                    file.CopyTo(fs);
                }
                if (fileextension == ".csv")
                {
                    using (var reader = new StreamReader(filepath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        //Map the csv content to Person object
                        //were are not allowed to modify the headers of the csv file Person object is
                        //to maintain that naming for proper data mapping

                        var records = csv.GetRecords<Person>();
                        foreach (var record in records)
                        {
                            //Validate record 
                           var validationResult = CustomValidator.ValidatePerson(new PersonRequest()
                           {
                               Active = record.Active,
                               Age = record.Age,
                               Sex = record.Sex,
                               Surname = record.Surname,
                               FirstName = record.FirstName,
                               Mobile = record.Mobile
                           });
                            if(!validationResult.isvalid)
                            {
                                return BadRequest(validationResult.message);
                            }

                            Person person = new Person()
                            {
                                Active = record.Active.ToUpper(),
                                Age = record.Age,
                                Sex = record.Sex.ToUpper(),
                                Surname = record.Surname,
                                FirstName = record.FirstName,
                                Mobile = record.Mobile
                            };
                            persons.Add(person);
                        }
                    }

                }
                else
                {
                    return BadRequest($"sorry the system does not support this file type {fileextension}");
                }

                await _repository.Person.AddList(persons);
                await _repository.Save();
            }
            catch (Exception e)
            {
                _logger.LogError($"Batch Person Upload Exception: {e.Message}");
                return BadRequest("sorry something went wrong, invalid data entry please cross check your file");
            }
            return Ok($"csv file: {file.FileName} was uploaded successfully.");
        }

        [Authorize]
        [HttpGet("get-all-uploaded-records")]
        public async Task<IActionResult> GetAllPersons()
        {
            try
            {
                var persons = await _repository.Person.FindAll().ToListAsync();
                return Ok(persons);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get Persons Exception: {e.Message}");
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }

        [Authorize]
        [HttpGet("get-person/{id}")]
        public async Task<IActionResult> GetPersons(int id)
        {
            try
            {
                var persons = await _repository.Person.Find(p => p.Identity == id);
                return Ok(persons);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get Person Exception: {e.Message}");
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }

        [Authorize]
        [HttpPost("add-person")]
        public async Task<IActionResult> AddPerson(PersonRequest model)
        {
            try
            {
                //Validate record 
                var validationResult = CustomValidator.ValidatePerson(model);
                if (!validationResult.isvalid)
                {
                    return BadRequest(validationResult.message);
                }

                var person = new Person()
                {
                    Active = model.Active.ToUpper(),
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    Age = model.Age,
                    Sex = model.Sex.ToUpper(),
                    Mobile = model.Mobile
                };

                await _repository.Person.Create(person);
                await _repository.Save();
                return Ok("record was added successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Add Person Exception: {e.Message}");
                return BadRequest("Oops, sorry something went wrong, pleace cross check your entry.");
            }
        }

        [Authorize]
        [HttpPut("update-person")]
        public async Task<IActionResult> UpdatePerson(PersonRequest model)
        {
            try
            {
                //Validate record 
                var validationResult = CustomValidator.ValidatePerson(model);
                if (!validationResult.isvalid)
                {
                    return BadRequest(validationResult.message);
                }

                var person = await _repository.Person.Find(p => p.Identity == model.Identity);
                if (person != null)
                {
                    person.Active = model.Active.ToUpper();
                    person.FirstName = model.FirstName;
                    person.Surname = model.Surname;
                    person.Age = model.Age;
                    person.Sex = model.Sex.ToUpper();
                    person.Mobile = model.Mobile;
                    _repository.Person.Update(person);
                }
                else
                {
                    return BadRequest($"sorry no record with this identity {model.Identity} exist.");
                }
                await _repository.Save();
                return Ok("record updated successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update Person Exception: {e.Message}");
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }

        [Authorize]
        [HttpDelete("delete-person/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var person = await _repository.Person.Find(p => p.Identity == id);
                if (person != null)
                {
                    _repository.Person.Delete(person);
                }
                else
                {
                    return BadRequest($"sorry no record with this identity {id} exist.");
                }
                await _repository.Save();
                return Ok("record deleted successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete Person Exception: {e.Message}");
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }
    }

}


