using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TicketType
    {
        public TicketType()
        {
            RequestTickets = new HashSet<RequestTicket>();
            ResponseTickets = new HashSet<ResponseTicket>();
        }

        public int Id { get; set; }
        public string? TypeName { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<RequestTicket> RequestTickets { get; set; }
        public virtual ICollection<ResponseTicket> ResponseTickets { get; set; }
    }
}
