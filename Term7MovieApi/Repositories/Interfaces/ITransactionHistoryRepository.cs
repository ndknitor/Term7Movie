using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface ITransactionHistoryRepository
    {
        IEnumerable<TransactionHistory> GetAllTransactionHistoryByCustomerId(long customerId);
        IEnumerable<TransactionHistory> GetAllTransactionHistoryByCompanyId(int companyId);
        TransactionHistory GetTransactionHistoryById(long id);
        int CreateTransactionHistory(TransactionHistory transactionHistory);
    }
}
