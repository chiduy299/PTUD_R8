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
                if (request.phone == null || request.phone.Length == 0)
                    return Ok(new
                    {
                        code = -1,
                        message = "Invalid phone",
                    });
                if (request.name == null || request.name.Length == 0)
                    return Ok(new
                    {
                        code = -1,
                        message = "Invalid name",
                    }); ;
                if (request.area_type == null || request.area_type.Length == 0)
                    return Ok(new
                    {
                        code = -1,
                        message = "Invalid area_type",
                    }); ;
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var filter = Builders<Customer>.Filter.Eq("phone", request.phone);
                var customer_check = dbClient.GetDatabase("ptudhttt").GetCollection<Customer>("Customers").AsQueryable().Where(x => x.phone == request.phone).FirstOrDefault();
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

                //add customer
                var customer = new Customer
                {
                    name = request.name,
                    email = request.email,
                    cmnd = request.cmnd,
                    district = request.district,
                    street = request.street,
                    city = request.city,
                    ward = request.ward,
                    phone = request.phone,
                    created_at = Services.helper.now_to_epoch_time(),
                    password = md5_password,
                    sex = request.sex,
                    avatar_url = request.avatar_url,
                    total_amount_paid = 0,
                    total_orders = 0,
                    is_enabled = true,
                    area_type = request.area_type
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

        [HttpPut("update/{id}")]
        public IActionResult UpdateCustomer(string id, [FromBody] UpdateCustomerModel request)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var customer = dbClient.GetDatabase("ptudhttt").GetCollection<Customer>("Customers").AsQueryable().Where(x => x.id == id).FirstOrDefault();
                if (customer == null)
                {
                    return Ok(new
                    {
                        code = -2,
                        message = "Not existing data",
                    });
                }
                // check and hash password
                if (request.password != null && request.confirm_password != null)
                {
                    if(request.password == request.confirm_password)
                    {
                        customer.password = Services.helper.CreateMD5(request.password);
                    }
                    else
                        return Ok(new
                        {
                            code = -2,
                            message = "Password does not match",
                        });

                }

                if (request.name != null && request.name.Length != 0)
                    customer.name = request.name;
                if (request.email != null && request.email.Length != 0)
                    customer.email = request.email;
                if (request.cmnd != null && request.cmnd.Length != 0)
                    customer.cmnd = request.cmnd;
                if (request.district != null && request.district.Length != 0)
                    customer.district = request.district;
                if (request.street != null && request.street.Length != 0)
                    customer.street = request.street;
                if (request.city != null && request.city.Length != 0)
                    customer.city = request.city;
                if (request.ward != null && request.ward.Length != 0)
                    customer.ward = request.ward;
                if (request.sex != 0 && request.sex.ToString().Length != 0)
                    customer.sex = request.sex;
                if (request.avatar_url != null && request.avatar_url.Length != 0)
                    customer.avatar_url = request.avatar_url;
                if (request.area_type != null && request.area_type.Length != 0)
                    customer.area_type = request.area_type;
                customer.updated_at = helper.now_to_epoch_time();
                dbClient.GetDatabase("ptudhttt").GetCollection<Customer>("Customers").FindOneAndReplace(x=> x.id == id,customer);
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
                return Ok(new { code = -401, message = "Bad Request" });
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
                    var key = _configuration.GetValue<string>("JwtConfig");
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
