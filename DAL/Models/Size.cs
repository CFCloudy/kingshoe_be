using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Size
    {
        public Size()
        {
            ShoesVariants = new HashSet<ShoesVariant>();
        }

        public int Id { get; set; }
        public string? Locale { get; set; }
        public string? Size1 { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<ShoesVariant> ShoesVariants { get; set; }
    }
}
