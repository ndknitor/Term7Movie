using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITransactionHistoryService
    {
        Task<TransactionHistoryListResponse> GetTransactionListHistoryAynsc(ParentFilterRequest request, string role, long userId);
    }
}
