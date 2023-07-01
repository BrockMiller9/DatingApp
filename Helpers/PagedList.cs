
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            // CountAsync() and ToListAsync() are extension methods from Microsoft.EntityFrameworkCore that allow us to execute the query asynchronously
            // We use the CountAsync() method to get the total number of items in the sequence and ToListAsync() to get the items for the specified page
            // We use the Skip() method to skip over the items that we don't want and then Take() to take the number of items that we want
            // We then pass the items, count, pageNumber and pageSize to the constructor of the PagedList class
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}