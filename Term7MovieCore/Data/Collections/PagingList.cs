using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Collections
{
    public class PagingList<T> 
    { 
        public int PageSize { set; get; } = Constants.DefaultPageSize;
        public int Page { set; get; } = Constants.DefaultPage;

        public IEnumerable<T> Results { set; get; } = new List<T>();
        public long Total { set; get; } = 0;
        public long MaxPage { get; } = 0;
        public PagingList() { }
        public PagingList(int pageSize, int page, IEnumerable<T> results, long total)
        {
            PageSize = pageSize;
            Page = page;
            Total = total;
            Results = results;
            MaxPage = (long) (Math.Ceiling((double)total / pageSize));
        }
    }
}
