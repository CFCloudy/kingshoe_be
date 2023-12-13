using System;
using System.Collections.Generic;

namespace GUI.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? ProductVariantId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
