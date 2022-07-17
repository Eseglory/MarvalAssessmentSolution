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

        [Authorize]
        [HttpPost("upload-person-csv-file")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            try
            {
                string uniqueFileName = null;
                string iPhotoUrl = null;
                //file should not be more than 5 mb you can increase the mb if you want
                if (file.Length > 500000)
                {
                    return BadRequest(new { message = "your image can not be more than 150 kilobytes" });
                }
                if (file.FileName != null)
                {
                    //store the original copy of the csv file
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "csvfiles");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    uniqueFileName = uniqueFileName.Replace("-", "");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                await _repository.Save();
                return Ok(new { UserImageUrl = iPhotoUrl });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Uploading file content failed" + ex.Message });
            }
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
                var person = new Person()
                {
                    Active = model.Active,
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    Age = model.Age,
                    Sex = model.Sex,
                    Mobile = model.Mobile
                };

                await _repository.Person.Create(person);
                await _repository.Save();
                return Ok("record was added successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Add Person Exception: {e.Message}");
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }

        [Authorize]
        [HttpPut("update-person")]
        public async Task<IActionResult> UpdatePerson(PersonRequest model)
        {
            try
            {
                var person = await _repository.Person.Find(p => p.Identity == model.Identity);
                if (person != null)
                {
                    person.Active = model.Active;
                    person.FirstName = model.FirstName;
                    person.Surname = model.Surname;
                    person.Age = model.Age;
                    person.Sex = model.Sex;
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
