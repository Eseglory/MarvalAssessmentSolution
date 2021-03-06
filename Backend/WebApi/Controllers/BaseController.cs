using Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarvalWebApi.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        public Account Account => (Account)HttpContext.Items["Account"];
    }
}
