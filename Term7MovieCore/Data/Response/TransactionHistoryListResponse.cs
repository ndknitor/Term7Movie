using Term7MovieCore.Data.Collections;
using Term7MovieCore.Entities;

namespace Term7MovieCore.Data.Response
{
    public class TransactionHistoryListResponse : ParentResponse
    {
        public PagingList<TransactionHistory> History { set; get; }
    }
}
