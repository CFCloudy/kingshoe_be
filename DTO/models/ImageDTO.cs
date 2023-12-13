using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class ImageDTO
    {
        public ImageDTO()
        {

        }
        public string Name { get; set; }
        public string Base64 { get; set; }
        public string Type { get; set; }
    }
}
