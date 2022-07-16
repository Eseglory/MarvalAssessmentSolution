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

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ManageCsvFilesController : BaseController
    {
        #region Dependencies
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        #endregion

        public ManageCsvFilesController(
            IMapper mapper,
            IWebHostEnvironment hostEnvironment,
            IAccountService accountService
            )
        {
            _webHostEnvironment = hostEnvironment;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpPost("UploadProfilePicture")]
        public async System.Threading.Tasks.Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            try
            {
                string uniqueFileName = null;
                string iPhotoUrl = null;

                if (file.Length > 500000)
                {
                    return BadRequest(new { message = "your image can not be more than 150 kilobytes" });
                }
                if (file.FileName != null)
                {
                }
                return Ok(new { UserImageUrl = iPhotoUrl });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Uploading file content failed" + ex.Message });
            }
        }


    }
}
