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
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var filter = Builders<Store>.Filter.Eq("phone", request.phone);
                var customer_check = dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Store").AsQueryable().Where(x => x.phone == request.phone).FirstOrDefault();
                if (customer_check != null)
                {
                    return Ok(new
                    {
                        code = -2,
                        message = "This phone already exists in database",
                    }); ;
                }

                var store = new Store
                {
                    store_name = request.store_name,
                    rating = 0,
                    provider_id = request.provider_id,
                    id_category = request.product_category,
                    phone = request.phone,
                    created_at = Services.helper.now_to_epoch_time(),
                    update_at = Services.helper.now_to_epoch_time(),
                    is_available = true,
                };

                dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Store").InsertOne(store);
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
            var store = dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Store").AsQueryable().Where(x => x.id == id_store).FirstOrDefault();
            if (store == null)
                return Ok(new
                {
                    code = -1,
                    message = "Not existing data"
                });
            if (store.is_available == true)
                store.is_available = false;
            else
                store.is_available = true;
            store.update_at = helper.now_to_epoch_time();
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = store
            });
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateProduct(string id_store, [FromBody] UpdateStoreModel request)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var store = dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Store").AsQueryable().Where(x => x.id == id_store).FirstOrDefault();
            if (store == null)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Not existing data"
                });
            }
            if (request.store_name != null)
                store.store_name = request.store_name;
            if (request.product_category != null)
                store.provider_id = request.provider_name;
            if (request.provider_name != null)
                store.provider_id = request.provider_name;
            if (request.phone != null)
                store.phone = request.phone;
            store.update_at = helper.now_to_epoch_time();
            dbClient.GetDatabase("ptudhttt").GetCollection<Store>("Store").FindOneAndReplace(x => x.id == id_store, store);
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = store
            });
        }
    }
}
