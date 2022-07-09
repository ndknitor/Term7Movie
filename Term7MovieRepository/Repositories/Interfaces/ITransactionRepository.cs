using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        void CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Guid id, int statusId, int momoStatus);
        Task<PagingList<TransactionDto>> GetAllTransactionAsync(TransactionFilterRequest request, long userId, string role);
        Task<TransactionDto> GetTransactionByIdAsync(Guid transactionId);
        void CancelAllExpiredTransaction();
    }
}
