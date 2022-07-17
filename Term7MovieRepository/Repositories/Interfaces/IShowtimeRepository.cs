﻿using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Dto.Theater;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Utility;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IShowtimeRepository
    {
        Task<PagingList<ShowtimeDto>> GetShowtimesByTheaterIdAsync(ShowtimeFilterRequest request);

        Task<PagingList<ShowtimeDto>> GetShowtimesByManagerIdAsync(ShowtimeFilterRequest request, long managerId);
        Task<ShowtimeDto> GetShowtimeByIdAsync(long id);
        Task<long> CreateShowtimeAsync(ShowtimeCreateRequest request);
        Task UpdateShowtimeAsync(Showtime showtime);
        int DeleteShowtimeById(long id);
        Task<bool> IsShowtimeNotOverlap(ShowtimeCreateRequest request);

        Task<bool> CanManagerCreateShowtime(ShowtimeCreateRequest request, long managerId);

        Task<bool> CanManagerCreateTicket(long managerId, IEnumerable<Guid> showtimeTicketTypeId, IEnumerable<long> seatId);
        Task<IEnumerable<TheaterShowTimeLocationDTO>> GetRecentlyShowTimeForMovieHomepage();

        Task<IEnumerable<TheaterShowTimeDTO>> GetRecentlyShowTimeWithMinutesRemain(Coordinate UserLocation);

        Task<ShowtimeQuanityDTO> GetShowtimeQuanityInTwoRecentWeek(long managerid, 
            DateTime ThisMondayWeek, DateTime ThisSundayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);

        Task<ShowtimeQuanityDTO> GetShowtimeQuanityInTwoRecentWeek(DateTime ThisMondayWeek, DateTime ThisSundayWeek,
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);

        Task<ShowtimeQuanityDTO> GetShowtimeQuanityInTwoRecentMonth(long managerid,
            DateTime ThisFirstMonth, DateTime ThisLastMonth, DateTime FirstPreviousMonth, DateTime LastPreviousMonth);

        Task<ShowtimeQuanityDTO> GetShowtimeQuanityInTwoRecentMonth(DateTime ThisFirstMonth, DateTime ThisLastMonth,
            DateTime FirstPreviousMonth, DateTime LastPreviousMonth);
    }
}
