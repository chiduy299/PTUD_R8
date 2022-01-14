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
    public class StoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StoreController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("register")]
        public IActionResult CreateStore([FromBody] RegisterStoreModel request)
        {
            try
            {
                // validate
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var store_check = dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Stores").AsQueryable().Where(x => x.phone == request.phone).FirstOrDefault();
                if (store_check != null)
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
                var store = new Store
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
                    total_orders = 0,
                    is_enabled = true,
                    area_type = request.area_type
                };

                dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Stores").InsertOne(store);
                return Ok(new
                {
                    code = 0,
                    message = "Register store success",
                    payload = store
                }
                );
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpPut("change_status/{id}")]
        public IActionResult EnableDisableStore(string id_store)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var store = dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Stores").AsQueryable().Where(x => x.id == id_store).FirstOrDefault();
            if (store == null)
                return Ok(new
                {
                    code = -1,
                    message = "Not existing data"
                });
            if (store.is_enabled == true)
                store.is_enabled = false;
            else
                store.is_enabled = true;
            store.updated_at = helper.now_to_epoch_time();
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = store
            });
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateStore(string id_store, [FromBody] UpdateStoreModel request)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var store = dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Stores").AsQueryable().Where(x => x.id == id_store).FirstOrDefault();
                if (store == null)
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
                    if (request.password == request.confirm_password)
                    {
                        store.password = Services.helper.CreateMD5(request.password);
                    }
                    else
                        return Ok(new
                        {
                            code = -2,
                            message = "Password does not match",
                        });

                }

                if (request.name != null && request.name.Length != 0)
                    store.name = request.name;
                if (request.email != null && request.email.Length != 0)
                    store.email = request.email;
                if (request.cmnd != null && request.cmnd.Length != 0)
                    store.cmnd = request.cmnd;
                if (request.district != null && request.district.Length != 0)
                    store.district = request.district;
                if (request.street != null && request.street.Length != 0)
                    store.street = request.street;
                if (request.city != null && request.city.Length != 0)
                    store.city = request.city;
                if (request.ward != null && request.ward.Length != 0)
                    store.ward = request.ward;
                if (request.area_type != null && request.area_type.Length != 0)
                    store.area_type = request.area_type;
                store.updated_at = helper.now_to_epoch_time();
                dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Stores").FindOneAndReplace(x => x.id == id_store, store);
                return Ok(new
                {
                    code = 0,
                    message = "Update store success",
                    payload = store
                }
                );
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpPost("login")]
        public IActionResult LoginStore([FromBody] LoginCustomerModel request)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));

                var md5_password_request = Services.helper.CreateMD5(request.password);
                var store = dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Stores")
                    .AsQueryable().Where(x => x.phone == request.username && x.password == md5_password_request)
                    .FirstOrDefault();

                if (store != null)
                {
                    // generateToken
                    var key = _configuration.GetValue<string>("JwtConfig");
                    var keyBytes = Encoding.ASCII.GetBytes(key);

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var tokenDecriptor = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, store.phone),
                            new Claim("id",store.id.ToString()),
                            new Claim("password", store.password)
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
                        payload = store,
                        token = login_token
                    }
                );
                }
                else
                {
                    return Ok(new { code = -400, message = "Not existing data" });
                }
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }
    }
}
