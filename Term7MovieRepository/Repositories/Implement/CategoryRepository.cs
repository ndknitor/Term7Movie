using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            if (!await _context.Database.CanConnectAsync())
                return null;
            List<Category> categories = new List<Category>();
            categories = await _context.Categories.ToListAsync();
            return categories;
        }
    }
}
