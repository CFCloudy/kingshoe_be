using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateColorDTO
    {
        public string? ColorName { get; set; }
        public string? Rgba { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
