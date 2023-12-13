using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class VoucherBank
    {
        public VoucherBank()
        {
            VouchersUseLogs = new HashSet<VouchersUseLog>();
        }

        public int Id { get; set; }
        public int? VoucherId { get; set; }
        public int? UserId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? Status { get; set; }

        public virtual UserProfile? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<VouchersUseLog> VouchersUseLogs { get; set; }
    }
}
