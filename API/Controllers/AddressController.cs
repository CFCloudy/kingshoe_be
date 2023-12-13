using BUS.IServices;
using BUS.Services;
using DTO.Address;
using DTO.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressServices _repoAddress;
        private readonly ShoeStoreContext _context;
        public AddressController(ShoeStoreContext context)
        {
            _repoAddress = new AddressServices();
            _context = context;
        }

        [HttpPost]
        [Route("create-address")]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDTO model)
        {
            var res= _repoAddress.CreateAddress(model);
            if (res == true)
            {
                return Ok(new Response<int> { Status = "Success", Message = "Thêm mới địa chỉ thành công." });
            }
            else
            {
                return BadRequest(res);
            }
            
        }

        [HttpGet]
        [Route("get-book-address")]
        public async Task<IActionResult> GetBookAddress([FromQuery] GetBookAdressFilterDTO input)
        {
            var res = _repoAddress.GetBookAddress(input);

                return Ok(new Response<List<BookAddressDTO>>
                {
                    TotalCount = res.Count(),
                    Status = "Success",
                    Message = "Thêm mới địa chỉ thành công.",
                    Payload = res.OrderByDescending(x => x.IsDefault).Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
                });
        }

        [HttpDelete]
        [Route("delete-address-detail")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = _repoAddress.Delete(id);
            if (res == true)
            {
                return Ok(new Response<int>
                {
                    Status = "Success",
                    Message = "Xóa địa chỉ thành công."
                });
            }
            else
            {
                return NotFound();
            }
         

        }


        [HttpGet]
        [Route("get-address-detail")]
        public async Task<IActionResult> GetDetail(int id)
        {
            
            return Ok(new Response<BookAddressDTO> { Status = "Success", Message = "Thêm mới địa chỉ thành công.", Payload = _repoAddress.GetDetail(id)});

        }

        [HttpPut]
        [Route("update-address")]
        public async Task<IActionResult> UpdateAdress(UpdateAddressDTO model)
        {
            if (!_context.UserAddresses.Any(x => x.Id == model.Id))
                return StatusCode(StatusCodes.Status404NotFound, new Response<UpdateAddressDTO>
                {
                    Status = "Not found",
                    Message = "Không tìm thấy địa chỉ!",
                    Payload = null
                });
            var address = _context.UserAddresses.FirstOrDefault(a => a.IsDefault == true && model.UserId == a.UserId);
            if (address is not null && model.IsDefault == true)
            {
                address.IsDefault = false;
                _context.SaveChanges();
            }
            var newad = _context.UserAddresses.FirstOrDefault(a => a.Id == model.Id);
            newad.PhoneNumber = model.PhoneNumber;
            newad.Ward = model.Ward;
            newad.AddressDetail = model.AddressDetail;
            newad.City = model.City;
            newad.District = model.District;
            newad.Name = model.Name;
            newad.Type = model.Type;
            newad.ModifiTime = DateTime.Now;
            _context.UserAddresses.Update(newad);
            await _context.SaveChangesAsync();
            return Ok(new Response<UpdateAddressDTO> { Status = "Success", Message = "Thêm mới địa chỉ thành công.", Payload = model });
        }
    }
}
