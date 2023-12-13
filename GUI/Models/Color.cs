using System;
using System.Collections.Generic;

namespace GUI.Models
{
    public partial class Color
    {
        public Color()
        {
            ShoesVariants = new HashSet<ShoesVariant>();
        }

        public int Id { get; set; }
        public string? ColorName { get; set; }
        public int? DisplayOrder { get; set; }
        public string? Rgba { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<ShoesVariant> ShoesVariants { get; set; }
    }
}
