using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Gallery
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public bool? IsThumbnail { get; set; }
        public string? ContentLength { get; set; }
        public string? Mime { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? CommentId { get; set; }
        public Comment? Comments { get; set; }
    }
}
