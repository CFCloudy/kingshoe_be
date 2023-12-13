using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GUI.Models;
using BUS.Services.Implements;

namespace GUI.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly CartItemServices _context;
        private readonly CartServices _cartContext;

        public CartItemsController(CartItemServices context, CartServices cartContext)
        {
            _context = context;
            _cartContext = cartContext;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var shoeStoreContext = _context.GetAll();
            return View(await shoeStoreContext.ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GetAll == null)
            {
                return NotFound();
            }

            var cartItem = await _context.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_cartContext.GetAll(), "Id", "Id");
            //ViewData["ProductVariantId"] = new SelectList(_context.ShoesVariants, "Id", "Id");
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CartId,ProductVariantId,Quantity,Price,Status,CreatedAt,ModifiedAt")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                var cartItemData = new DAL.Models.CartItem();
                cartItemData.Status = cartItem.Status;
                cartItemData.Id = 0;
                cartItemData.Price = cartItem.Price;
                cartItemData.Quantity = cartItem.Quantity;
                cartItemData.ProductVariantId = cartItem.ProductVariantId;
                cartItemData.CartId = cartItem.CartId;
                cartItemData.CreatedAt = cartItem.CreatedAt;
                cartItemData.ModifiedAt = cartItem.ModifiedAt;

                _context.Create(cartItemData);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_cartContext.GetAll(), "Id", "Id", cartItem.CartId);
            //ViewData["ProductVariantId"] = new SelectList(_context.ShoesVariants, "Id", "Id", cartItem.ProductVariantId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GetAll() == null)
            {
                return NotFound();
            }

            var cartItem = await _context.GetAll().FirstOrDefaultAsync(c=>c.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_cartContext.GetAll(), "Id", "Id", cartItem.CartId);
            //ViewData["ProductVariantId"] = new SelectList(_context.ShoesVariants, "Id", "Id", cartItem.ProductVariantId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CartId,ProductVariantId,Quantity,Price,Status,CreatedAt,ModifiedAt")] CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cartItemData = new DAL.Models.CartItem();
                    cartItemData.Status = cartItem.Status;
                    cartItemData.Id = cartItem.Id;
                    cartItemData.Price = cartItem.Price;
                    cartItemData.Quantity = cartItem.Quantity;
                    cartItemData.ProductVariantId = cartItem.ProductVariantId;
                    cartItemData.CartId = cartItem.CartId;
                    cartItemData.CreatedAt = cartItem.CreatedAt;
                    cartItemData.ModifiedAt = cartItem.ModifiedAt;
                    _context.Update(cartItemData);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.Id))
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
            ViewData["CartId"] = new SelectList(_cartContext.GetAll(), "Id", "Id", cartItem.CartId);
            //ViewData["ProductVariantId"] = new SelectList(_context.ShoesVariants, "Id", "Id", cartItem.ProductVariantId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GetAll() == null)
            {
                return NotFound();
            }

            var cartItem = await _context.GetAll()
                .Include(c => c.Cart)
                .Include(c => c.ProductVariant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GetAll() == null)
            {
                return Problem("Entity set 'ShoeStoreContext.CartItems'  is null.");
            }
            var cartItem = await _context.GetAll().FirstOrDefaultAsync(c=>c.Id == id);
            if (cartItem != null)
            {
                _context.Delete(cartItem.Id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
          return _context.GetAll().Any(e => e.Id == id);
        }
    }
}
