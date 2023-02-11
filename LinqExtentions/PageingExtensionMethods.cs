using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtentions
{
    public static class PageingExtensionMethods
    {
        public static (List<T> Items,int Total) ToPagedList<T>(this IQueryable<T> query, int page = 0, int countPerPage = 10)
        {
            var pagedItems = query.Skip(page * countPerPage).Take(countPerPage).ToList();
            return (pagedItems, query.Count());
        } 
    }
}
