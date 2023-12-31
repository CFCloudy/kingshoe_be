﻿using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class VouchersUseLog
    {
        public int Id { get; set; }
        public int? VoucherUserId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? Status { get; set; }
        public decimal? Price { get; set; }
        public virtual Order? Order { get; set; }
    }
}
