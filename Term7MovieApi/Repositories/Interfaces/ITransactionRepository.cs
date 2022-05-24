using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction GetTransactionById(long id);
        int CreateTransaction(Transaction transaction);
        int UpdateTransaction(Transaction transaction);
    }
}
