using BUS.IServices;
using DAL.Models;
using DTO.Customer;
using DTO.Simple;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShoeStoreContext _context;
        private readonly ICustomerServices _customerServices;
        public UserController(ShoeStoreContext context, ICustomerServices customerServices)
        {
            _context = context;
            _customerServices = customerServices;
        }

        [HttpGet]

        public async Task<IActionResult> GetCustomerDetail(int uId)
        {
            try
            {
                var user = _context.UserProfiles.FirstOrDefault(p => p.Id == uId);
                var returnList = new CustomerDetail();
                if (user != null) {
                    var listGet = _context.Orders.Where(c => uId == 0 || c.UserId == uId).ToList();
                    var lstOrder = new List<ReturnOrder>();
                    returnList.FullName=user.FullName;
                    returnList.Id=user.Id;
                    returnList.PhoneNumber=user.PhoneNumber;
                    //re
                    foreach (var item in listGet)
                    {
                        var obj = new ReturnOrder();
                        obj.orderId = item.Id;
                        obj.cartId = 0;
                        
                        var listGets = _context.OrderItems.Where(c => c.OrderId == item.Id).ToList();
                        if (listGets != null && listGets.Any())
                        {
                            foreach (var item1 in listGets)
                            {
                                var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == item1.ProductVariantId);
                                var images = JsonConvert.DeserializeObject<List<int>>(shoeItem.ImageId);
                                //var returnItem = new ReturnOrderItem(item1.ProductVariantId ?? -1, item1.Quantity ?? -1, item1.Price ?? -1, _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url, _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName, _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1);
                                var returnItem = new ReturnOrderItem();
                                returnItem.quantity = item1.Quantity ?? -1;
                                returnItem.price = item1.Price ?? -1;
                                returnItem.variantId = item1.ProductVariantId ?? -1;
                                returnItem.img = _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url;
                                returnItem.color = _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName;
                                returnItem.size = _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1;
                                obj.items.Add(returnItem);
                            }
                        }
                        obj.total = listGets.Sum(x=>(x.Quantity*x.Price));
                        obj.createDate = item.CreatedAt ?? DateTime.MinValue;
                        lstOrder.Add(obj);
                    }
                    returnList.ListOrder = lstOrder;
                }
                return Ok(returnList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("get-customer")]
        public async Task<IActionResult> GetCustomer([FromQuery] CustomerFilterDto input)
        {
            var res = _customerServices.GetAllCustomer(input);
            if (res is not null)
            {
                return Ok(new { Payload = res.Result });
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("get-customer-details")]
        public async Task<IActionResult> GetCustomer([FromQuery] int id)
        {
            var res = _customerServices.GetCustomerDetails(id);
            if (res is not null)
            {
                return Ok(new { Payload = res.Result });
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPut]
        [Route("update-profiles")]
        public async Task<IActionResult> UpdateProfils([FromBody] UpdateProfiles input)
        {
            var user = _context.UserProfiles.FirstOrDefault(x => x.Id == input.Id);
            if (user != null) { 
                
                user.FullName= input.FullName;
                user.Avatar = input.Image;
                user.Gender = input.Gender;
                user.PhoneNumber = input.PhoneNumber;
                _context.UserProfiles.Update(user);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
