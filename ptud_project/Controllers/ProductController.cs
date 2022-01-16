using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Data;
using ptud_project.Models;
using ptud_project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_product_store")]
        public IActionResult GetAllProductByProvideId([FromQuery] string store_id, [FromQuery] int page, [FromQuery] int limit)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var list_product = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").Find(x => x.store_id == store_id).Skip(page*limit).Limit(limit).ToList();
                return Ok(new
                {
                    code = 0,
                    message = "Success",
                    payload = list_product
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateProduct(string id_product, [FromBody] ProductUpdateModel request)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var product = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id == id_product).FirstOrDefault();
            if (product == null)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Not existing data"
                });
            }
            if (request.product_name != null)
                product.product_name = request.product_name;
            if (request.product_remaining != 0)
                product.product_remaining = request.product_remaining;
            if (request.unit_price != 0)
                product.unit_price = request.unit_price;
            if (request.unit_product_name != null)
                product.unit_product_name = request.unit_product_name;
            product.updated_at = helper.now_to_epoch_time();
            dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").FindOneAndReplace(x => x.id == id_product, product);
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = product
            });
        }

        [HttpGet("get_by_id/{id_product}")]
        public IActionResult GetProductbyID(string id_product)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var product = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id == id_product).FirstOrDefault();
            if (product == null)
                return Ok(new
                {
                    code = -1,
                    message = "Not existing data"
                });
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = product
            });
        }

        [HttpPut("change_status/{id_product}")]
        public IActionResult EnableDisableProduct(string id_product)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var product = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id == id_product).FirstOrDefault();
            if (product == null)
                return Ok(new
                {
                    code = -1,
                    message = "Not existing data"
                });
            if (product.is_available == true)
                product.is_available = false;
            else
                product.is_available = true;
            product.updated_at = helper.now_to_epoch_time();
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = product
            });
        }

        [HttpGet("get_by_category/{id_category}")]
        public IActionResult GetProductbyCategory(string id_category)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var list_product = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id_category == id_category).ToList();
                if (list_product == null)
                    return Ok(new
                    {
                        code = -1,
                        message = "Not existing data"
                    });
                return Ok(new
                {
                    code = 0,
                    message = "Success",
                    payload = list_product
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpPost("create")]
        public IActionResult CreateProduct([FromBody] CreateProductModel request)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var product_check = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.product_name == request.product_name && x.store_id == request.store_id).FirstOrDefault();
                if (product_check != null)
                {
                    return Ok(new
                    {
                        code = -2,
                        message = "This product already exists in database,  change the name of product",
                    }); ;
                }

                var product = new Product
                {
                    product_name = request.product_name,
                    rating = 0,
                    unit_price = request.unit_price,
                    unit_product_name = request.unit_product_name,
                    id_category = request.product_category,
                    product_remaining = request.quantity,
                    sell_number = 0,
                    store_id = request.store_id,
                    created_at = Services.helper.now_to_epoch_time(),
                    updated_at = Services.helper.now_to_epoch_time(),
                    images = request.list_images,
                    is_available = true,
                };

                dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").InsertOne(product);
                return Ok(new
                {
                    code = 0,
                    message = "Add product success",
                    payload = product
                }
                );
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }
    }
}
