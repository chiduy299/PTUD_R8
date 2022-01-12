using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Data;
using ptud_project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ptud_project.Models.ShipperModel;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : Controller
    {
        private readonly IConfiguration _configuration;
        public ShipperController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult RegisterShipper([FromBody] RegisterShipperModel request)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var filter = Builders<Shipper>.Filter.Eq("phone", request.phone);
                var shipper_check = dbClient.GetDatabase("ptudhttt").GetCollection<Shipper>("Shippers").AsQueryable().Where(x => x.phone == request.phone).FirstOrDefault();
                if (shipper_check != null)
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
                var shipper = new Shipper
                {
                    name = request.name,
                    cmnd = request.cmnd,
                    address = request.address,
                    phone = request.phone,
                    created_at = Services.helper.now_to_epoch_time(),
                    password = md5_password,
                    sex = request.sex,
                    avatar_url = request.avatar_url,
                    is_enabled = false,
                    number_order_delivered = 0,
                    //ko can check area_type vi giao dien fe cho chon theo selection query tu DB
                    area_type = request.area_type
                };

                dbClient.GetDatabase("ptudhttt").GetCollection<Shipper>("Shippers").InsertOne(shipper);
                return Ok(new
                {
                    code = 0,
                    message = "Register account success",
                    payload = shipper
                }
                );
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpPost("approval/{id_shipper}")]
        public IActionResult ApprovalShipper(string id_shipper)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var shipper = dbClient.GetDatabase("ptudhttt").GetCollection<Shipper>("Shippers").AsQueryable().Where(x => x.id == id_shipper).FirstOrDefault();
                if (shipper == null)
                    return Ok(new { code = -1, message = "Not exising data" });
                if (shipper.is_enabled == true)
                    return Ok(new { code = -1, message = "This shipper alreary approval" });
                shipper.is_enabled = true;
                shipper.updated_at = helper.now_to_epoch_time();
                dbClient.GetDatabase("ptudhttt").GetCollection<Shipper>("Shippers").FindOneAndReplace(x => x.id == id_shipper, shipper);
                return Ok(new
                {
                    code = 0,
                    message = "Approve success",
                    payload = shipper
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpPost("ban/{id_shipper}")]
        public IActionResult BanShipper(string id_shipper)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var shipper = dbClient.GetDatabase("ptudhttt").GetCollection<Shipper>("Shippers").AsQueryable().Where(x => x.id == id_shipper).FirstOrDefault();
                if (shipper == null)
                    return Ok(new { code = -1, message = "Not exising data" });
                shipper.is_enabled = false;
                shipper.updated_at = helper.now_to_epoch_time();
                dbClient.GetDatabase("ptudhttt").GetCollection<Shipper>("Shippers").FindOneAndReplace(x => x.id == id_shipper, shipper);
                return Ok(new
                {
                    code = 0,
                    message = "Ban success",
                    payload = shipper
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }

        [HttpPut("receiver_order/{id_shipper}/{id_order}")]
        public IActionResult ReceiverOrder(string id_shipper, string id_order)
        {
            try
            {
                //validate
                // kiem tra sdt dang ky da ton tai chua?
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var shipper = dbClient.GetDatabase("ptudhttt").GetCollection<Shipper>("Shippers").AsQueryable().Where(x => x.id == id_shipper).FirstOrDefault();
                if (shipper == null)
                    return Ok(new { code = -1, message = "Not exising data shipper" });
                var order = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.id == id_order).FirstOrDefault();
                if (order == null)
                    return Ok(new { code = -1, message = "Not exising data order" });
                //kiem tra status shipper
                if (shipper.is_enabled == false)
                    return Ok(new { code = -1, message = "Shipper can not receiver order" });
                //kiem tra status order co ready to change ko? (status phai ==2 nghia la store da xac nhan)
                if (order.status != 2)
                    return Ok(new { code = -1, message = "Order can not receive" });
                order.status = 3;
                order.updated_at = helper.now_to_epoch_time();
                dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").FindOneAndReplace(x => x.id == id_order, order);
                return Ok(new
                {
                    code = 0,
                    message = "Receive order success",
                    payload = order
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }
    }
}
