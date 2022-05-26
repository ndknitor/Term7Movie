using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<TheaterCompany> GetAllCompany();
        TheaterCompany GetCompanyById(int id);
        int CreateCompany(TheaterCompany company);
        int UpdateCompany(TheaterCompany company);
        int DeleteCompany(int id);
    }
}
