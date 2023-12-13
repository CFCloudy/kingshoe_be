using DAL.Models;
using DTO.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersUseLogController : ControllerBase
    {
        private readonly ShoeStoreContext _context;

        public VouchersUseLogController(ShoeStoreContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddVouchersUseLog(CreateVouchersUseLog createVouchersUseLog)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(c => c.Id == createVouchersUseLog.OrderId);
                if (order == null)
                {
                    return BadRequest("không có order");
                }
                var orderIdByUser = _context.Orders.Where(c => c.UserId == order.UserId).Select(c => c.Id);
                var limitVoucher = _context.Vouchers.FirstOrDefault(c=>c.Id == createVouchersUseLog.VoucherUserId).UseLimit;
                var countVouchersUseLogs = _context.VouchersUseLogs.Where(c => orderIdByUser.Contains(c.OrderId.Value) && c.VoucherUserId == createVouchersUseLog.VoucherUserId).Count();
                if (countVouchersUseLogs >= limitVoucher)
                {
                    return BadRequest("user không sử dụng được voucher này nữa vì hết lượt sử dụng");
                }
                var vouchersUseLog = new VouchersUseLog();
                vouchersUseLog.Id = 0;
                vouchersUseLog.VoucherUserId = createVouchersUseLog.VoucherUserId;
                vouchersUseLog.OrderId = createVouchersUseLog.OrderId;
                vouchersUseLog.Status = false;
                vouchersUseLog.Price = createVouchersUseLog.Price;
                vouchersUseLog.CreatedAt = DateTime.Now;
                vouchersUseLog.ModifiedAt = DateTime.Now;
                _context.VouchersUseLogs.Add(vouchersUseLog);
                _context.SaveChanges();
                return Ok(new { Status = 200, Message = "Thêm thanh công" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
