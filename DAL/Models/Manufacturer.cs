using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            WarehouseProducts = new HashSet<WarehouseProduct>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<WarehouseProduct> WarehouseProducts { get; set; }
    }
}
