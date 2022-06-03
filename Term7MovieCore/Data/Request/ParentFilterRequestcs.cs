using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class ParentFilterRequest
    {
        private int pageSize;
        private int page;
        public int PageSize 
        { 
            set => pageSize = value > 0 ? value : Constants.DefaultPageSize;
            get => pageSize; 
        }
        public int Page
        { 
            set => page = value > 0 ? value : Constants.DefaultPage; 
            get => page; 
        }

        public string? SearchKey { set; get; }
    }
}
