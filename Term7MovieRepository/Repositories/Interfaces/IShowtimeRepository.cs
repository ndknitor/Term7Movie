﻿using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IShowtimeRepository
    {
        Task<PagingList<ShowtimeDto>> GetShowtimesByTheaterIdAsync(ShowtimeFilterRequest request);

        Task<PagingList<ShowtimeDto>> GetShowtimesByManagerIdAsync(ShowtimeFilterRequest request, long managerId);
        Task<ShowtimeDto> GetShowtimeByIdAsync(long id);
        Task<long> CreateShowtimeAsync(Showtime showtime);
        Task UpdateShowtimeAsync(Showtime showtime);
        int DeleteShowtimeById(long id);
        Task<bool> IsShowtimeNotOverlap(ShowtimeCreateRequest request);

        Task<bool> CanManagerCreateShowtime(ShowtimeCreateRequest request, long managerId);
    }
}
