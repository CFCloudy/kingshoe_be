using System;
using System.Collections.Generic;

namespace GUI.Models
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
        public int? Quantity { get; set; } // Số lượng voucher của mỗi khách hàng (=UsePerPerson)
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? Status { get; set; }
        public string VoucherName { get; set; }
        public string UserName { get; set; }

        public virtual UserProfile? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<VouchersUseLog> VouchersUseLogs { get; set; }
    }
}
