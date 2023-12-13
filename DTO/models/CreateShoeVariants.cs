using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateShoeVariants
    {
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public List<SizeCreateShoe> Sizes { get; set; } = new List<SizeCreateShoe>();
        public List<ColorCreateShoe> Colors { get; set; } = new List<ColorCreateShoe>();
        public string? Description { get; set; }
    }
    public class ColorCreateShoe
    {
        public int? ColorId { get; set; }
        public int? Quantity { get; set; }
        public string? ImageId { get; set; }
    }
    public class SizeCreateShoe
    {
        public int? SizeId { get; set; }
    }
}
