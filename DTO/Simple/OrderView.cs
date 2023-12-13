using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Simple
{
    public class OrderView
    {
        public int Id { get; set; } = 0;
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; } = 0;
        public string? OrderCode { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = new DateTime();
        public List<OrderItemView> ListItems { get; set; } = new List<OrderItemView>();
        public ShippingDetailView? ShippingDetails { get; set; } = new ShippingDetailView();
    }

    public class OrderItemView
    {
        public int VariantID { get; set; }
        public string? VariantName { get; set; }
        //public string? ProductName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }

    }

    public class ShippingDetailView
    {
        public int OrderId { get; set; } = 0;
        public int? ShippingId { get; set; }
        public string? ShippingName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingPhone { get; set; }
        public string? OrderNote { get; set; }
        public int Status { get; set; } = 0;
    }

    public class ReturnOrder
    {
        public List<ReturnOrderItem>? items { get; set; } = new List<ReturnOrderItem>();
        public int orderId { get; set; }
        public string? maDonHang { get; set; }
        public int cartId { get; set; }
        public decimal? total { get; set; }
        public DateTime createDate { get; set; }
        public int? Status {get;set; }
    }
    public class ReturnOrderItem
    {

        public int variantId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string img { get; set; } = "";
        public string color { get; set; } = "";
        public string size { get; set; } = "";
        public string? variantName { get; set; } = "";
        public decimal? sum { get; set; }
    }
}
