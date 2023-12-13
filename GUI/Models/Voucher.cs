using System;
using System.Collections.Generic;

namespace GUI.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            VoucherBanks = new HashSet<VoucherBank>();
        }

        public int Id { get; set; }
        public string? VoucherContent { get; set; } // tên vc
        public int? DiscountType { get; set; } // Loại đơn vị giảm (1: % 2: tiền)
        public decimal? DiscountValue { get; set; } // Giá trị voucher
        public decimal? DiscountMaxValue { get; set; } // Giá trị tối đa voucher ( nêu sgiamr thẳng -> DiscountValue = DiscountMaxValue)
        public string? VoucherCode { get; set; } // MÃ voucher áp dụng
        public int? Status { get; set; } 
        public int? StatusVoucher { get; set; } // đang hoạt dộng & không hoạt động
        public int? UsePerPerson { get; set; } // Số lần dùng của mỗi khách hàng
        public int? UseLimit { get; set; } // Số lần dùng trong chương trình 
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? RequriedValue { get; set; } // Giá trị tối thiểu của đơn hàng

        public virtual ICollection<VoucherBank> VoucherBanks { get; set; }
    }
}
