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
    public class AreaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AreaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_all")]
        public IActionResult GetAllArea()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var list_areas = dbClient.GetDatabase("ptudhttt").GetCollection<Area>("Areas").Find(_ => true).ToList();
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = list_areas,
            });
        }
    }
}
