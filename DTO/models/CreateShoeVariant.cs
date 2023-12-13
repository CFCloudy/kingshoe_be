using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateShoeVariant
    {
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public int? Stock { get; set; }
        public int? Size { get; set; }
        public int? Color { get; set; }
        public string? ImageId { get; set; }
    }
}
