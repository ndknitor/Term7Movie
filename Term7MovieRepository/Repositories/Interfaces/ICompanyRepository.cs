using Term7MovieCore.Data.Dto;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<TheaterCompany> GetAllCompany();
        Task<CompanyDto> GetCompanyById(int id);

        Task<CompanyDto> GetCompanyByManagerId(long managerId);
        int CreateCompany(TheaterCompany company);
        int UpdateCompany(TheaterCompany company);
        int DeleteCompany(int id);
    }
}
