﻿using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IShowtimeRepository
    {
        Task<PagingList<ShowtimeDto>> GetShowtimesByTheaterIdAsync(ShowtimeFilterRequest request);
        Task<ShowtimeDto> GetShowtimeByIdAsync(long id);
        Task<int> CreateShowtimeAsync(Showtime showtime);
        Task UpdateShowtimeAsync(Showtime showtime);
        int DeleteShowtimeById(long id);
    }
}
