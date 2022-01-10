using Microsoft.AspNetCore.Mvc;
using ptud_project.Data;
using ptud_project.Models;
using ptud_project.Services;
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
                    sex = request.sex,
                    avatar_url = request.avatar_url
                };
                Console.WriteLine(customer);
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
                var md5_password_request = Services.helper.CreateMD5(request.password);
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

    }
}
