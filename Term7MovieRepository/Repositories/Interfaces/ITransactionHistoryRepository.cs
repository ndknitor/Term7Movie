using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITransactionHistoryRepository
    {
        Task<IEnumerable<TransactionHistory>> GetAllTransactionHistory(ParentFilterRequest request);
        Task<IEnumerable<TransactionHistory>> GetAllTransactionHistoryByCustomerId(ParentFilterRequest request, long customerId);
        Task<IEnumerable<TransactionHistory>> GetAllTransactionHistoryByCompanyId(ParentFilterRequest request, long managerId);
        TransactionHistory GetTransactionHistoryById(long id);
        Task CreateTransactionHistory(IEnumerable<long> idList);
    }
}
