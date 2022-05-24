using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
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
