using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Collections
{
    public class PagingList<T> : List<T>
    {
        public const int DefaultPageSize = 10;
        public const int DefaultPage = 1;
        public int PageSize { set; get; } = DefaultPageSize;
        public int Page { set; get; } = DefaultPage;

        public PagingList() : base() { }
        public PagingList(int pageSize, int page)
        {
            PageSize = pageSize;
            Page = page;
        }
    }
}
