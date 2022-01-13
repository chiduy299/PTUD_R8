using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Models;
using ptud_project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult LoginAdmin([FromBody] LoginCustomerModel request)
        {
            try
            {
                var username = _configuration.GetValue<string>("admin:username");
                var password = _configuration.GetValue<string>("admin:password");
                var md5_password = helper.CreateMD5(request.password);
                if (username != request.username || password != md5_password)
                    return Ok(new { code = -1, message = "Can not login to admin" });
                return Ok(new { code = 0, message = "Login success"});
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

    }
}
