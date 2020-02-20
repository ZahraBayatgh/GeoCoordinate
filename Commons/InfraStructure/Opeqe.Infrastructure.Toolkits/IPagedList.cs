using System.Collections.Generic;

namespace Opeqe.Infrastructure.Toolkits
{
    public interface IPagedList
    {
        bool HasNextPage { get; }
        bool HasPreviousPage { get; }
        int PageIndex { get; }
        int PageSize { get; }
        long TotalCount { get; }
        long TotalPages { get; }
    }

    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }

}