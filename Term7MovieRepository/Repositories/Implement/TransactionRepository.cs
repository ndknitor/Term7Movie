using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TransactionRepository : ITransactionRepository
    {
        private AppDbContext _context;
        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }
        public Transaction GetTransactionById(long id)
        {
            Transaction transaction = null;
            return transaction;
        }
        public void CreateTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }
        public async Task UpdateTransaction(Guid id, int statusId, int momoStatus)
        {
            Transaction dbTransaction = await _context.Transactions.FindAsync(id);

            if (dbTransaction == null) throw new DbNotFoundException();

            dbTransaction.StatusId = statusId;
            dbTransaction.MomoResultCode = momoStatus;
        }
    }
}
