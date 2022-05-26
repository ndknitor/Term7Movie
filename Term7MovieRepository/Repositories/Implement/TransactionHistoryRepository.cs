using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private AppDbContext _context;
        public TransactionHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TransactionHistory> GetAllTransactionHistoryByCustomerId(long customerId)
        {
            IEnumerable<TransactionHistory> list = new List<TransactionHistory>();
            return list;
        }
        public IEnumerable<TransactionHistory> GetAllTransactionHistoryByCompanyId(int companyId)
        {
            IEnumerable<TransactionHistory> list = new List<TransactionHistory>();
            return list;
        }
        public TransactionHistory GetTransactionHistoryById(long id)
        {
            TransactionHistory history = null;
            return history;
        }
        public int CreateTransactionHistory(TransactionHistory transactionHistory)
        {
            int count = 0;
            return count;
        }
    }
}
