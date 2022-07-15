﻿using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Theater;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITheaterRepository
    {
        Task<PagingList<TheaterDto>> GetAllTheaterAsync(TheaterFilterRequest request);
        Task UpdateDefaultPriceAsync(TheaterDefaultPriceUpdateRequest request, long managerId);
        Task<PagingList<TheaterDto>> GetAllTheaterByManagerIdAsync(TheaterFilterRequest request, long managerId);
        Task<TheaterDto> GetTheaterByIdAsync(int id);
        Task CreateTheaterAsync(Theater theater);
        Task UpdateTheaterAsync(Theater theater);
        Task DeleteTheaterAsync(int id);
        Task<IEnumerable<TheaterDto>> GetTheaterByManagerIdAsync(long managerId);
        Task<IEnumerable<TheaterNameDTO>> GetAllTheaterByCompanyIdAsync(int companyid);
    }
}
