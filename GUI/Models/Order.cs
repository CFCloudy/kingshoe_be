using System;
using System.Collections.Generic;

namespace GUI.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            ShippingDetails = new HashSet<ShippingDetail>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? Detail { get; set; }
        public string? OrderCode { get; set; }
        public decimal? Total { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ShippingDetail> ShippingDetails { get; set; }
    }
}
