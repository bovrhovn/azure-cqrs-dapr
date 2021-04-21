using System;
using System.Collections.Generic;
using System.Linq;

namespace CityApp.Engine
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalItemCount { get; }

        public PaginatedList() : this(new List<T>(), 0, 1, 0)
        {
        }

        public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            AddRange(items);
            TotalItemCount = count;
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => (PageIndex < TotalPages);

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}