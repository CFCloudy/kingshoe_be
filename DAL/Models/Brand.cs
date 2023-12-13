using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Shoes = new HashSet<Shoe>();
        }

        public int Id { get; set; }
        public string BrandName { get; set; } = null!;
        public int? DisplayOrder { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<Shoe> Shoes { get; set; }
    }
}
