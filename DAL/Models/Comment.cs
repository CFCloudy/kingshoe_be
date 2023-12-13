using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? ParentId { get; set; }
        public ICollection<Gallery>? Attachment { get; set; }
    }
}
