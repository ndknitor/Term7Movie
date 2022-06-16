using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<PagingList<UserDTO>> GetAllUserAsync(UserFilterRequest request);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User userUpdate);
        Task DeleteUserAsync(long id);

        Task<int> GetCompanyIdByManagerId(long managerId);
        Task<long> GetUserIdByUserLoginAsync(UserInfo userInfo);
        Task<User> GetUserByIdAsync(long id);

        Task<User> GetUserWithRoleByIdAsync(long id);

        Task<User> GetUserById(int id);
    }
}
