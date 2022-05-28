using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieCore.Entities;
namespace Term7MovieRepository.Repositories.Implement
{
    public class CompanyRepository : ICompanyRepository
    {
        private AppDbContext _context;
        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<TheaterCompany> GetAllCompany()
        {
            IEnumerable<TheaterCompany> list = new List<TheaterCompany>();
            return list;
        }
        public TheaterCompany GetCompanyById(int id)
        {
            TheaterCompany company = null;
            return company;
        }
        public int CreateCompany(TheaterCompany company)
        {
            int count = 0;
            return count;
        }
        public int UpdateCompany(TheaterCompany company)
        {
            int count = 0;
            return count;
        }
        public int DeleteCompany(int id)
        {
            int count = 0;
            return count;
        }
    }
}
