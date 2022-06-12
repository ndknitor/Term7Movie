using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction GetTransactionById(long id);
        Task CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Guid id, int statusId, int momoStatus);
    }
}
