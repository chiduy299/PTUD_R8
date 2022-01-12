using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
    public class StoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StoreController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
