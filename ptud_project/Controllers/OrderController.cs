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
using MongoDB.Bson;
using static ptud_project.Models.Payment;
using System.Net.Http;
using System.Text.Json;
using System.Text;

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
                var orders = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.store_id == store_id).ToList();
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
        {
            try
            {
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));

                int sum_item = 0;
                // kiem tra so luong san pham
                for (int i = 0; i < request.products.Count(); i++)
                {
                    var id_product = request.products[i];
                    var quantity = request.quantities[i];
                    var product_db = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id == id_product).FirstOrDefault();
                    if (product_db.product_remaining < quantity)
                        return Ok(new { code = -1, message = product_db.product_name + " is out of stock" });
                    sum_item += quantity;
                }

                Guid order_id = Guid.NewGuid();
                // them order
                var order = new Order
                {
                    order_id = order_id.ToString(),
                    created_at = helper.now_to_epoch_time(),
                    total_amount = request.total_amount,
                    status = 0,
                    total_item = (short)sum_item,
                    paymnent_method_id = request.payment_method_id,
                    customer_id = request.customer_id,
                    store_id = request.store_id
                };
                dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").InsertOne(order);


                List<DetailOrder> list_detail_order = new List<DetailOrder>();
                for (int i = 0; i < request.products.Count(); i++)
                {
                    var id_product = request.products[i];
                    var quantity = request.quantities[i];
                    var product_db = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id == id_product).FirstOrDefault();
                    //cap nhat so luong san pham
                    var quantity_new = product_db.product_remaining - quantity;
                    product_db.product_remaining = quantity_new;
                    product_db.sell_number = product_db.sell_number + quantity;
                    product_db.updated_at = helper.now_to_epoch_time();
                    dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").FindOneAndReplace(x => x.id == id_product,product_db);

                    //them vao detail order
                    var detail_order = new DetailOrder
                    {
                        order_id = order_id.ToString(),
                        product_id = id_product,
                        unit_price = product_db.unit_price,
                        quantity = (short)quantity,
                        total = (double)(product_db.unit_price * quantity)
                    };
                    list_detail_order.Add(detail_order);
                }
                dbClient.GetDatabase("ptudhttt").GetCollection<DetailOrder>("DetailOrders").InsertMany(list_detail_order);
                var momo_resp = await request_momo_paymentAsync(order_id.ToString(), request.total_amount);
                MomoCreatePaymentResponse resp = JsonSerializer.Deserialize<MomoCreatePaymentResponse>(momo_resp);
                return Ok(new { code = 0, message = "Successs", payload = resp });

            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }


        private async Task<string> request_momo_paymentAsync(string order_id, Double amount)
        {
            try
            {
                var partner_code = _configuration.GetValue<string>("MomoConfig:PARTNER_CODE");
                var access_key = _configuration.GetValue<string>("MomoConfig:ACCESS_KEY");
                var secret_key = _configuration.GetValue<string>("MomoConfig:SECRET_KEY");
                var type_purchase = _configuration.GetValue<string>("MomoConfig:TYPE_PURCHASE");
                var ipn_url = _configuration.GetValue<string>("MomoConfig:NOTIFY_URL");
                var return_url = _configuration.GetValue<string>("MomoConfig:RETURN_URL");
                var rawHash = "accessKey=" + access_key
                                + "&amount=" + amount
                                + "&extraData=" + ""
                                + "&ipnUrl=" + ipn_url
                                + "&orderId=" + order_id
                                + "&orderInfo=" + "Don hang cua sundara"
                                + "&partnerCode=" + partner_code
                                + "&redirectUrl=" + return_url
                                + "&requestId=" + order_id
                                + "&requestType=" + type_purchase;
                var signature = Services.helper.GetHMAC(rawHash,secret_key);
                var myjson = new MomoCreatePaymentRequest
                {
                    partnerCode = partner_code,
                    requestId = order_id,
                    amount = (Int64)amount,
                    orderId = order_id,
                    orderInfo = "Don hang cua chung toi",
                    redirectUrl = return_url,
                    ipnUrl = ipn_url,
                    requestType = type_purchase,
                    signature = signature,
                    extraData = "",
                };
                string jsonString = JsonSerializer.Serialize(myjson);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(
                        "https://test-payment.momo.vn/gw_payment/transactionProcessor",
                         new StringContent(jsonString, Encoding.UTF8, "application/json"));
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch
            {
                Console.WriteLine("error");
                return "";
            }
        }
    }

}
