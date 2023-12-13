using BUS.IService;
using DAL.Models;
using DTO.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ShoeStoreContext _context;

        public CartController(ShoeStoreContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddCart(CartDTO cartDTO)
        {
            try
            {
                var cartDAL = _context.Carts.FirstOrDefault(c => c.UserId == cartDTO.UserId);
                Cart cart = new Cart();
                if (cartDAL == null)
                {
                    cart.UserId = cartDTO.UserId;
                    cart.TotalItem = cartDTO.cartItemDTOs.Count;
                    cart.Status = 0;
                    cart.CreatedAt = DateTime.Now;
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }
                else
                {
                    cart = cartDAL;
                    cart.TotalItem += cartDTO.cartItemDTOs.Count;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                foreach (var item in cartDTO.cartItemDTOs)
                {
                    var cartItemDAL = _context.CartItems.FirstOrDefault(c => c.CartId == cart.Id && c.ProductVariantId == item.ProductVariantId);
                    var shoevariant = _context.ShoesVariants.FirstOrDefault(c=>c.Id == item.ProductVariantId);
                    if (cartItemDAL != null)
                    {
                        cartItemDAL.Quantity += item.Quantity;
                        if (cartItemDAL.Quantity > shoevariant.Stock)
                        {
                            return BadRequest("số lượng vượt quá");
                        }
                        _context.CartItems.Update(cartItemDAL);
                    }
                    else
                    {
                        CartItem cartItem = new CartItem();
                        cartItem.CartId = cart.Id;
                        cartItem.ProductVariantId = item.ProductVariantId;
                        cartItem.Quantity = item.Quantity;
                        cartItem.Price = item.Price;
                        cartItem.CreatedAt = DateTime.Now;
                        _context.CartItems.Add(cartItem);
                    }
                }
                _context.SaveChanges();
                return Ok(new { Status = StatusCodes.Status200OK, Message = "Thêm thành công" });
            }
            catch (Exception)
            {
                return Ok(new { Status = StatusCodes.Status400BadRequest, Message = "Thêm không thành công" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cartDAL = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cartDAL == null)
            {
                return Ok(new { Status = StatusCodes.Status400BadRequest, Message = $"không có cart cho user có id {userId}" });
            }
            var galleries = await _context.Galleries.ToListAsync();
            CartDTO cartDTO = new CartDTO();
            cartDTO.UserId = userId;
            cartDTO.TotalItem = cartDAL.TotalItem;
            cartDTO.Id = cartDAL.Id;
            var cartItems = _context.CartItems.Include(c => c.ProductVariant).Where(c => c.CartId == cartDAL.Id);
            foreach (var item in cartItems)
            {
                CartItemDTO cartItemDTO = new CartItemDTO();
                cartItemDTO.CartId = cartDAL.Id;
                cartItemDTO.Id = item.Id;
                cartItemDTO.ProductVariantId = item.ProductVariantId;
                cartItemDTO.VariantName = item.ProductVariant.VariantName;
                cartItemDTO.Color = _context.Colors.FirstOrDefault(n=>n.Id==item.ProductVariant.Color)?.ColorName;
                cartItemDTO.Size = _context.Sizes.FirstOrDefault(n => n.Id == item.ProductVariant.Size)?.Size1;
                cartItemDTO.Description = item.ProductVariant.Description;
                if (item.ProductVariant.ImageId != null)
                {
                    var images = JsonConvert.DeserializeObject<List<int>>(item.ProductVariant.ImageId);
                    cartItemDTO.Image = galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url;
                }
                cartItemDTO.Quantity = item.Quantity;
                cartItemDTO.Price = item.Price;
                cartDTO.cartItemDTOs.Add(cartItemDTO);
            }
            return Ok(new { Status = StatusCodes.Status200OK, Payload = cartDTO });
        }
        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, CartItemDTO cartItem)
        {
            try
            {
                if (cartItemId == 0 || cartItemId == null)
                {
                    return BadRequest(new { Status = StatusCodes.Status400BadRequest, Message = "Sửa không thành công khi không truyền cartId" });
                }
                var cartDAL = _context.CartItems.Include(c => c.ProductVariant).FirstOrDefault(c => c.Id == cartItemId);
                if (cartDAL == null)
                {
                    return BadRequest(new { Status = StatusCodes.Status400BadRequest, Message = $"không có cart với id {cartItemId}" });
                }
                var shoes = _context.ShoesVariants.FirstOrDefault(x => x.Id == cartItem.ProductVariantId);
                if (cartItem.Quantity > shoes.Stock) {
                    return BadRequest(new { Status = StatusCodes.Status400BadRequest, Message = $"Số lượng vượt quá số lượng trong kho, Số lượng trong kho còn lại là: {shoes.Stock}" });
                }
                cartDAL.Quantity = cartItem.Quantity;
                _context.CartItems.Update(cartDAL);
                _context.SaveChanges();
                return Ok(new { Status = StatusCodes.Status200OK, Message = "Sửa thành công" });
            }
            catch (Exception)
            {
                return Ok(new { Status = StatusCodes.Status400BadRequest, Message = "Sửa không thành công" });
            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCartItem(List<CartItemDTO> cartItems)
        {
            try
            {
                if (cartItems == null)
                {
                    return Ok(new { Status = StatusCodes.Status400BadRequest, Message = "không có cart" });
                }
                foreach (var item in cartItems)
                {
                    var cartDAL = _context.CartItems.Include(c => c.ProductVariant).FirstOrDefault(c => c.Id == item.Id);
                    _context.CartItems.Remove(cartDAL);
                }
                _context.SaveChanges();
                return Ok(new { Status = StatusCodes.Status200OK, Message = "Xoá thành công" });
            }
            catch (Exception)
            {
                return Ok(new { Status = StatusCodes.Status400BadRequest, Message = "Xoá không thành công" });
            }
        }
    }
}
