using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
        }

        public int Id { get; set; }
        public string? VoucherContent { get; set; }
        public int? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountMaxValue { get; set; }
        public string? VoucherCode { get; set; }
        public int? Status { get; set; }
        public int? StatusVoucher { get; set; }
        public int? UsePerPerson { get; set; }
        public int? UseLimit { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RequriedValue { get; set; }
    }
}
