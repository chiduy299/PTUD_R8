using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ptud_project.Models.OrderModel;
using ptud_project.Services;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_order_by_customer")]
        [Authorize]
        public IActionResult GetOrderByCustomer()
        {
            var id_claim = User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            if (id_claim == null)
            {
                return Ok(new { code = -400, message = "Not existing data"});
            }
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var orders = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.customer_id == id_claim.Value.ToString());
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = orders,
            });
        }

        [HttpGet("get_all_order_store")]
        public IActionResult GetAllOrderStore([FromQuery] string store_id)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var orders = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.provider_id == store_id).ToList();
                return Ok(new
                {
                    code = 0,
                    message = "Success",
                    payload = orders
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpGet("get_detail_order_by_id/{id}")]
        public IActionResult GetDetailOrderById(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var details_order = dbClient.GetDatabase("ptudhttt").GetCollection<DetailOrder>("DetailOrders").AsQueryable().Where(x => x.order_id == id);
            List<GetDetailOrderResponse> resp = new List<GetDetailOrderResponse>();
            foreach (var order in details_order)
            {
                var product = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id == order.product_id).SingleOrDefault();
                resp.Add(new GetDetailOrderResponse
                {
                    product_id = product.id,
                    product_name = product.product_name,
                    rating = product.rating,
                    unit_price = product.unit_price,
                    unit_product_name = product.unit_product_name,
                    quantity = order.quantity,
                    total = order.total
                }
                );
            }
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = resp,
            });
        }

        [HttpPut("cancel/{id}")]
        public IActionResult CancelOrderById(string id, [FromQuery] string reason)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var order = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.id == id).FirstOrDefault();
            if (order == null)
            {
                return Ok(new
                {
                    code = -400,
                    message = "Not existing data"
                });
            }
            if (order.status != 0)
            {
                return Ok(new
                {
                    code = -1,
                    message = "This order can not cancel"
                });
            }
            order.status = -1;
            order.reason_cancel = reason;
            order.updated_at = helper.now_to_epoch_time();
            dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").ReplaceOne(x => x.id == id,order);

            return Ok(new
            {
                code = 0,
                message = "Success"
            });
        }

        [HttpPut("confirm/{id}")]
        public IActionResult ConfirmOrderById(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var order = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.id == id).FirstOrDefault();
            if (order == null)
            {
                return Ok(new
                {
                    code = -400,
                    message = "Not existing data"
                });
            }
            if (order.status != 1)
            {
                return Ok(new
                {
                    code = -1,
                    message = "This order can not confirm"
                });
            }
            order.status = 2;
            order.updated_at = helper.now_to_epoch_time();
            dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").ReplaceOne(x => x.id == id, order);

            return Ok(new
            {
                code = 0,
                message = "Success"
            });
        }

        [HttpPut("change_status_ship/{id}")]
        public IActionResult ChangeShippingStatusOrderById(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var order = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.id == id).FirstOrDefault();
            if (order == null)
            {
                return Ok(new
                {
                    code = -400,
                    message = "Not existing data"
                });
            }
            if (order.status != 2)
            {
                return Ok(new
                {
                    code = -1,
                    message = "This order can not shipping because not confirm"
                });
            }
            order.status = 3;
            order.updated_at = helper.now_to_epoch_time();
            dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").ReplaceOne(x => x.id == id, order);

            return Ok(new
            {
                code = 0,
                message = "Success"
            });
        }

        [HttpPut("change_status_success/{id}")]
        public IActionResult ChangeSuccessStatusOrderById(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var order = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.id == id).FirstOrDefault();
            if (order == null)
            {
                return Ok(new
                {
                    code = -400,
                    message = "Not existing data"
                });
            }
            if (order.status != 3)
            {
                return Ok(new
                {
                    code = -1,
                    message = "This order can not change status to success"
                });
            }
            order.status = 4;
            order.updated_at = helper.now_to_epoch_time();
            dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").ReplaceOne(x => x.id == id, order);

            return Ok(new
            {
                code = 0,
                message = "Success"
            });
        }

        [HttpGet("get_delivery_history")]
        public IActionResult GetDeliveryHistoryByShipper([FromQuery] string id_shipper)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var orders = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.shipper_id == id_shipper && x.status == 4).ToList();

                return Ok(new
                {
                    code = 0,
                    message = "Success",
                    payload = orders
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }
    }

}
