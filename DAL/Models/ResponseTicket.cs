using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ResponseTicket
    {
        public ResponseTicket()
        {
            ResponseTicketItems = new HashSet<ResponseTicketItem>();
        }

        public int Id { get; set; }
        public string? TicketMessage { get; set; }
        public int? TicketType { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual TicketType? TicketTypeNavigation { get; set; }
        public virtual ICollection<ResponseTicketItem> ResponseTicketItems { get; set; }
    }
}
