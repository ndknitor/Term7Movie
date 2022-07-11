
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllRoleAsync();
    }
}
