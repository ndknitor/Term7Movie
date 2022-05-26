
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class LanguageRepository : ILanguageRepository
    {
        private AppDbContext _context;
        public LanguageRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Language> GetAllLanguage()
        {
            IEnumerable<Language> list = new List<Language>();
            return list;
        }
    }
}
