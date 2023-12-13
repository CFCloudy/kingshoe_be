using DAL.Models;
using DTO.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CustomerResponse
    {
        public int Id { get; set; }

        public string? FullName { get; set; }
        public string? UserId { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int? NumberOfOder { get; set; }
        public decimal? Money { get; set; }

        public string? LastOrder { get; set; }
        public int? OrderId { get; set; }

        public bool IsLock{get;set; }

        public DateTime? CreatedAt { get;set; }
        public DateTime? ModifiedAt { get; set; }

        public List<Order>? _lstOder { get;set; }
    }

    public class CustomerDetail
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int? NumberOfOder { get; set; }
        public decimal? Money { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public List<ReturnOrder>? ListOrder { get; set; }
    }

}
