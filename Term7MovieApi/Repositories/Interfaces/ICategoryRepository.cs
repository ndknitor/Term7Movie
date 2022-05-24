using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategory();

    }
}
