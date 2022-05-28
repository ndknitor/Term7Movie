using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction GetTransactionById(long id);
        int CreateTransaction(Transaction transaction);
        int UpdateTransaction(Transaction transaction);
    }
}
