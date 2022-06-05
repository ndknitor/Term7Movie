using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieCore.Entities;
using Term7MovieCore.Data.Options;
using Microsoft.Data.SqlClient;

namespace Term7MovieRepository.Repositories.Implement
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public CompanyRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
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
