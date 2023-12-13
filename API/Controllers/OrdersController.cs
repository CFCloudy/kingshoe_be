using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using GUI.Models;
using DAL.Models;
using DTO.Simple;
using Newtonsoft.Json;
using DTO;
using BUS.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using BUS.IService;

namespace API.Controllers
{


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly ShoeStoreContext _context;
        private readonly MailServices _mailServices;
        private IRepositoryShoeVariant_BUS _repositoryShoeVariant_BUS;

        public OrdersController(ShoeStoreContext context, IRepositoryShoeVariant_BUS repositoryShoeVariant_BUS)
        {
            _context = context;
            _mailServices = new MailServices();
            _repositoryShoeVariant_BUS = repositoryShoeVariant_BUS;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] OrderView order, bool isShip)
        {
            try
            {
                foreach (var item in order.ListItems)
                {
                    var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == item.VariantID);
                    if (shoeItem == null)
                    {
                        return BadRequest("không có variant");
                    }
                    if (shoeItem.Stock < item.Quantity)
                    {
                        return BadRequest("số luọng không đủ");
                    }
                }
                var InsertOrder = new Order();
                InsertOrder.Id = 0;
                InsertOrder.OrderCode = order.OrderCode;
                InsertOrder.UserId = order.UserID == 0 ? null : order.UserID;
                InsertOrder.Total = order.Total;
                InsertOrder.OrderCode = "HD00" + Convert.ToInt32(Convert.ToInt32(_context.Orders.OrderBy(x => x.CreatedAt).LastOrDefault().Id) + 1);
                InsertOrder.Status = 0;
                InsertOrder.CreatedAt = DateTime.Now;

                _context.Orders.Add(InsertOrder);
                await _context.SaveChangesAsync();
                var orderID = InsertOrder.Id;

                var retunObject = new ReturnOrder();
                retunObject.orderId = orderID;
                retunObject.cartId = 0;
                retunObject.total = order.Total;
                retunObject.items = new List<ReturnOrderItem>();
                retunObject.createDate = DateTime.Now;
                retunObject.maDonHang = InsertOrder.OrderCode;

                foreach (var item in order.ListItems)
                {
                    var InsertItem = new OrderItem();
                    InsertItem.OrderId = orderID;
                    InsertItem.ProductVariantId = item.VariantID;
                    InsertItem.Quantity = item.Quantity;
                    InsertItem.Price = item.Price;
                    InsertItem.Status = 1;
                    _context.OrderItems.Add(InsertItem);
                    var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == item.VariantID);
                    try
                    {
                        shoeItem.Stock -= item.Quantity;
                        _context.ShoesVariants.Update(shoeItem);
                        var images = JsonConvert.DeserializeObject<List<int>>(shoeItem.ImageId);
                        var returnItem = new ReturnOrderItem();
                        returnItem.quantity = item.Quantity;
                        returnItem.price = item.Price;
                        returnItem.variantId = item.VariantID;
                        returnItem.img = _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url;
                        returnItem.color = _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName;
                        returnItem.size = _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1;
                        retunObject.items.Add(returnItem);
                    }
                    catch (Exception)
                    {

                    }
                    await _context.SaveChangesAsync();



                }
                if (isShip && order.ShippingDetails != null)
                {
                    var UserAddresse = _context.UserAddresses.FirstOrDefault(c => c.Id == order.ShippingDetails.ShippingId);
                    var insertShip = new ShippingDetail();
                    insertShip.UserId = order.UserID == 0 ? null : order.UserID;
                    insertShip.OrderId = orderID;
                    insertShip.ShippingPhone = UserAddresse.PhoneNumber;
                    insertShip.ShippingName = UserAddresse?.Name;
                    insertShip.SenderName = "SP00" + Convert.ToInt32(Convert.ToInt32(_context.Orders.OrderBy(x => x.CreatedAt).LastOrDefault().Id) + 1);
                    insertShip.SenderPhone = UserAddresse?.PhoneNumber;
                    var addressDetail = UserAddresse?.AddressDetail;
                    var district = UserAddresse.District.Split('|')[UserAddresse.District.Split('|').Count() - 1];
                    var city = UserAddresse.City.Split('|')[UserAddresse.City.Split('|').Count() - 1]; ;
                    var ward = UserAddresse.Ward.Split('|')[UserAddresse.Ward.Split('|').Count() - 1]; ;
                    insertShip.ShippingAddress = $"{addressDetail}, {ward}, {district}, {city}";
                    insertShip.SenderAddress = $"{addressDetail}, {ward}, {district}, {city}";
                    insertShip.OrderNote = order.ShippingDetails?.OrderNote;
                    insertShip.Status = order.ShippingDetails?.Status;
                    _context.ShippingDetails.Add(insertShip);
                    await _context.SaveChangesAsync();
                }
                var x = new OrderHistoryLog()
                {
                    IdKhachHang = order.UserID == 0 ? null : order.UserID,
                    OrderId = orderID,
                    LogTime = DateTime.Now,
                    Message = "Đã tạo mới đơn hàng."
                };

                _context.OrderHistoryLogs.Add(x);
                _context.SaveChanges();
                await InvoiceSlip(orderID);
                return Ok(retunObject);


            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [HttpGet]
        public IActionResult GetOrdersByUserId(int uId = 0)
        {
            try
            {
                var listGet = _context.Orders.Where(c => uId == 0 || c.UserId == uId).OrderByDescending(x => x.CreatedAt).ToList();
                var returnList = new List<ReturnOrder>();
                foreach (var item in listGet)
                {
                    var obj = new ReturnOrder();
                    obj.orderId = item.Id;
                    obj.cartId = 0;
                    var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == item.Id);

                    obj.Status = item.Status;
                    obj.maDonHang = item.OrderCode;
                    var listGets = _context.OrderItems.Where(c => c.OrderId == item.Id).ToList();
                    if (listGets != null && listGets.Any())
                    {
                        foreach (var item1 in listGets)
                        {
                            var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == item1.ProductVariantId);
                            var images = JsonConvert.DeserializeObject<List<int>>(shoeItem.ImageId);
                            //var returnItem = new ReturnOrderItem(item1.ProductVariantId ?? -1, item1.Quantity ?? -1, item1.Price ?? -1, _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url, _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName, _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1);
                            var returnItem = new ReturnOrderItem();
                            returnItem.quantity = item1.Quantity ?? -1;
                            returnItem.price = item1.Price ?? -1;
                            returnItem.variantId = item1.ProductVariantId ?? -1;
                            returnItem.img = _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url;
                            returnItem.color = _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName;
                            returnItem.sum = item1.Quantity * item1.Price;
                            returnItem.size = _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1;
                            returnItem.variantName = _context.ShoesVariants.FirstOrDefault(x => x.Id == item1.ProductVariantId).VariantName;
                            obj.items.Add(returnItem);
                        }
                    }
                    obj.createDate = item.CreatedAt ?? DateTime.MinValue;
                    if (voucherUserLog != null)
                    {
                        obj.total = obj.items.Sum(x => x.sum) - voucherUserLog.Price.Value;
                    }
                    else
                    {
                        obj.total = obj.items.Sum(x => x.sum);
                    }
                    returnList.Add(obj);
                }
                return Ok(returnList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetOrderItemsById(int orderId = -1)
        {
            if (orderId == -1)
            {
                return BadRequest("Order ID doesn't exist");
            }
            else
            {
                try
                {
                    var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == orderId);
                    var listGet = _context.Orders.FirstOrDefault(c => orderId == 0 || c.Id == orderId);
                    var phieugH = _context.ShippingDetails.FirstOrDefault(x => x.OrderId == orderId);
                    var returnList = new List<ReturnOrder>();
                    var res = new ResponseOrder();
                    var listItem = new List<CartItems>();
                    if (listGet != null)
                    {
                        res.Status = listGet.Status;
                        res.IdAdress = listGet.Id;
                        res.GhiChu = "";
                        res.TenKhachHang = _context.UserProfiles.FirstOrDefault(x => x.Id == listGet.UserId)?.FullName;
                        res.IdKhachHang = listGet.UserId;
                        res.PhoneNumber = _context.UserProfiles.FirstOrDefault(x => x.Id == listGet.UserId)?.PhoneNumber;
                        res.Id = listGet.Id;
                        res.CreateAtTime = listGet.CreatedAt;
                        if (voucherUserLog != null)
                        {
                            res.SoTienDuocTru = voucherUserLog.Price;
                        }
                        var itm = _context.OrderItems.Where(x => x.OrderId == listGet.Id).ToList();
                        if (itm != null && itm.Any())
                        {
                            foreach (var item in itm)
                            {
                                var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == item.ProductVariantId);
                                var images = JsonConvert.DeserializeObject<List<int>>(shoeItem.ImageId);
                                var cartitem = new CartItems()
                                {
                                    ProductVariantId = item.ProductVariantId,
                                    Color = _context.Colors.FirstOrDefault(x => x.Id == shoeItem.Color).ColorName,
                                    Size = _context.Sizes.FirstOrDefault(x => x.Id == shoeItem.Size).Size1,
                                    Img = _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url,
                                    Price = item.Price,
                                    Quantity = item.Quantity,
                                    TongTien = item.Price * item.Quantity,
                                    VariantName = _context.ShoesVariants.FirstOrDefault(x => x.Id == item.ProductVariantId).VariantName,
                                };
                                listItem.Add(cartitem);

                            };
                            res.Items = listItem;
                            var shippingDetails = _context.ShippingDetails.FirstOrDefault(c => c.OrderId == listGet.Id);
                            if (shippingDetails != null)
                            {
                                var shippingDetailView = new ShippingDetailView();
                                shippingDetailView.ShippingAddress = shippingDetails.ShippingAddress;
                                shippingDetailView.ShippingPhone = shippingDetails.SenderPhone;
                                shippingDetailView.ShippingName = shippingDetails.ShippingName;
                                res.ShippingDetailView = shippingDetailView;
                            }

                        }

                    }

                    return Ok(res);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpPost]
        public IActionResult GetAll([FromBody] FilterOrder input)
        {
            try
            {
                var query = _context.Orders
                            .Join(_context.OrderItems, item => item.Id, orDt => orDt.OrderId, (item, orDt) => new { item, orDt })
                            .Join(_context.UserProfiles, joined => joined.item.UserId, kh => kh.Id, (joined, kh) => new { joined.item, joined.orDt, kh })
                            .Select(joinedResult => new
                            {
                                joinedResult.item,
                                joinedResult.orDt,
                                joinedResult.kh
                            });

                var qurye2 = from item in _context.Orders.ToList()
                             join orDt in _context.OrderItems.ToList() on item.Id equals orDt.OrderId
                             join kh in _context.UserProfiles.ToList() on item.UserId equals kh.Id
                             select new
                             {
                                 item,
                                 orDt,
                                 kh
                             };
                if (input.status != null)
                {
                    query = query.Where(x => x.item.Status == input.status);
                }
                if (input.StartDate != null && input.EndDate != null)

                {
                    query = query.Where(x => x.item.CreatedAt >= input.StartDate && x.item.CreatedAt <= input.EndDate);
                }
                var qrPost = query.Where(x => input.UserId == null || x.item.UserId == input.UserId)
                                   .Where(x => input.OrderCode == null || x.item.OrderCode.Contains(input.OrderCode))
                                   .Skip(input.SkipCount).Take(input.MaxResultCount);

                var items = qrPost.PageBy(input).OrderByDescending(x => x.item.CreatedAt).ToList();



                var res = items.Select(t =>
                 {
                     var output = new ResponOrderDTO();

                     if (t.kh != null)
                     {
                         output.TenKhachHang = t.kh.FullName;
                         output.IdKhachHang = t.kh.Id;
                     }
                     var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == t.item.Id);
                     if (voucherUserLog != null)
                     {
                         output.TongTien = t.item.OrderItems.Sum(x => x.Price * x.Quantity) - voucherUserLog.Price.Value;
                     }
                     else
                     {
                         output.TongTien = t.item.OrderItems.Sum(x => x.Price * x.Quantity);
                     }
                     output.Status = t.item.Status;
                     output.TongSanPham = _context.OrderItems.Where(x => x.OrderId == t.item.Id).Count();
                     output.CodeOrder = t.item.OrderCode;
                     output.CreatedTime = t.item.CreatedAt;
                     output.ModifiedTime = t.item.ModifiedAt;
                     output.Id = t.item.Id;
                     return output;
                 }).ToList().GroupBy(x => x.Id) // Nhóm các bản ghi theo trường Id
    .Select(grouped => grouped.FirstOrDefault());

                if (input.Sorting == "money")
                {
                    res = res.OrderByDescending(x => x.TongTien);
                }
                else if (input.Sorting == "money desc")
                {
                    res = res.OrderBy(x => x.TongTien);
                }
                else if (input.Sorting == "order")
                {
                    res = res.OrderByDescending(x => x.CreatedTime);
                }
                return Ok(res);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]

        public IActionResult UpdateTrangThai(int uId, int status, int? idBoss)
        {
            try
            {

                var listGet = _context.OrderItems.Where(c => c.OrderId == uId).ToList();
                var order = _context.Orders.FirstOrDefault(x => x.Id == uId);
                if (order != null)
                {
                    order.Status = status;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    var listReturn = new List<ReturnOrderItem>();
                    if (listGet != null && listGet.Any())
                    {
                        foreach (var item in listGet)
                        {
                            var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == item.ProductVariantId);
                            if (shoeItem != null)
                            {
                                if (status == 2)
                                {
                                    shoeItem.Stock = shoeItem.Stock + item.Quantity;
                                    _context.ShoesVariants.Update(shoeItem);
                                    _context.SaveChanges();
                                }
                                if (status == 1)
                                {
                                    //shoeItem.Stock = shoeItem.Stock - item.Quantity;
                                    //_context.ShoesVariants.Update(shoeItem);
                                    //_context.SaveChanges();
                                }
                                else if (status == 7)
                                {
                                    shoeItem.Stock = shoeItem.Stock + item.Quantity;
                                    _context.ShoesVariants.Update(shoeItem);
                                    _context.SaveChanges();
                                }
                                else if (status == 6)
                                {
                                    shoeItem.Stock = shoeItem.Stock + item.Quantity;
                                    _context.ShoesVariants.Update(shoeItem);
                                    _context.SaveChanges();
                                }
                                else if (status == 8)
                                {
                                    shoeItem.Stock = shoeItem.Stock + item.Quantity;
                                    _context.ShoesVariants.Update(shoeItem);
                                    _context.SaveChanges();
                                }

                                var images = JsonConvert.DeserializeObject<List<int>>(shoeItem.ImageId);
                                //var returnItem = new ReturnOrderItem(item.ProductVariantId ?? -1, item.Quantity ?? -1, item.Price ?? -1, _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url, _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName, _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1);
                                var returnItem = new ReturnOrderItem();
                                returnItem.quantity = item.Quantity ?? -1;
                                returnItem.price = item.Price ?? -1;
                                returnItem.variantId = item.ProductVariantId ?? -1;
                                returnItem.img = _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url;
                                returnItem.color = _context.Colors.FirstOrDefault(c => c.Id == shoeItem.Color).ColorName;
                                returnItem.size = _context.Sizes.FirstOrDefault(c => c.Id == shoeItem.Size).Size1;
                                listReturn.Add(returnItem);
                            }
                        }
                    }
                    var text = "";
                    var body = "";
                    if (status == 1)
                    {
                        text = "đã phê duyệt đơn hàng";
                        body = @"
                                <h3>Chào bạn đến với King Shoes</h1>
                                    <p>Đơn hàng của bạn đã được duyệt. Đơn hàng sẽ được sớm gửi tới tay bạn</p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.Id == order.UserId).Email, "Đơn hàng đã bị hủy", body);
                    }
                    else if (status == 2)
                    {
                        text = "đã từ chối đơn hàng";
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        body = @"
                                <h3>Chào bạn đến với King Shoes</h1>
                                    <p>Đơn hàng của bạn đã bị hủy bới người bán. Mọi hỗ trợ hay khiếu nại xin liên hệ vào SDT : 0123456789 </p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.Id == order.UserId).Email, "Đơn hàng đã bị hủy", body);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }
                    else if (status == 6)
                    {
                        text = "đã hủy đơn hàng";
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        body = @"
                                <h3>King Shoes</h1>
                                    <p>Khách hàng đã hủy đơn hàng vào lúc :{time} </p>
                                    ";
                        body = body.Replace("{time}", Convert.ToString(DateTimeOffset.Now));
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.RoleId == 2)?.Email, "Khách hàng đã hủy đơn hàng", body);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }
                    else if (status == 3)
                    {
                        text = "Đơn hàng đang giao";
                        body = @"
                                <h3>Chào bạn đến với King Shoes</h1>
                                    <p>Đơn hàng của bạn đang được giao </p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.Id == order.UserId).Email, "Đơn hàng đã bị hủy", body);
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }
                    else if (status == 4)
                    {
                        text = "Đơn hàng đang trên đường giao tới bạn";
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        body = @"
                                <h3>Chào bạn đến với King Shoes</h1>
                                    <p>Đơn hàng đang trên đường giao tới bạn </p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.Id == order.UserId).Email, "Đơn hàng đã bị hủy", body);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }
                    else if (status == 5)
                    {
                        text = "Đơn hàng đang trên đường giao tới bạn";
                        body = @"
                                <h3>Đơn hàng đang trên đường giao tới bạn</h1>
                                    <p>Đơn hàng đang trên đường giao tới bạn</p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.Id == order.UserId).Email, "Đơn hàng đang trên đường giao tới bạn", body);
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }
                    else if (status == 5)
                    {
                        text = "đã nhận hàng";
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        body = @"
                                <h3>Khách hàng đã nhận được hàng</h1>
                                    <p>Khách hàng đã nhận được hàng</p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.RoleId == 2)?.Email, "Khách hàng đã nhận được hàng", body);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }
                    else if (status == 7)
                    {
                        text = "từ chối nhận hàng";
                        body = @"
                                <h3>Khách hàng đã từ chối hàng</h1>
                                    <p>Khách hàng đã từ chối hàng</p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.RoleId == 2)?.Email, "Khách hàng đã từ chối hàng", body);
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }
                    else if (status == 8)
                    {
                        text = "Khách hàng không nhận hàng";
                        body = @"
                                <h3>Khách hàng không nhận hàng</h1>
                                    <p>Khách hàng không nhận hàng</p>
                                    ";
                        _mailServices.SendMail(_context.UserProfiles.FirstOrDefault(x => x.RoleId == 2)?.Email, "Khách hàng không nhận hàng", body);
                        var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == uId);
                        if (voucherUserLog != null)
                        {
                            _context.VouchersUseLogs.Remove(voucherUserLog);
                            _context.SaveChanges();
                        }
                    }

                    var x = new OrderHistoryLog()
                    {
                        IdKhachHang = order.UserId,
                        OrderId = uId,
                        LogTime = DateTime.Now,
                        Message = text
                    };
                    if (idBoss != null)
                    {
                        x.IdBoss = idBoss;
                    }
                    if (status == 3 || status == 4)
                    {
                        x.IdKhachHang = null;
                        x.IdBoss = null;
                    }
                    _context.OrderHistoryLogs.Add(x);
                    _context.SaveChanges();

                    return Ok(listReturn);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetUseLog(int orderId)
        {
            try
            {
                var res = _context.OrderHistoryLogs.Where(x => x.OrderId == orderId).ToList();

                if (res.Any())
                {
                    var item = res.Select(t =>
                    {
                        var output = new OrderLogResponse();

                        if (t.IdBoss != null)
                        {
                            output.TenBoss = _context.UserProfiles.FirstOrDefault(x => x.Id == t.IdBoss)?.FullName;
                        }
                        if (t.IdKhachHang != null)
                        {
                            output.TenKhachHang = _context.UserProfiles.FirstOrDefault(x => x.Id == t.IdKhachHang)?.FullName;
                        }
                        output.OrderId = orderId;
                        output.Message = t.Message;
                        output.LogTime = t.LogTime;
                        output.Id = t.Id;
                        return output;
                    });
                    return Ok(item);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateTrangThaiGiaoHang(int idshipping, int status, int? idBoss)
        {
            try
            {
                var shipping = _context.ShippingDetails.FirstOrDefault(x => x.Id == idshipping);
                if (shipping != null)
                {
                    if (status == 2)
                    {
                        shipping.Status = status;
                        _context.ShippingDetails.Update(shipping);
                        _context.SaveChanges();
                        var x = new OrderHistoryLog()
                        {
                            IdKhachHang = 3,
                            OrderId = (int)shipping.OrderId,
                            LogTime = DateTime.Now,
                            Message = "đã từ chối giao hàng",

                        };
                        if (idBoss != null)
                        {
                            x.IdBoss = idBoss;
                        }
                        return Ok(shipping);
                    }
                    else
                    {
                        shipping.Status = status;
                        _context.ShippingDetails.Update(shipping);
                        _context.SaveChanges();

                        var items = _context.OrderItems.Where(x => x.OrderId == shipping.OrderId).ToList();
                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                var shoe = _context.ShoesVariants.FirstOrDefault(x => x.Id == item.ProductVariantId);
                                if (shoe != null)
                                {
                                    shoe.Stock = Convert.ToInt32(shoe.Stock - item.Quantity);
                                    _context.ShoesVariants.Update(shoe);
                                    _context.SaveChanges();
                                }
                            }
                        }
                        var x = new OrderHistoryLog()
                        {
                            IdKhachHang = 3,
                            OrderId = (int)shipping.OrderId,
                            LogTime = DateTime.Now,
                            Message = "bắt đầu giao hàng",

                        };
                        return Ok(shipping);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult GetAllPhieuGiaoHang([FromBody] FilterPhieuGiaoHang input)
        {
            try
            {
                var query = _context.Orders
                            .Join(_context.OrderItems, item => item.Id, orDt => orDt.OrderId, (item, orDt) => new { item, orDt })
                            .Join(_context.UserProfiles, joined => joined.item.UserId, kh => kh.Id, (joined, kh) => new { joined.item, joined.orDt, kh })
                            .Select(joinedResult => new
                            {
                                joinedResult.item,
                                joinedResult.orDt,
                                joinedResult.kh
                            });

                var qurye2 = from a in _context.ShippingDetails.ToList()
                             join b in _context.Orders.ToList() on a.OrderId equals b.Id
                             join c in _context.OrderItems.ToList() on b.Id equals c.OrderId
                             join d in _context.UserProfiles.ToList() on a.UserId equals d.Id
                             select new
                             {
                                 a,
                                 b,
                                 c,
                                 d
                             };
                var qrPost = qurye2.Where(x => input.UserId == null || x.a.UserId == input.UserId).Where(x => x.b.Status == 1);

                var items = qrPost.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                var res = items.Select(t =>
                {
                    var output = new ResponsePhieuGiaoHang();

                    if (t.d != null)
                    {
                        output.TenKhachHang = t.d.FullName;
                        output.IdKhachHang = t.d.Id;
                    }
                    output.Status = t.a.Status;
                    output.ShippingPhone = t.a.ShippingPhone;
                    output.SenderAddress = t.a.SenderAddress;
                    output.GhiChu = t.a.OrderNote;
                    output.ShippingName = t.a.ShippingName;
                    output.CreatedAt = t.a.CreatedAt;
                    output.MaShip = t.a.SenderName;
                    output.Id = t.a.Id;
                    return output;
                }).ToList().GroupBy(x => x.Id) // Nhóm các bản ghi theo trường Id
    .Select(grouped => grouped.FirstOrDefault());
                return Ok(res);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        public IActionResult GetPhieuGiaHangById(int shippignId = -1)
        {
            if (shippignId == -1)
            {
                return BadRequest("Order ID doesn't exist");
            }
            else
            {
                try
                {
                    var listGet = _context.ShippingDetails.FirstOrDefault(c => shippignId == 0 || c.Id == shippignId);
                    var returnList = new List<ReturnOrder>();
                    var res = new ResponsePhieuGiaoHang();
                    var listItem = new List<CartItems>();
                    if (listGet != null)
                    {
                        res.Status = listGet.Status;
                        res.IdAdress = listGet.Id;
                        res.GhiChu = "";
                        res.TenKhachHang = _context.UserProfiles.FirstOrDefault(x => x.Id == listGet.UserId)?.FullName;
                        res.IdKhachHang = listGet.UserId;
                        res.ShippingPhone = listGet.ShippingPhone;
                        res.SenderAddress = listGet.SenderAddress;
                        res.GhiChu = listGet.OrderNote;
                        res.ShippingName = listGet.ShippingName;
                        res.CreatedAt = listGet.CreatedAt;
                        res.MaShip = listGet.SenderName;
                        res.Id = listGet.Id;
                        var itm = _context.OrderItems.Where(x => x.OrderId == listGet.OrderId).ToList();
                        if (itm != null && itm.Any())
                        {
                            foreach (var item in itm)
                            {
                                var shoeItem = _context.ShoesVariants.FirstOrDefault(s => s.Id == item.ProductVariantId);
                                var images = JsonConvert.DeserializeObject<List<int>>(shoeItem.ImageId);
                                var cartitem = new CartItems()
                                {
                                    ProductVariantId = item.ProductVariantId,
                                    Color = _context.Colors.FirstOrDefault(x => x.Id == shoeItem.Color).ColorName,
                                    Size = _context.Sizes.FirstOrDefault(x => x.Id == shoeItem.Size).Size1,
                                    Img = _context.Galleries.FirstOrDefault(c => c.Id == images.FirstOrDefault()).Url,
                                    Price = item.Price,
                                    Quantity = item.Quantity,
                                    TongTien = item.Price * item.Quantity
                                };
                                listItem.Add(cartitem);

                            };
                            res.Items = listItem;

                        }

                    }

                    return Ok(res);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpPost("InvoiceSlip")]
        public async Task<IActionResult> InvoiceSlip(int orderId)
        {
            var order = _context.Orders.Include(c => c.OrderItems).Include(c => c.User).FirstOrDefault(c => c.Id == orderId);
            if (order == null)
            {
                return BadRequest("Không có order này");
            }

            var tbody = string.Empty;
            var sum = 0.0M;
            var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == order.Id);
            var priceVoucherUserLog = 0.0M;

            foreach (var orderItem in order.OrderItems)
            {
                var shoeVariant = await _repositoryShoeVariant_BUS.Getone(orderItem.ProductVariantId.Value);
                sum += (orderItem.Price * orderItem.Quantity).Value;
                tbody += $"<tr>\r\n\t\t                <td style=\"\"padding: 8px;border: 1px solid #ccc;\"\">{shoeVariant.VariantName}</td>\r\n\t\t                <td style=\"\"padding: 8px;border: 1px solid #ccc;\"\">{orderItem.Quantity}</td>\r\n\t\t                <td style=\"\"padding: 8px;border: 1px solid #ccc;\"\">{orderItem.Price.Value.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))}</td>\r\n\t\t                <td style=\"\"padding: 8px;border: 1px solid #ccc;\"\">{((orderItem.Price) * orderItem.Quantity).Value.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))}</td>\r\n\t                  </tr>";
            }
            if (voucherUserLog != null)
            {
                priceVoucherUserLog = voucherUserLog.Price.Value;
                sum -= voucherUserLog.Price.Value;
            }
            var body = @$"
               <div style=""width: 600px;margin: 0 auto;padding: 20px;border: 1px solid #ccc;font-family: Arial, sans-serif;"">
                <div style=""text-align: center;margin-bottom: 20px;"">
                  <h2>Phiếu gửi hoá đơn</h2>
                </div>
                <div style=""margin-bottom: 20px;"">
                  <p><strong>Thông tin khách hàng:</strong></p>
                  <p>Họ tên: {order.User.FullName}</p>
                  <p>Email: {order.User.Email}</p>
                  <p>Số điện thoại: {order.User.PhoneNumber}</p>
  
                  <p><strong>Ngày gửi:</strong>{order.CreatedAt.Value.Date} </p>
  
                  <table style=""width: 100%;border-collapse: collapse;"">
	                <thead>
	                  <tr>
		                <th style=""padding: 8px;border: 1px solid #ccc;"">Sản phẩm</th>
		                <th style=""padding: 8px;border: 1px solid #ccc;"">Số lượng</th>
		                <th style=""padding: 8px;border: 1px solid #ccc;"">Đơn giá</th>
		                <th style=""padding: 8px;border: 1px solid #ccc;"">Thành tiền</th>
	                  </tr>
	                </thead>
	                <tbody>
	                  {tbody}
	                </tbody>
                  </table>
                </div>
                <div style=""text-align: right;"">
                  <p>Số tiền giảm giá: <strong>{priceVoucherUserLog.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))}</strong></p>
                </div>
                <div style=""text-align: right;"">
                  <p>Tổng cộng: <strong>{sum.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))}</strong></p>
                </div>
                </div>
            
            "
           ;
            _mailServices.SendMail(order.User.Email, "Phiếu Hoá đơn", body);
            return Ok("Gửi phiếu thành công");
        }
    }
}
