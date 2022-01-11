using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ptud_project.Data;
using ptud_project.Models;
using ptud_project.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_info")]
        [Authorize]
        public IActionResult GetCustomerById()
        {
            var id_claim = User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            if (id_claim == null) {
                return Ok(new { code = -400, message = "Not existing data"});
            }
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var filter = Builders<Customer>.Filter.Eq("Id", id_claim);
            var customer = dbClient.GetDatabase("ptudhttt").GetCollection<Customer>("Customers").AsQueryable().Where(x => x.id == id_claim.Value.ToString()).FirstOrDefault();
            if (customer != null )
            {
                return Ok(customer);
            }
            else
            {
                return Ok(new { code = -400, message = "Not existing data"});
            }
        }

        [HttpPost("register")]
        public IActionResult CreateCustomer([FromBody] RegisterCustomerModel request)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var filter = Builders<Customer>.Filter.Eq("phone", request.phone);
                var customer_check = dbClient.GetDatabase("ptudhttt").GetCollection<Customer>("Customers").AsQueryable().Where(x => x.phone == request.phone).FirstOrDefault();
                Console.WriteLine(customer_check);
                if (customer_check != null)
                {
                    return Ok(new
                    {
                        code = -2,
                        message = "This phone already exists in database",
                    }); ;
                }
                // check and hash password
                if (request.password != request.confirm_password)
                {
                    return Ok(new
                    {
                        code = -1,
                        message = "Password does not match"
                    });
                }
                // hash password before add 
                var md5_password = Services.helper.CreateMD5(request.password);

                TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;
                //add customer
                var customer = new Customer
                {
                    name = request.name,
                    cmnd = request.cmnd,
                    address = request.address,
                    phone = request.phone,
                    created_at = secondsSinceEpoch,
                    password = md5_password,
                    sex = request.sex,
                    avatar_url = request.avatar_url
                };

                dbClient.GetDatabase("ptudhttt").GetCollection<Customer>("Customers").InsertOne(customer);
                return Ok(new
                {
                    code = 0,
                    message = "Register account success",
                    payload = customer
                }
                );
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request"});
            }
        }

        [HttpPost("login")]
        public IActionResult LoginCustomer([FromBody] LoginCustomerModel request)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));

                var md5_password_request = Services.helper.CreateMD5(request.password);
                var customer = dbClient.GetDatabase("ptudhttt").GetCollection<Customer>("Customers")
                    .AsQueryable().Where(x => x.phone == request.username && x.password == md5_password_request)
                    .FirstOrDefault();

                if (customer != null)
                {
                    // generateToken
                    var key = _configuration.GetValue<string>("JwtConfig:Key");
                    var keyBytes = Encoding.ASCII.GetBytes(key);

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var tokenDecriptor = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, customer.phone),
                            new Claim("id",customer.id.ToString()),
                            new Claim("password", customer.password)
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDecriptor);
                    var login_token = tokenHandler.WriteToken(token);
                    //
                    return Ok(new
                    {
                        code = 0,
                        message = "Login success",
                        payload = customer,
                        token = login_token
                    }
                );
                }
                else
                {
                    return Ok(new { code = -400, message = "Not existing data"});
                }
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request"});
            }
        }

    }
}
