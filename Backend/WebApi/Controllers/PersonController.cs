using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using Application.Services.Interface;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Common.Core.Models.Person;
using Common.Entities;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PersonController : BaseController
    {
        private IRepositoryWrapper _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;


        public PersonController(IWebHostEnvironment hostEnvironment, IAccountService accountService, IRepositoryWrapper repository)
        {
            _webHostEnvironment = hostEnvironment;
            _repository = repository;
            _accountService = accountService;
        }

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

        [HttpGet("get-all-uploaded-records")]
        public async Task<IActionResult> GetAllPersons()
        {
            var persons = await _repository.Person.FindAll().ToListAsync();
            return Ok(persons);
        }

        [HttpGet("get-person/{id}")]
        public async Task<IActionResult> GetPersons(int id)
        {
            var persons = await _repository.Person.Find(p => p.Identity == id);
            return Ok(persons);
        }


        [HttpPost("add-person")]
        public async Task<IActionResult> addPerson(PersonRequest model)
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
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }

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
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }

        [HttpDelete("delete-person")]
        public async Task<IActionResult> DeletePerson(PersonRequest model)
        {
            try
            {
                var person = await _repository.Person.Find(p => p.Identity == model.Identity);
                if (person != null)
                {
                    _repository.Person.Delete(person);
                }
                else
                {
                    return BadRequest($"sorry no record with this identity {model.Identity} exist.");
                }
                await _repository.Save();
                return Ok("record deleted successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("Oops, sorry something went wrong, pleace check error logs.");
            }
        }
    }
}
