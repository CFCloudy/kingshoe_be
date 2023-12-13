namespace DTO.Paging
{
    public interface IPagedResultRequest: ILimitedResultRequest
    {
        int SkipCount { get; set; }
    }
}
