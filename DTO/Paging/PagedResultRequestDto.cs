using System.ComponentModel.DataAnnotations;

namespace DTO.Paging
{
    public class PagedResultRequestDto: LimitedResultRequestDto, IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }
}
