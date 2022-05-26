using Term7MovieApi.Entities;
using Term7MovieApi.Repositories.Interfaces;

namespace Term7MovieApi.Repositories.Implement
{
    public class CategoryRepository : ICategoryRepository
    {
        private AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

    public IEnumerable<Category> GetAllCategory()
        {

            IEnumerable<Category> categories = new List<Category>();
            return categories;
        }
    }
}
