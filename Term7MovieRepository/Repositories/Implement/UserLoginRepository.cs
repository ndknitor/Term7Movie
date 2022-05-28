using Term7MovieCore.Data.Dto;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly AppDbContext _context;
        public UserLoginRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateLogin(UserInfo userInfo, User user)
        {
            UserLogin login = new UserLogin()
            {
                UserId = user.Id,
                LoginProvider = userInfo.ProviderId,
                ProviderDisplayName = userInfo.ProviderId,
                ProviderKey = userInfo.Uid
            };
            await _context.UserLogins.AddAsync(login);
        }
    }
}
