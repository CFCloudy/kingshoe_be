using BUS.IService;
using BUS.Service;
using DAL.Models;
using DTO.Utils;
using DTO.Voucher;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private IServiceVoucher_Bus _ServiceVoucher;
        private readonly ShoeStoreContext _context;
        public VoucherController()
        {
            _ServiceVoucher=new Service_Voucher();
            _context=new ShoeStoreContext();
        }
        [HttpPost]
        public async Task<List<VoucherResponse>> GetVoucher([FromBody] FilterVoucher input)
        {
            var styles = await _ServiceVoucher.GetAllVoucher();
            if (styles == null) return null;
            styles=styles.OrderByDescending(x=>x.CreatedAt).ToList();
            if (input.status != null)
            {
                if (input.status == 1)
                {
                    styles = styles.Where(x => x.StartDate <DateTime.Now&&x.EndDate>DateTime.Now).ToList();
                }else if (input.status == 2)
                {
                    styles = styles.Where(x => x.StartDate > DateTime.Now).ToList();
                }else if (input.status == 3)
                {
                    styles = styles.Where(x =>  x.EndDate < DateTime.Now).ToList();
                }
            }
            if (input.StartDate != null && input.EndDate != null)

            {
                styles = styles.Where(x => x.CreatedAt >= input.StartDate && x.CreatedAt <= input.EndDate).ToList();
            }
            var res= styles.Select(x =>
            {
                var ouput = new VoucherResponse()
                {
                    EndDate=x.EndDate,
                    MaxValue=x.DiscountMaxValue,
                    MinValue=0,
                    NameVoucher=x.VoucherCode,
                    Quantity=x.UseLimit,
                    StartDate=x.StartDate,
                    Status=x.Status,
                    Unit=x.DiscountType==0?true:false,
                    Value=x.DiscountValue,
                    VoucherId=x.Id,
                    
                };

                return ouput;
            }).ToList();
            return res;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetVoucherByID(int id)
        {
            var styles = await _ServiceVoucher.Getone(id);
            if (styles == null) return null;
            var res = new VoucherDetails()
            {
                VoucherCode=styles.VoucherCode,
                DiscountMaxValue=styles.DiscountMaxValue,
                DiscountType=styles.DiscountType,
                DiscountValue=styles.DiscountValue,
                EndDate=styles.EndDate,
                Id=id,
                RequriedValue=styles.RequriedValue,
                StartDate=styles.StartDate, 
                Status = styles.Status,
                StatusVoucher=styles.StatusVoucher,
                UseLimit=styles.UseLimit,
                UsePerPerson=styles.UsePerPerson,
                VoucherContent=styles.VoucherContent,
            };


            return Ok(new Response<VoucherDetails> { Status = "Success", Message = "Thêm mới thành công" ,Payload=res});
        }

        [HttpGet("GetVoucherByUserId")]
        public async Task<List<VoucherResponse>> GetVoucherByUserId(int uid)
        {
            var useLog = _context.VouchersUseLogs.Where(x => x.VoucherUserId == uid);
            var styles = await _ServiceVoucher.GetAllVoucher();
            var lstVoucher=new List<VoucherResponse>();
            if (styles == null) return null;

            foreach (var voucher in styles
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                ) {
                if (useLog.Any())
                {
                    var count = useLog.Where(x => x.VoucherUserId == voucher.Id).Count();
                    if (count < voucher.UseLimit)
                    {
                        var VoucherResponse = new VoucherResponse()
                        {
                            EndDate = voucher.EndDate,
                            MaxValue = voucher.DiscountMaxValue,
                            MinValue = 0,
                            NameVoucher = voucher.VoucherCode,
                            Quantity = voucher.UseLimit,
                            StartDate = voucher.StartDate,
                            Status = voucher.Status,
                            Unit = voucher.DiscountType == 0 ? true : false,
                            Value = voucher.DiscountValue,
                            VoucherId = voucher.Id
                        };
                        lstVoucher.Add(VoucherResponse);
                    }
                }
                else
                {
                    var VoucherResponse = new VoucherResponse()
                    {
                        EndDate = voucher.EndDate,
                        MaxValue = voucher.DiscountMaxValue,
                        MinValue = 0,
                        NameVoucher = voucher.VoucherCode,
                        Quantity = voucher.UseLimit,
                        StartDate = voucher.StartDate,
                        Status = voucher.Status,
                        Unit = voucher.DiscountType == 1 ? true : false,
                        Value = voucher.DiscountValue,
                        VoucherId = voucher.Id
                    };
                    lstVoucher.Add(VoucherResponse);
                }
            }
            return lstVoucher;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateVoucher([FromBody]CreateVoucher voucher)
        {
            var voucher2=new Voucher()
            {
                CreatedAt = DateTime.Now,
                DiscountMaxValue = voucher.DiscountMaxValue,
                DiscountType = voucher.DiscountType,
                DiscountValue = voucher.DiscountValue,
                EndDate = voucher.EndDate,
                ModifiedAt = DateTime.Now,
                RequriedValue = voucher.RequriedValue,
                UseLimit = voucher.UseLimit,
                VoucherCode=voucher.VoucherCode,
                VoucherContent=voucher.VoucherContent,
                UsePerPerson=voucher.UsePerPerson,
                StatusVoucher = voucher.StatusVoucher,
                StartDate = voucher.StartDate,
            };
            if (_context.Vouchers.FirstOrDefault(x => x.VoucherCode == voucher.VoucherCode) != null)
            {
                return BadRequest(new Response<int> { Status = "Success", Message = "Tên voucher đã tồn tại" });
            };
            var result = _context.Vouchers.Add(voucher2);
            _context.SaveChanges();
            return Ok(new Response<int> { Status = "Success", Message = "Thêm mới thành công" });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateVoucher([FromBody] UpdateVoucher voucher)
        {

            var vou=_context.Vouchers.FirstOrDefault(x=>x.Id == voucher.Id);
            vou.UsePerPerson = voucher.UsePerPerson;
            vou.StatusVoucher = voucher.StatusVoucher;
            vou.StartDate = voucher.StartDate;
            vou.EndDate = voucher.EndDate;
            vou.VoucherCode = voucher.VoucherCode;
            vou.UsePerPerson = voucher.UsePerPerson;    
            vou.DiscountMaxValue = voucher.DiscountMaxValue;
            vou.DiscountType = voucher.DiscountType;
            vou.UseLimit= voucher.UseLimit;
            
            if (_context.Vouchers.FirstOrDefault(x => x.VoucherCode == voucher.VoucherCode &&x.Id!=voucher.Id) != null)
            {
                return BadRequest(new Response<int> { Status = "Success", Message = "Tên voucher đã tồn tại" });
            };
            _context.Vouchers.Update(vou);
            _context.SaveChanges();
            return Ok(new Response<int> { Status = "Success", Message = "Thêm mới thành công" });
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateVoucher(Voucher voucher)
        //{
        //    var result = _ServiceVoucher.Update(voucher);
        //    return Ok(result);
        //}

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var result = _context.Vouchers.FirstOrDefault(x=>x.Id== id);
            if(result == null) return BadRequest(new Response<int> { Status = "Success", Message = "Không tìm thấy  voucher!" });

            _context.Vouchers.Remove(result);
            _context.SaveChanges();
            return Ok(new Response<int> { Status = "Success", Message = "Xóa  thành công" });
        }
    }
}
