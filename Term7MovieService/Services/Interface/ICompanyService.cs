﻿using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ICompanyService
    {
        Task<CompanyDetailResponse> GetCompanyDetailAsync(int? companyId, long? managerId = null);
        Task<CompanyListResponse> GetAllCompanyAsync(ParentFilterRequest request);
    }
}
