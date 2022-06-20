using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ICompanyService
    {
        Task<CompanyDetailResponse> GetCompanyDetailAsync(int? companyId, long? managerId = null);
        Task<object> GetAllCompanyAsync(CompanyFilterRequest request);

        Task<ParentResponse> UpdateCompanyAsync(CompanyUpdateRequest request);
    }
}
