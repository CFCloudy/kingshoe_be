using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Shoe
    {
        public Shoe()
        {
            ShoesVariants = new HashSet<ShoesVariant>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? OldPrice { get; set; }
        public decimal DisplayPrice { get; set; }
        public int? BrandGroup { get; set; }
        public int? Feature { get; set; }
        public int? StyleGroup { get; set; }
        public int? DisplayImage { get; set; }
        public bool? IsHotProduct { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public string? DescriptionDetail { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Brand? BrandGroupNavigation { get; set; }
        public virtual Feature? FeatureNavigation { get; set; }
        public virtual Style? StyleGroupNavigation { get; set; }
        public virtual ICollection<ShoesVariant> ShoesVariants { get; set; }
    }
}
