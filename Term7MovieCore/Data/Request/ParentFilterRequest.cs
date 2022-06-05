namespace Term7MovieCore.Data.Request
{
    public class ParentFilterRequest
    {
        private int pageSize;
        private int page;
        public int PageSize 
        { 
            set => pageSize = value;
            get => pageSize = pageSize > 0 ? pageSize : Constants.DefaultPageSize; 
        }
        public int Page
        { 
            set => page = value; 
            get => page = page > 0 ? page : Constants.DefaultPage; 
        }

        public string SearchKey { set; get; }
    }
}
