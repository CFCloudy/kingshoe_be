using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class Available_color
    {
        public Available_color()
        {

        }
        public int IdColor { get; set; }
        public string Name { get; set; }
        public List<ImageVariant> ImageVariants { get; set; } = new List<ImageVariant>();
        public string? Rgba { get; set; }

    }
}
