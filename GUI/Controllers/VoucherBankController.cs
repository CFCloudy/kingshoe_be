using AutoMapper;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GUI.Controllers
{
    public class VoucherBankController : Controller
    {
        private readonly ShoeStoreContext _context;
        private readonly IMapper mapper;
        private IServiceVoucher_Bus serviceVoucher_Bus;
        private IServiceVoucherBank serviceVoucherBank;
        private Service_Voucher _ServiceVoucher;

        public VoucherBankController(ShoeStoreContext context, Service_Voucher _ServiceVoucher, IServiceVoucherBank serviceVoucherBank, IServiceVoucher_Bus serviceVoucher_Bus, IMapper _mapper)
        {
            this.serviceVoucherBank = serviceVoucherBank;
            this._ServiceVoucher = _ServiceVoucher;
            this.serviceVoucher_Bus = serviceVoucher_Bus;
            mapper = _mapper;
            _context = context;
        }

        // GET: VoucherBanks
        public async Task<IActionResult> Index(int page = 1, string key = null)
        {
            var voucherBank = await serviceVoucherBank.GetAll();
            var voucher = await _ServiceVoucher.GetAllVoucher();
            //var user = await serviceVoucherBank.GetAll();
            var voucherbanks = new List<Models.VoucherBank>();
            foreach (var item in voucherBank)
            {
                var entity = mapper.Map<VoucherBank, Models.VoucherBank>(item);
                entity.VoucherName = voucher.FirstOrDefault(c => c.Id == entity.VoucherId).VoucherContent;
                //entity.UserName = user.FirstOrDefault(c => c.Id == entity.UserId).Name;
                voucherbanks.Add(entity);
            }
            
            return View(voucherbanks);
            //var shoeStoreContext = _context.VoucherBanks.Include(v => v.User).Include(v => v.Voucher);
            //return View(await shoeStoreContext.ToListAsync());
        }

        // GET: VoucherBanks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VoucherBanks == null)
            {
                return NotFound();
            }

            var voucherBank = await _context.VoucherBanks
                //.Include(v => v.User)
                //.Include(v => v.Voucher)
                .FirstOrDefaultAsync(m => m.Id == id);

            var entity = mapper.Map<VoucherBank, Models.VoucherBank>(voucherBank);
            if (voucherBank == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: VoucherBanks/Create
        public async Task<IActionResult> Create()
        {
            var voucher = await GetVouchers();
            ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Id");
            ViewData["VoucherId"] = new SelectList(_context.Vouchers.Where(c => c.Status.Value != 1 && c.StatusVoucher != 0 && c.EndDate > DateTime.Now), "Id", "VoucherContent");
            return View();
        }

        public async Task<List<Models.Voucher>> GetVouchers()
        {
            var voucher = new List<Models.Voucher>();
            { new Models.Voucher(); };
            var voucherDAL = await serviceVoucher_Bus.GetAllVoucher();
            foreach (var vouchers in voucherDAL)
            {
                voucher.Add(mapper.Map<DAL.Models.Voucher, Models.Voucher>(vouchers));
            }
            return voucher;
        }

        // POST: VoucherBanks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VoucherId,UserId,Quantity,CreatedAt,ModifiedAt,Status")] VoucherBank voucherBank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voucherBank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Password", voucherBank.UserId);
            ViewData["VoucherId"] = new SelectList(_context.Vouchers, "Id", "Id", voucherBank.VoucherId);
            return View(voucherBank);
        }

        // GET: VoucherBanks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VoucherBanks == null)
            {
                return NotFound();
            }

            var voucherBank = await _context.VoucherBanks.FindAsync(id);
            if (voucherBank == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Password", voucherBank.UserId);
            ViewData["VoucherId"] = new SelectList(_context.Vouchers, "Id", "Id", voucherBank.VoucherId);
            return View(voucherBank);
        }

        // POST: VoucherBanks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VoucherId,UserId,Quantity,CreatedAt,ModifiedAt,Status")] VoucherBank voucherBank)
        {
            if (id != voucherBank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucherBank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherBankExists(voucherBank.Id))
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
            ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Password", voucherBank.UserId);
            ViewData["VoucherId"] = new SelectList(_context.Vouchers, "Id", "Id", voucherBank.VoucherId);
            return View(voucherBank);
        }

        // GET: VoucherBanks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VoucherBanks == null)
            {
                return NotFound();
            }

            var voucherBank = await _context.VoucherBanks
                .Include(v => v.User)
                .Include(v => v.Voucher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucherBank == null)
            {
                return NotFound();
            }

            return View(voucherBank);
        }

        // POST: VoucherBanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VoucherBanks == null)
            {
                return Problem("Entity set 'ShoeStoreContext.VoucherBanks'  is null.");
            }
            var voucherBank = await _context.VoucherBanks.FindAsync(id);
            if (voucherBank != null)
            {
                _context.VoucherBanks.Remove(voucherBank);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherBankExists(int id)
        {
            return (_context.VoucherBanks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
