using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TransactionStatusRepository : ITransactionStatusRepository
    {
        private AppDbContext _context;
        public TransactionStatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TransactionStatus> GetAllTransactionStatus()
        {
            IEnumerable<TransactionStatus> list = new List<TransactionStatus>();
            return list;
        }
    }
}
