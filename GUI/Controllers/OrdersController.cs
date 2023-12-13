using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using GUI.Models;
using DAL.Models;
using DTO.Simple;

namespace GUI.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ShoeStoreContext _context;

        public OrdersController(ShoeStoreContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string key = "", int page = 1, int rowPerPage = 1)
        {
            page = page--;
            ViewBag.SearchKey = key;
            var shoeStoreContext = _context.Orders.Include(o => o.User).Where(c=> key == "" || c.Id.ToString() == key || c.OrderCode == key);
            ViewBag.CurrentPage = page;
            ViewBag.TopPage = shoeStoreContext.Count() / rowPerPage;
            return View(await shoeStoreContext.Skip(page * rowPerPage).Take(rowPerPage).ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public List<VariantView> getListVariantsByProd(int prodID)
        {
            var listVariants = new List<VariantView>();
            try
            {
                var obj = new VariantView();
                obj.ProductID = prodID;
                var prod = _context.Shoes.First(c => c.Id == prodID);
                foreach (var item in _context.ShoesVariants.Where(c => c.ProductId == prodID))
                {
                    obj.ProductVariantID = item.Id;
                    obj.ProductName = prod.ProductName + " " + item.VariantName;
                    listVariants.Add(obj);
                }
            }
            catch (Exception)
            {
            }
            return listVariants;
        }

        [HttpGet]
        public decimal getTotal(int varId, int quant)
        {
            try
            {
                return _context.ShoesVariants.First(c => c.Id == varId).DisplayPrice * quant;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Username");
            ViewData["ListProd"] = new SelectList(_context.Shoes, "Id", "ProductName");
            var code = Guid.NewGuid();
            while (_context.Orders.Where(c => c.OrderCode == code.ToString()).Any())
            {
                code = Guid.NewGuid();
            }
            ViewBag.OrderCode = code;
            //ViewData["ListVariant"] = prId == 0 ? null : new SelectList(_context.ShoesVariants.Where(c => c.ProductId == prId), "Id", "VariantName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderView order, bool isShip = false)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(order);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Username");
            //ViewData["ListProd"] = new SelectList(_context.Shoes, "Id", "ProductName");
            //return View(order);

            try
            {
                var InsertOrder = new Order();
                InsertOrder.Id = 0;
                InsertOrder.OrderCode = order.OrderCode;
                InsertOrder.UserId = order.UserID == 0 ? null : order.UserID;
                InsertOrder.Total = order.Total;
                InsertOrder.Status = 0;
                _context.Add(InsertOrder);
                _context.SaveChanges();
                var orderID = _context.Orders.First(c => c.OrderCode == order.OrderCode).Id;
                foreach (var item in order.ListItems)
                {
                    var InsertItem = new OrderItem();
                    InsertItem.OrderId = orderID;
                    InsertItem.ProductVariantId = item.VariantID;
                    InsertItem.Quantity = item.Quantity;
                    InsertItem.Price = item.Price;
                    InsertItem.Status = 1;
                    _context.Add(InsertItem);
                    await _context.SaveChangesAsync();
                }
                if (isShip && order.ShippingDetails != null)
                {
                    var ViewShip = order.ShippingDetails;
                    var InsertShip = new ShippingDetail();
                    InsertShip.UserId = order.UserID == 0 ? null : order.UserID;
                    InsertShip.OrderId = orderID;
                    InsertShip.ShippingPhone = ViewShip.ShippingPhone;
                    InsertShip.ShippingAddress = ViewShip.ShippingAddress;
                    InsertShip.ShippingName = ViewShip.ShippingName;
                    InsertShip.OrderNote = ViewShip.OrderNote;
                    InsertShip.Status = 0;
                    _context.Add(InsertShip);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return Json(new
                {
                    succ = false,
                    mess = e.Message
                });
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(OrderView order, bool isShip = false)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(order);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Username");
            //ViewData["ListProd"] = new SelectList(_context.Shoes, "Id", "ProductName");
            //return View(order);

            try
            {
                var InsertOrder = new Order();
                InsertOrder.Id = 0;
                InsertOrder.OrderCode = order.OrderCode;
                InsertOrder.UserId = order.UserID == 0 ? null : order.UserID;
                InsertOrder.Total = order.Total;
                InsertOrder.Status = 0;
                _context.Add(InsertOrder);
                _context.SaveChanges();
                var orderID = _context.Orders.First(c => c.OrderCode == order.OrderCode).Id;
                foreach (var item in order.ListItems)
                {
                    var InsertItem = new OrderItem();
                    InsertItem.OrderId = orderID;
                    InsertItem.ProductVariantId = item.VariantID;
                    InsertItem.Quantity = item.Quantity;
                    InsertItem.Price = item.Price;
                    InsertItem.Status = 1;
                    _context.Add(InsertItem);
                    await _context.SaveChangesAsync();
                }
                if (isShip && order.ShippingDetails != null)
                {
                    var ViewShip = order.ShippingDetails;
                    var InsertShip = new ShippingDetail();
                    InsertShip.UserId = order.UserID == 0 ? null : order.UserID;
                    InsertShip.OrderId = orderID;
                    InsertShip.ShippingPhone = ViewShip.ShippingPhone;
                    InsertShip.ShippingAddress = ViewShip.ShippingAddress;
                    InsertShip.ShippingName = ViewShip.ShippingName;
                    InsertShip.OrderNote = ViewShip.OrderNote;
                    InsertShip.Status = 0;
                    _context.Add(InsertShip);
                    await _context.SaveChangesAsync();
                }
                return Json(new
                {
                    succ = true,
                    mess = ""
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    succ = false,
                    mess = e.Message
                });
            }

        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.UserId.HasValue)
            {
                ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Id", order.UserId); 
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Detail,OrderCode,Total,Status,CreatedAt,ModifiedAt")] DAL.Models.Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    order.ModifiedAt = DateTime.Now;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["UserId"] = new SelectList(_context.UserProfiles, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ShoeStoreContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
