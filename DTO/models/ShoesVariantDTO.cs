using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class ShoesVariantDTO
    {
        public ShoesVariantDTO()
        {

        }
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string? VariantName { get; set; }
        public string Size { get; set; }
        public int SizeId { get; set; }
        public string Color { get; set; }
        public int ColorId { get; set; }
        public string? Rgba { get; set; }
        public string? Description { get; set; }
        public int? Quantity  { get; set; }
        public decimal Price { get; set; }
        public int? Status { get; set; }
    }
}
