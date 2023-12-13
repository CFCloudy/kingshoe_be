using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateVouchersUseLog
    {
        public int? VoucherUserId { get; set; }
        public int? OrderId { get; set; }
        public decimal? Price { get; set; }
    }
}
