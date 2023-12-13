using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int? ProductVariantId { get; set; }
        public string? VariantName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
