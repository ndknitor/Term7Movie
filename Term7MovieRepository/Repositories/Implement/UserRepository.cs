using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
namespace Term7MovieRepository.Repositories.Implement
{
    public class UserRepository : IUserRepository
    {

        private AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public int CreateUser(User user)
        {
            int count = 0;
            return count;
        }
        public int UpdateUser(User user)
        {
            int count = 0;
            return count;
        }
        public int DeleteUser(long id)
        {
            int count = 0;
            return count;
        }
    }
}
