using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Utils
{
    public class Response<T>
    {
        public string? Status { get; set; }
        public string? Message { get; set; }

        public T? Payload { get; set; }

        public int? TotalCount { get; set; }
    }
}
