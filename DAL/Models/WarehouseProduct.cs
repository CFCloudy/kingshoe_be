using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class WarehouseProduct
    {
        public WarehouseProduct()
        {
            RequestTicketItems = new HashSet<RequestTicketItem>();
            ResponseTicketItems = new HashSet<ResponseTicketItem>();
        }

        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int? StorageStock { get; set; }
        public decimal? ImportPrice { get; set; }
        public int? Manufacturer { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Manufacturer? ManufacturerNavigation { get; set; }
        public virtual ICollection<RequestTicketItem> RequestTicketItems { get; set; }
        public virtual ICollection<ResponseTicketItem> ResponseTicketItems { get; set; }
    }
}
