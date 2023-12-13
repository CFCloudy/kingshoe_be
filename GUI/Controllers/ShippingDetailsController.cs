using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GUI.Models;
using DAL.Models;

namespace GUI.Controllers
{
    public class ShippingDetailsController : Controller
    {
        private readonly ShoeStoreContext _context;

        public ShippingDetailsController(ShoeStoreContext context)
        {
            _context = context;
        }

        // GET: ShippingDetails
        public async Task<IActionResult> Index(string key = "", int page = 1, int rowPerPage = 5)
        {
            page--;
            ViewBag.SearchKey = key;
            var shoeStoreContext = await _context.ShippingDetails.Include(s => s.Order).Where(c => key == "" || c.OrderId.ToString() == key || c.ShippingAddress == key || c.ShippingName == key || c.ShippingPhone == key).ToListAsync();
            ViewBag.UserList = _context.UserProfiles.ToList();
            ViewBag.CurrentPage = page + 1;
            ViewBag.TopPage = (shoeStoreContext.Count() / rowPerPage) < 1 ? 1 : shoeStoreContext.Count() / rowPerPage;
            return View(shoeStoreContext.Skip(page * rowPerPage).Take(rowPerPage).ToList());
        }

        // GET: ShippingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShippingDetails == null)
            {
                return NotFound();
            }

            var shippingDetail = await _context.ShippingDetails
                .Include(s => s.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippingDetail == null)
            {
                return NotFound();
            }

            return View(shippingDetail);
        }

        // GET: ShippingDetails/Create
        public IActionResult Create()
        {
            //ViewData["OrderId"] = new SelectList(_context.Orders.Where(c=> !_context.ShippingDetails.Select(c=>c.OrderId).ToList().Contains(c.Id)), "OrderCode", "OrderCode");
            return View();
        }

        // POST: ShippingDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public async Task<JsonResult> CheckOrderCode(string id)
        {
            try
            {
                var orderList = _context.Orders.Where(c => c.OrderCode.Trim() == id.Trim()).ToList();
                if (orderList != null && orderList.Any())
                {
                    var getOrder = orderList.First();
                    if (_context.ShippingDetails.Any(c=>c.OrderId == getOrder.Id))
                    {
                        return Json(new
                        {
                            success = false,
                            orderId = -1
                        });
                    }
                    return Json(new
                    {
                        success = true,
                        orderId = getOrder.Id
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        orderId = 0
                    });
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    orderId = -2
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,UserId,OrderId,ShippingName,ShippingAddress,ShippingPhone,SenderName,SenderAddress,SenderPhone,OrderNote,Status,CreatedAt,ModifiedAt")] DAL.Models.ShippingDetail shippingDetail)
        {
            shippingDetail.UserId = _context.Orders.First(c => c.Id == shippingDetail.OrderId).UserId;
            shippingDetail.CreatedAt = DateTime.Now;
            shippingDetail.ModifiedAt = DateTime.Now;
            shippingDetail.Status = 0;
            _context.Add(shippingDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ShippingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShippingDetails == null)
            {
                return NotFound();
            }

            var shippingDetail = await _context.ShippingDetails.FindAsync(id);
            if (shippingDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", shippingDetail.OrderId);
            return View(shippingDetail);
        }

        // POST: ShippingDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OrderId,ShippingName,ShippingAddress,ShippingPhone,OrderNote,Status,CreatedAt,ModifiedAt")] DAL.Models.ShippingDetail shippingDetail)
        {
            if (id != shippingDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingDetailExists(shippingDetail.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", shippingDetail.OrderId);
            return View(shippingDetail);
        }

        // GET: ShippingDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShippingDetails == null)
            {
                return NotFound();
            }

            var shippingDetail = await _context.ShippingDetails
                .Include(s => s.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippingDetail == null)
            {
                return NotFound();
            }

            return View(shippingDetail);
        }

        // POST: ShippingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShippingDetails == null)
            {
                return Problem("Entity set 'ShoeStoreContext.ShippingDetails'  is null.");
            }
            var shippingDetail = await _context.ShippingDetails.FindAsync(id);
            if (shippingDetail != null)
            {
                _context.ShippingDetails.Remove(shippingDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingDetailExists(int id)
        {
          return (_context.ShippingDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
