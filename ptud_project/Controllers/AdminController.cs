using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
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
                var key = _configuration.GetValue<string>("admin");
                return Ok(new { code = 0, message = "Bad Request", payload = key });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

    }
}
