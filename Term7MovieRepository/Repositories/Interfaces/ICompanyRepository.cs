﻿using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<PagingList<CompanyDto>> GetAllCompany(CompanyFilterRequest request);
        Task<IEnumerable<CompanyDto>> GetAllCompanyNoPaging();
        Task<CompanyDto> GetCompanyById(int id);

        Task<CompanyDto> GetCompanyByManagerId(long managerId);
        Task<int> CreateCompany(CompanyCreateRequest request);
        Task<int> UpdateCompany(CompanyUpdateRequest request);
        int DeleteCompany(int id);

        Task<long?> GetManagerIdFromCompanyId(int companyid);
    }
}
