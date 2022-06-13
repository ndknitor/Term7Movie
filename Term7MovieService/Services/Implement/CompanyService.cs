using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyRepository companyRepo;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            companyRepo = _unitOfWork.CompanyRepository;
        }

        public async Task<CompanyDetailResponse> GetCompanyDetailAsync(int? companyId, long? managerId = null)
        {
            CompanyDto company = null;

            if (managerId == null && companyId == null) throw new DbNotFoundException();

            if (managerId != null)
            {
                company = await companyRepo.GetCompanyByManagerId(managerId.Value);
            } else
            {
                company = companyId != null ? await companyRepo.GetCompanyById(companyId.Value) : null;
            }

            if (company == null) throw new DbNotFoundException();

            return new CompanyDetailResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Company = company
            };
        }

        public async Task<CompanyListResponse> GetAllCompanyAsync(ParentFilterRequest request)
        {
            IEnumerable<CompanyDto> list = await companyRepo.GetAllCompany(request);

            return new CompanyListResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Companies = list
            };
        }
    }
}
