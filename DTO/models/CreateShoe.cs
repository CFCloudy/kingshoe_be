using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateShoe
    {
        public string ProductName { get; set; } = "";
        public decimal DisplayPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int? BrandId { get; set; }
        public int? FeatureId { get; set; }
        public int? StyleId { get; set; }
        public int? ImageId { get; set; }
        public bool? IsHotProduct { get; set; }
        public string? Description { get; set; }
        public string? DescriptionDetail { get; set; }
        public List<UpdateImageVariant> UpdateImageVariant { get; set; } = new List<UpdateImageVariant>();
    }
}
