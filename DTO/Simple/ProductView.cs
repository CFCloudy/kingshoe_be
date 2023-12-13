using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Simple
{
    public class ProductView
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
    }

    public class VariantView
    {
        public int ProductID { get; set; }
        public int ProductVariantID { get; set; }
        public string? ProductName { get; set; }
    }
}
