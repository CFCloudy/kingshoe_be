using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Address
{
    public class BookAddressDTO
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? City { get; set; } // thành phố

        public string? Ward { get; set; } //phường xã

        public string? District { get; set; } // quận huyện

        public string? AddressDetail { get; set; }

        public bool Type { get; set; }

        public bool? IsDefault { get; set; }

        public string? Name { get; set; }
    }
}
