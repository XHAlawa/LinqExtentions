using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions.Models
{
    internal class CacheModel<T>
    {
        public DateTime WillExpiredOn { get; set; }
        public T? Data { get; set; }
    }
    internal class ListCacheModel<T>
    {
        public DateTime WillExpiredOn { get; set; }
        public List<T>? Data { get; set; }
    }
}
