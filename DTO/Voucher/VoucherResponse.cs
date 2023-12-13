using DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Voucher
{
    public class VoucherResponse
    {
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? MaxValue { get; set; }
        public decimal? MinValue { get; set; }
        public string NameVoucher { get; set; }
        public int? Quantity { get; set; }
        public int? Status { get; set; }
        public bool Unit { get; set; }
        public decimal? Value { get; set; }
        public int VoucherId { get; set; }
    }

    public class CreateVoucher
    {
        public string? VoucherContent { get; set; }
        public int? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountMaxValue { get; set; }
        public string? VoucherCode { get; set; }
        public int? Status { get; set; }
        public int? StatusVoucher { get; set; }
        public int? UsePerPerson { get; set; }
        public int? UseLimit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RequriedValue { get; set; }
    }

    public class UpdateVoucher
    {
        public string? VoucherContent { get; set; }
        public int? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountMaxValue { get; set; }
        public string? VoucherCode { get; set; }
        public int? Status { get; set; }

        public int Id { get; set; }
        public int? StatusVoucher { get; set; }
        public int? UsePerPerson { get; set; }
        public int? UseLimit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RequriedValue { get; set; }
    }

    public class VoucherDetails
    {
        public string? VoucherContent { get; set; }
        public int? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountMaxValue { get; set; }
        public string? VoucherCode { get; set; }
        public int? Status { get; set; }
        public int? StatusVoucher { get; set; }
        public int? UsePerPerson { get; set; }
        public int? UseLimit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RequriedValue { get; set; }
        public int Id { get; set; }
    }

    public class FilterVoucher{

        public int? status { get; set; }
        public string? NameVoucher { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
