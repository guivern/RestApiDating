using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RestApiDating.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }
        /// <summary>
        /// Crea y retorna una lista paginada de un IQueryable DbSet.
        /// </summary>
        /// <param name="source">IQueryable DbSet</param>
        /// <param name="pageNumber">Nro. de pagina</param>
        /// <param name="pageSize">Tamanio de pagina</param>
        /// <returns></returns>
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var totalCountToPreviousPage = (pageNumber - 1) * pageSize;
             
            var items = await source.Skip(totalCountToPreviousPage)
                .Take(pageSize)
                .ToListAsync();
                
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}