using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITransactionHistoryRepository
    {
        IEnumerable<TransactionHistory> GetAllTransactionHistoryByCustomerId(long customerId);
        IEnumerable<TransactionHistory> GetAllTransactionHistoryByCompanyId(int companyId);
        TransactionHistory GetTransactionHistoryById(long id);
        int CreateTransactionHistory(TransactionHistory transactionHistory);
    }
}
