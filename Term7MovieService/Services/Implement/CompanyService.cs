using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
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

        public async Task<object> GetAllCompanyAsync(CompanyFilterRequest request)
        {
            if (request.WithNoManager)
            {
                return await GetAllCompanyNoPagingAsync();
            }

            return await GetAllCompanyPagingAsync(request);
        }

        private async Task<CompanyListResponse> GetAllCompanyPagingAsync(CompanyFilterRequest request)
        {
            PagingList<CompanyDto> list = await companyRepo.GetAllCompany(request);

            return new CompanyListResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Companies = list
            };
        }

        private async Task<ParentResultResponse> GetAllCompanyNoPagingAsync()
        {
            IEnumerable<CompanyDto> list = await companyRepo.GetAllCompanyNoPaging();

            return new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = list.Where(c => c.ManagerId == null)
            };
        }

        public async Task<ParentResponse> UpdateCompanyAsync(CompanyUpdateRequest request)
        {
            int count = await companyRepo.UpdateCompany(request);

            if (count == 0) throw new BadRequestException();

            return new ParentResponse
            {
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<ParentResponse> CreateCompanyAsync(CompanyCreateRequest request)
        {
            int count = await companyRepo.CreateCompany(request);

            if (count == 0) throw new BadRequestException();

            return new ParentResponse
            {
                Message = Constants.MESSAGE_SUCCESS
            };
        }
    }
}
