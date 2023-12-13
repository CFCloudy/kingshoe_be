using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class ColorDTO
    {
        public ColorDTO()
        {

        }
        public int Id { get; set; }
        public string? ColorName { get; set; }
        public string? Rgba { get; set; }
        public string Type { get; set; }
    }
}
