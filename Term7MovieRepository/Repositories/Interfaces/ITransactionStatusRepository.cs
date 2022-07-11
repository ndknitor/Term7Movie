

using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITransactionStatusRepository
    {
        IEnumerable<TransactionStatus> GetAllTransactionStatus();
    }
}
