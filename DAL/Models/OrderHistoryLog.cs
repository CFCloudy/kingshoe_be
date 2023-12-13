using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class OrderHistoryLog
    {
        public int Id { get; set; } 
        public int OrderId { get; set; }

        public int? IdKhachHang { get; set; }

        public int? IdBoss { get; set; }

        public string ?Message { get; set; }
        public DateTime LogTime { get; set; }

        public Order Order { get; set; }
    }
}
