using DTO.Utils;

using BUS.IServices;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTO.Simple;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace BUS.Services
{
    public class CustomerServices : ICustomerServices
    {
        //public IGenericRepository<UserProfile> _repoUserProfile;
        //public IGenericRepository<UserAddress> _repoUserAddress;
        public ShoeStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerServices(
           ShoeStoreContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            //_repoUserProfile=repoUserProfile;
            _context = context;
            //_repoUserAddress=repoUserAddress;
            _userManager = userManager;
        }
        public async Task<List<CustomerResponse>> GetAllCustomer(CustomerFilterDto input)
        {
            var listCustomer = _context.UserProfiles.ToList().Where(x => x.RoleId == 1)
                .Where(x => input.TenKhachHang == null || x.FullName.Contains(input.TenKhachHang)); 

            if (listCustomer.Any())
            {
                var _lst = listCustomer.Select(x => 
                {
                    var output = new CustomerResponse();
                    output.Address = _context.UserAddresses.ToList().FirstOrDefault(c => c.UserId == x.Id) != null ? _context.UserAddresses.ToList().FirstOrDefault(c => c.UserId == x.Id).AddressDetail : null;
                    output.FullName = x.FullName;
                    output.Id = x.Id;
                    List<ReturnOrder> returnOrder = new List<ReturnOrder>();
                    var lstOrder = _context.Orders.Where(c => c.UserId == x.Id).ToList();
                    returnOrder = lstOrder.Select(t =>
                    {
                        ReturnOrder nwReturnOrder = new ReturnOrder();
                        nwReturnOrder.maDonHang = t.OrderCode;
                        nwReturnOrder.orderId = t.Id;
                        var lstItem = _context.OrderItems.Where(x => x.OrderId == t.Id).ToList();
                        List<ReturnOrderItem> orderItem = new List<ReturnOrderItem>();
                        nwReturnOrder.items = lstItem.Select(c => {
                            ReturnOrderItem order = new ReturnOrderItem();
                            order.price = c.Price ?? 0;
                            order.quantity = c.Quantity ?? 0;
                            return order;
                        }).ToList();
                        nwReturnOrder.total = nwReturnOrder.items.Sum(x => x.price * x.quantity);
                        return nwReturnOrder;
                    }).ToList();
                    output.Money = returnOrder.Sum(x => x.total);
                   
                    if(returnOrder != null)
                    {
                        output.LastOrder = returnOrder.LastOrDefault()?.maDonHang ;
                        output.OrderId = returnOrder.LastOrDefault()?.orderId;
                    }
                    output.NumberOfOder = 0;
                    output.PhoneNumber = x.PhoneNumber;
                    output.CreatedAt = x.CreatedAt;
                    output.ModifiedAt = x.ModifiedAt;
                    output.NumberOfOder = returnOrder.Count();
                    output.IsLock=_userManager.FindByIdAsync(x.UserId).Result.LockoutEnabled;
                    output.UserId=_userManager.FindByIdAsync(x.UserId).Result.Id;
                    return output;
                }).OrderByDescending(x => x.CreatedAt).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                if (input.Sorting == "money")
                {
                    _lst=_lst.OrderByDescending(x => x.Money).ToList();
                }
                else if (input.Sorting == "money desc") {
                    _lst= _lst.OrderBy(x => x.Money).ToList();
                }
                return _lst;

            }
            else
            {
                return null;

            }

        }
        public async Task<CustomerDetail> GetCustomerDetails([FromBody]int id) {

            try
            {
                var customer = _context.UserProfiles.FirstOrDefault(x => x.Id == id);
                CustomerDetail customerDetails= new CustomerDetail();
                if (customer == null) {
                    return null;
                } else {
                    customerDetails.Id = id;
                    customerDetails.PhoneNumber = customer.PhoneNumber;
                    customerDetails.FullName = customer.FullName;
                    customerDetails.CreatedAt = customer.CreatedAt;

                    List<ReturnOrder >returnOrder = new List<ReturnOrder>();
                    var lstOrder = _context.Orders.Where(x => x.UserId == id).ToList();
                    returnOrder = lstOrder.Select(t =>
                    {
                        ReturnOrder nwReturnOrder = new ReturnOrder();
                        nwReturnOrder.orderId = t.Id;
                        nwReturnOrder.createDate = t.CreatedAt??DateTime.Now;
                        nwReturnOrder.maDonHang = t.OrderCode;
                        var lstItem = _context.OrderItems.Where(x => x.OrderId == t.Id).ToList();
                        List < ReturnOrderItem > orderItem=new List<ReturnOrderItem>();
                        nwReturnOrder.items = lstItem.Select(c=> {
                            var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == c.ProductVariantId);
                            var images = JsonConvert.DeserializeObject<List<int>>(shoeItem.ImageId);
                            ReturnOrderItem order = new ReturnOrderItem();
                            order.price = c.Price ?? 0;
                            order.quantity = c.Quantity ?? 0;
                            order.img = _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url;
                            order.color = _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName;
                            order.size = _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1;
                            order.variantId = c.ProductVariantId??0;
                            if (c.ProductVariantId != null && c.ProductVariantId > 0)
                            {
                                var id2 = _context.ShoesVariants.FirstOrDefault(n => n.Id == c.ProductVariantId).ProductId;
                                if (id2 != null)
                                {
                                    order.variantName = _context.Shoes.FirstOrDefault(x=>x.Id==id2).ProductName;
                                }
                            }
                            return order;
                        }).ToList();
                        nwReturnOrder.total = nwReturnOrder.items.Sum(x => x.price * x.quantity);
                        nwReturnOrder.Status = t.Status;
                        return nwReturnOrder;
                    }).ToList();
                    customerDetails.ListOrder = returnOrder;
                    customerDetails.Money = returnOrder.Sum(x => x.total);
                    customerDetails.NumberOfOder = returnOrder.Sum(x => x.orderId);
                }
                return customerDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
