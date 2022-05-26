using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
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
