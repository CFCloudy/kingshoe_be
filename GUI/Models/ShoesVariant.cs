using System;
using System.Collections.Generic;

namespace GUI.Models
{
    public partial class ShoesVariant
    {
        public ShoesVariant()
        {
            //CartItems = new HashSet<CartItem>();
            //OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string? VariantName { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal DisplayPrice { get; set; }
        public int? Stock { get; set; }
        public int? Sku { get; set; }
        public int? Size { get; set; }
        public int? Color { get; set; }
        public string? ImageId { get; set; }
        public string? ImageURL { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Color? ColorNavigation { get; set; }
        public virtual Shoe? Product { get; set; }
        public virtual Size? SizeNavigation { get; set; }
        //public virtual ICollection<CartItem> CartItems { get; set; }
        //public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
