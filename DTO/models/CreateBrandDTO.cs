using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateBrandDTO
    {
        public string BrandName { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
    }
}
