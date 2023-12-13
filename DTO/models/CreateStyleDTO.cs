using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateStyleDTO
    {
        public string StyleName { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
    }
}
