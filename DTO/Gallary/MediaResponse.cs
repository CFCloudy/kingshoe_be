using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Gallary
{
    public class MediaResponse
    {
        public string? ContentLength { get; set; }

        public string? Mime { get; set; }

        public int? Id { get; set; }
        public string? Url { get; set; }

        public string? Status { get; set; }
        public bool Error { get; set; }
    }
}
