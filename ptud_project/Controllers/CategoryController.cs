using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_all")]
        public IActionResult GetAllCategory()
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var categories = dbClient.GetDatabase("ptudhttt").GetCollection<ProductCategory>("Categories").Find(_ => true).ToList();
                return Ok(new
                {
                    code = 0,
                    message = "Success",
                    payload = categories,
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request"});
            }
        }
    }
}
