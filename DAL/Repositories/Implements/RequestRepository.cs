using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class RequestRepository
    {
        private readonly ShoeStoreContext _context;
        public RequestRepository()
        {
            _context = new ShoeStoreContext();
        }

        public Tuple<List<Order>, int> GetListOrderByRequest(string key, int skip, int take)
        {
            try
            {
                var data = _context.Orders.AsNoTracking().Where(c=>c.Id.ToString() == key);
                var total = data.Count();
                var result = data.Skip(skip).Take(take).ToList();
                return Tuple.Create(result, total);
            }
            catch (Exception)
            {
                return new FailedQuery<Order>().GetListFailedRequest();
            }
        }

        public Tuple<List<OrderItem>, int> GetListOrderItemsByRequest(string key, int skip, int take)
        {
            try
            {
                var data = _context.OrderItems.AsNoTracking().Where(c => c.Id.ToString() == key || c.OrderId.ToString() == key || c.ProductVariantId.ToString() == key);
                var total = data.Count();
                var result = data.Skip(skip).Take(take).ToList();
                return Tuple.Create(result, total);
            }
            catch (Exception)
            {
                return new FailedQuery<OrderItem>().GetListFailedRequest();
            }
        }

        public Tuple<List<Cart>, int> GetListCartsByRequest(string key, int skip, int take)
        {
            try
            {
                var data = _context.Carts.AsNoTracking().Where(c => c.Id.ToString() == key || c.UserId.ToString() == key);
                var total = data.Count();
                var result = data.Skip(skip).Take(take).ToList();
                return Tuple.Create(result, total);
            }
            catch (Exception)
            {
                return new FailedQuery<Cart>().GetListFailedRequest();
            }
        }

        public Tuple<List<CartItem>, int> GetListCartItemsByRequest(string key, int skip, int take)
        {
            try
            {
                var data = _context.CartItems.AsNoTracking().Where(c => c.Id.ToString() == key || c.CartId.ToString() == key || c.ProductVariantId.ToString() == key);
                var total = data.Count();
                var result = data.Skip(skip).Take(take).ToList();
                return Tuple.Create(result, total);
            }
            catch (Exception)
            {
                return new FailedQuery<CartItem>().GetListFailedRequest();
            }
        }

        public Tuple<List<ShippingDetail>, int> GetListOrderDetailsByRequest(string key, int skip, int take)
        {
            try
            {
                var data = _context.ShippingDetails.AsNoTracking().Where(c => c.Id.ToString() == key || c.OrderId.ToString() == key || c.ShippingName == key || c.ShippingPhone == key);
                var total = data.Count();
                var result = data.Skip(skip).Take(take).ToList();
                return Tuple.Create(result, total);
            }
            catch (Exception)
            {
                return new FailedQuery<ShippingDetail>().GetListFailedRequest();
            }
        }

        public Order GetOneOrderByID(int ID)
        {
            try
            {
                var result = _context.Orders.AsNoTracking().FirstOrDefault(o => o.Id == ID);
                return result ?? new Order();
            }
            catch (Exception)
            {
                return new Order();
            }
        }

        public OrderItem GetOneOrderItemByID(int ID)
        {
            try
            {
                var result = _context.OrderItems.AsNoTracking().FirstOrDefault(o => o.Id == ID);
                return result ?? new OrderItem();
            }
            catch (Exception)
            {
                return new OrderItem();
            }
        }
        public Cart GetOneCartByID(int ID)
        {
            try
            {
                var result = _context.Carts.AsNoTracking().FirstOrDefault(o => o.Id == ID);
                return result ?? new Cart();
            }
            catch (Exception)
            {
                return new Cart();
            }
        }

        public CartItem GetOneCartItemByID(int ID)
        {
            try
            {
                var result = _context.CartItems.AsNoTracking().FirstOrDefault(o => o.Id == ID);
                return result ?? new CartItem();
            }
            catch (Exception)
            {
                return new CartItem();
            }
        }
        public ShippingDetail GetOneOrderDetailByID(int ID)
        {
            try
            {
                var result = _context.ShippingDetails.AsNoTracking().FirstOrDefault(o => o.Id == ID);
                return result ?? new ShippingDetail();
            }
            catch (Exception)
            {
                return new ShippingDetail();
            }
        }
        internal class FailedQuery<T> where T : class
        {
            public FailedQuery() { }
            public Tuple<List<T>, int> GetListFailedRequest()
            {
                return Tuple.Create(new List<T>(), -1);
            }
        }

    }
}
