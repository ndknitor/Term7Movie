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
        public int CreateTransaction(Transaction transaction)
        {
            int count = 0;
            return count;
        }
        public int UpdateTransaction(Transaction transaction)
        {
            int count = 0;
            return count;
        }
    }
}
