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
        public async Task CreateTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }
        public async Task UpdateTransaction(Transaction transaction)
        {
            Transaction dbTransaction = await _context.Transactions.FindAsync(transaction.Id);

            if (dbTransaction == null) throw new DbNotFoundException();

            dbTransaction.StatusId = transaction.StatusId;
            dbTransaction.MomoResultCode = transaction.MomoResultCode;
        }
    }
}
