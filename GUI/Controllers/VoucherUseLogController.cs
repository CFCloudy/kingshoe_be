using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Shoe.Controllers
{
    public class VoucherUseLogController : Controller
    {
        private readonly ShoeStoreContext _context;

        public VoucherUseLogController(ShoeStoreContext context)
        {
            _context = context;
        }

        // GET: VouchersUseLogs
        public async Task<IActionResult> Index()
        {
            var shoeStoreContext = _context.VouchersUseLogs.Include(v => v.Order);
            return View(await shoeStoreContext.ToListAsync());
        }

        // GET: VouchersUseLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VouchersUseLogs == null)
            {
                return NotFound();
            }

            var vouchersUseLog = await _context.VouchersUseLogs
                .Include(v => v.Order)
                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vouchersUseLog == null)
            {
                return NotFound();
            }

            return View(vouchersUseLog);
        }

        // GET: VouchersUseLogs/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["VoucherUserId"] = new SelectList(_context.VoucherBanks, "Id", "Id");
            return View();
        }

        // POST: VouchersUseLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VoucherUserId,OrderId,CreatedAt,ModifiedAt,Status")] VouchersUseLog vouchersUseLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vouchersUseLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", vouchersUseLog.OrderId);
            ViewData["VoucherUserId"] = new SelectList(_context.VoucherBanks, "Id", "Id", vouchersUseLog.VoucherUserId);
            return View(vouchersUseLog);
        }

        // GET: VouchersUseLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VouchersUseLogs == null)
            {
                return NotFound();
            }

            var vouchersUseLog = await _context.VouchersUseLogs.FindAsync(id);
            if (vouchersUseLog == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", vouchersUseLog.OrderId);
            ViewData["VoucherUserId"] = new SelectList(_context.VoucherBanks, "Id", "Id", vouchersUseLog.VoucherUserId);
            return View(vouchersUseLog);
        }

        // POST: VouchersUseLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VoucherUserId,OrderId,CreatedAt,ModifiedAt,Status")] VouchersUseLog vouchersUseLog)
        {
            if (id != vouchersUseLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vouchersUseLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VouchersUseLogExists(vouchersUseLog.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", vouchersUseLog.OrderId);
            ViewData["VoucherUserId"] = new SelectList(_context.VoucherBanks, "Id", "Id", vouchersUseLog.VoucherUserId);
            return View(vouchersUseLog);
        }

        // GET: VouchersUseLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VouchersUseLogs == null)
            {
                return NotFound();
            }

            var vouchersUseLog = await _context.VouchersUseLogs
                .Include(v => v.Order)
               
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vouchersUseLog == null)
            {
                return NotFound();
            }

            return View(vouchersUseLog);
        }

        // POST: VouchersUseLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VouchersUseLogs == null)
            {
                return Problem("Entity set 'ShoeStoreContext.VouchersUseLogs'  is null.");
            }
            var vouchersUseLog = await _context.VouchersUseLogs.FindAsync(id);
            if (vouchersUseLog != null)
            {
                _context.VouchersUseLogs.Remove(vouchersUseLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VouchersUseLogExists(int id)
        {
            return (_context.VouchersUseLogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
