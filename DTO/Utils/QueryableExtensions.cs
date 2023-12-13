using DTO.Paging;

namespace DTO.Utils
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return query.Skip(skipCount).Take(maxResultCount);
        }
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
        {
            return query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
        }
    }
}
