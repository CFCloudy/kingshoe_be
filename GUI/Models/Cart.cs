using System;
using System.Collections.Generic;

namespace GUI.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TotalItem { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
