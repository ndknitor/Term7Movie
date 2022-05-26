using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IUserRepository
    {
        int CreateUser(User user);
        int UpdateUser(User user);
        int DeleteUser(long id);
    }
}
