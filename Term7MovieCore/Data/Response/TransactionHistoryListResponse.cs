using Term7MovieCore.Entities;

namespace Term7MovieCore.Data.Response
{
    public class TransactionHistoryListResponse : ParentResponse
    {
        public IEnumerable<TransactionHistory> History { set; get; }
    }
}
