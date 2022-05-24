using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        int CreateUser(User user);
        int UpdateUser(User user);
        int DeleteUser(long id);
    }
}
