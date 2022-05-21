using Term7MovieApi.Entities;
using Term7MovieApi.Repositories.Interfaces;

namespace Term7MovieApi.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
    }
}
