using AutoMapper;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shoe.Controllers
{
    public class VoucherController : Controller
    {
        private readonly ShoeStoreContext _context;
        private Service_Voucher _ServiceVoucher;
        private readonly IMapper _mapper;
        public VoucherController(ShoeStoreContext context, Service_Voucher _ServiceVoucher, IMapper mapper)
        {
            this._ServiceVoucher = _ServiceVoucher;
            _mapper = mapper;
            _context = context;
        }

        // GET: Vouchers
        public async Task<IActionResult> Index(int page = 1, string key = null/*string key = "", int page = 1, int rowPerPage = 5*/) //rowPerPage bao nhiêu dòng 1 trang
        {
            
            var ListVoucher = await _ServiceVoucher.GetAllVoucher()/*_ServiceVoucher.GetByRequest(/*key,page,rowPerPage*/;
            int TotalPage = 0;
            if (ListVoucher.Count % 5 == 0)
            {
                 TotalPage = ListVoucher.Count / 5;
            }
            else
            {
                TotalPage = (ListVoucher.Count / 5) + 1;
            }
            var ListVouchers = new List<GUI.Models.Voucher>();
            int skipCount = (page - 1) * 5;
            if (key == null)
            {
                foreach (var item in ListVoucher.Skip(skipCount).Take(5))
                {
                    ListVouchers.Add(_mapper.Map<Voucher, GUI.Models.Voucher>(item));

                }
            }
            else
            {
                TotalPage = ListVoucher.Where(c => c.VoucherCode.Contains(key)).Count();
                if (ListVoucher.Count % 5 == 0)
                {
                    TotalPage = ListVoucher.Count / 5;
                }
                else
                {
                    TotalPage = (ListVoucher.Count / 5) + 1;
                }
                foreach (var item in ListVoucher.Where(c => c.VoucherCode.Contains(key)).Skip(skipCount).Take(5))
                {
                    ListVouchers.Add(_mapper.Map<Voucher, GUI.Models.Voucher>(item));
                }
            }
            ViewBag.Page = page;
            ViewBag.totalPage = TotalPage;
            //var tongtrang = Math.Ceiling(Convert.ToDecimal(ListVoucher.Item2/rowPerPage));
            //ViewBag.Tongtrang = tongtrang;
            //ViewBag.TrangHT = page;
            //ViewBag.DongMoiTrang = rowPerPage;
            ViewBag.Search = key;
            
            return View(ListVouchers);
            
        }

        // GET: Vouchers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.Id == id);
            var entity = _mapper.Map<Voucher, GUI.Models.Voucher>(voucher);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Vouchers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vouchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VoucherContent,DiscountType,DiscountValue,DiscountMaxValue,VoucherCode,Status,StatusVoucher,UsePerPerson,UseLimit,CreatedAt,ModifiedAt,StartDate,EndDate,RequriedValue")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                _ServiceVoucher.Insert(voucher);
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        public async Task<int> CountTotal(List<int> lstVoucherId, int Toatal = 5000000)
        {
            lstVoucherId.Add(4);
            lstVoucherId.Add(6);
            lstVoucherId.Add(7);
            var lstVoucher =  _context.Vouchers.ToList();
            foreach (var item in lstVoucherId)
            {
                var voucher = lstVoucher.FirstOrDefault(c => c.Id == item);
                if(voucher.DiscountType == 2)
                {
                    if(Convert.ToInt32(voucher.RequriedValue) <= Toatal)
                    {
                        Toatal = Toatal - Convert.ToInt32(voucher.DiscountValue);
                    }
                }
                else
                {
                    if (Convert.ToInt32(voucher.RequriedValue) <= Toatal)
                    {
                        int value = (Toatal * Convert.ToInt32(voucher.DiscountValue)) / 100;
                        if (value <= Convert.ToInt32(voucher.DiscountMaxValue))
                        {
                            Toatal = Toatal - value;
                        }
                        else
                        {
                            Toatal = Toatal - Convert.ToInt32(voucher.DiscountMaxValue);
                        }
                    }

                }

                if(Toatal < 0)
                {
                    Toatal = 0;
                }
            }
            return Toatal;
        }

        // GET: Vouchers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            var entity = _mapper.Map<Voucher, GUI.Models.Voucher>(voucher);
            return View(entity);
        }

        // POST: Vouchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VoucherContent,DiscountType,DiscountValue,DiscountMaxValue,VoucherCode,Status,StatusVoucher,UsePerPerson,UseLimit,CreatedAt,ModifiedAt,StartDate,EndDate,RequriedValue")] Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        // GET: Vouchers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.Id == id);
            var entity = _mapper.Map<Voucher, GUI.Models.Voucher>(voucher);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vouchers == null)
            {
                return Problem("Entity set 'ShoeStoreContext.Vouchers'  is null.");
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            var voucherBank =  _context.VoucherBanks.Where(c => c.VoucherId == id).ToList();
            if (voucher != null)
            {
                voucher.Status = 1;
                _context.Update(voucher);
                foreach (var item in voucherBank)
                {
                    var entity = await _context.VoucherBanks.FindAsync(item.Id);
                    entity.Status = false;
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(int id)
        {
            return (_context.Vouchers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
    //public IActionResult Index()
    //{
    //    return View();
    //}
}

