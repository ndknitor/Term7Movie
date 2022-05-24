using System.Transactions;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface ITransactionStatusRepository
    {
        IEnumerable<TransactionStatus> GetAllTransactionStatus();
    }
}
