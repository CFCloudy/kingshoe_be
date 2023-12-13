using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public partial class ShippingDetail
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? OrderId { get; set; }
        public string? ShippingName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingPhone { get; set; }
        public string? SenderName { get; set; }
        public string? SenderAddress { get; set; }
        public string? SenderPhone { get; set; }
        public string? OrderNote { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Order? Order { get; set; }
    }
}
