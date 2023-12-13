
using System;
namespace DTO.Paging
{

    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    [Serializable]
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        public virtual string Sorting { get; set; }
    }
}
