using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        public CustomerController(MyDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("get_by_id/{id}")]
        public IActionResult GetCustomerById(string id)
        {
            var id_request = new Guid(id);
            // using LinQ [Object] Query
            var customer = _context.Customers.SingleOrDefault(cus => cus.id_cus == id_request);
            if (customer != null )
            {
                return Ok(customer);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("register")]
        public IActionResult CreateCustomer([FromBody] RegisterCustomerModel request)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                // using LinQ [Object] Query
                var customer_check = _context.Customers.SingleOrDefault(cus => cus.phone == request.phone);
                if (customer_check != null)
                {
                    return Ok(new
                    {
                        code = -2,
                        message = "This phone already exists in datanbase"
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
                var id_new = Guid.NewGuid();
                //add customer
                var customer = new Customer
                {
                    id_cus = id_new,
                    name = request.name,
                    cmnd = request.cmnd,
                    address = request.address,
                    phone = request.phone,
                    created_at = secondsSinceEpoch,
                    password = md5_password,
                    sex = request.sex,
                    avatar_url = request.avatar_url
                };

                Console.WriteLine(customer);
                _context.Add(customer);
                _context.SaveChanges();
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
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public IActionResult LoginCustomer([FromBody] LoginCustomerModel request)
        {
            try
            {
                var md5_password_request = Services.helper.CreateMD5(request.password);
                var customer = _context.Customers.SingleOrDefault(cus => (cus.phone == request.username) && (cus.password == md5_password_request));
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
                            new Claim("id",customer.id_cus.ToString()),
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
                        message = "Register account success",
                        payload = customer,
                        token = login_token
                    }
                );
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
