using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ResponseTicketItem
    {
        public int Id { get; set; }
        public int? TicketId { get; set; }
        public int WarehouseProductId { get; set; }
        public int? Quantity { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ResponseTicket? Ticket { get; set; }
        public virtual WarehouseProduct WarehouseProduct { get; set; } = null!;
    }
}
