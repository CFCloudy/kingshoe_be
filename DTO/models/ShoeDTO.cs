using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class ShoeDTO
    {
        public ShoeDTO()
        {

        }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal DisplayPrice { get; set; }
        public decimal? OldPrice { get; set; }
        public int Quantity { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string FeatureName { get; set; }
        public int FeatureId { get; set; }
        public string StyleName { get; set; }
        public string DisplayImage { get; set; }
        public int IdImage { get; set; }
        public bool? IsHotProduct { get; set; }
        public string? Description { get; set; }
        public string? DescriptionDetail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public int StyleId { get; set; }
        public List<string> Available_sizes { get; set; } = new List<string>();
        public List<Available_color> Available_colors { get; set; } = new List<Available_color> { };
        public List<ShoesVariantDTO> shoesVariantDTOs { get; set; } = new List<ShoesVariantDTO>();
    }
}
