using Microsoft.AspNetCore.Mvc;
using ptud_project.Data;
using ptud_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CustomerController(MyDbContext context)
        {
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

                // check and hash password
                if (request.password != request.confirm_password)
                {
                    return Ok(new
                    {
                        code = -1,
                        message = "password does not match"
                    });
                }

                var md5_password = CreateMD5(request.password);

                // hash password before add 

                TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;
                //add customer
                var customer = new Customer
                {
                    id_cus = Guid.NewGuid(),
                    name = request.name,
                    cmnd = request.cmnd,
                    address = request.address,
                    phone = request.phone,
                    created_at = secondsSinceEpoch,
                    password = md5_password,
                    sex = request.sex
                };

                _context.Add(customer);
                _context.SaveChanges();
                return Ok(customer);
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
                var md5_password_request = CreateMD5(request.password);
                var customer = _context.Customers.SingleOrDefault(cus => (cus.phone == request.username) && (cus.password == md5_password_request));
                if (customer != null)
                {
                    return Ok(customer);
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

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
