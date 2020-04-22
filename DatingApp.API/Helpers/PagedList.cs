using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            // users count / pagesize
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
           int pageNumber, int pageSize)
        {
             // how many items are there
            var count = await source.CountAsync();
            //  Skip member bypasses a certain number of elements
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            //  returns the list of users
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}