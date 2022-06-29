using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Theater;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;

        private const string FILTER_THEATER_NAME = "Name";
        public TheaterRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }
        public async Task<PagingList<TheaterDto>> GetAllTheaterAsync(TheaterFilterRequest request)
        {
            PagingList<TheaterDto> list = new();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize * (request.Page - 1);
                int fetch = request.PageSize;

                string sql =
                    " SELECT Id, Name, Address, Latitude, Longitude, CompanyId, ManagerId, Status " +
                    " FROM Theaters " +
                    " WHERE Status = 1 " +

                    GetAdditionalTheaterFilter(request, FILTER_THEATER_NAME) + 

                    " ORDER BY Id " + // offset phai co order by...
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ";

                string count =
                    " ; SELECT Count(1) " +
                    " FROM Theaters " +
                    " WHERE Status = 1 " +
                    GetAdditionalTheaterFilter(request, FILTER_THEATER_NAME);

                string roomQuery =
                    (!request.IsIncludeRoom) ? "" :
                    @" ; SELECT Id, No, TheaterId, NumberOfRow, NumberOfColumn, Status 
                         FROM Rooms 
                         WHERE Status = 1 ";

                object param = new { offset, fetch, request.SearchKey };

                var multiQ = await con.QueryMultipleAsync(sql + count + roomQuery, param);

                IEnumerable<TheaterDto> results = await multiQ.ReadAsync<TheaterDto>();

                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

                if (request.IsIncludeRoom)
                {
                    IEnumerable<RoomDto> rooms = await multiQ.ReadAsync<RoomDto>();

                    foreach (TheaterDto t in results)
                    {
                        t.Rooms = rooms.Where(r => r.TheaterId == t.Id);
                        t.TotalRoom = t.Rooms.Count();
                    }
                }

                list = new PagingList<TheaterDto>(pageSize: request.PageSize, page: request.Page, results, total);
            }

            return list;
        }

        public async Task<PagingList<TheaterDto>> GetAllTheaterByManagerIdAsync(TheaterFilterRequest request, long managerId)
        {
            PagingList<TheaterDto> list = new();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize * (request.Page - 1);
                int fetch = request.PageSize;

                string query =
                    " SELECT Id, Name, Address, Latitude, Longitude, CompanyId, ManagerId, Status " +
                    " FROM Theaters " +
                    " WHERE Status = 1 AND ManagerId = @managerId " +

                    GetAdditionalTheaterFilter(request, FILTER_THEATER_NAME) +

                    " ORDER BY Id " + // offset phai co order by...
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ";

                string count =
                    " ; SELECT Count(1) " +
                    " FROM Theaters " +
                    " WHERE Status = 1 AND ManagerId = @managerId " +
                    GetAdditionalTheaterFilter(request, FILTER_THEATER_NAME);

                string roomQuery =
                    @" ; SELECT Id, No, TheaterId, NumberOfRow, NumberOfColumn, Status 
                         FROM Rooms 
                         WHERE Status = 1 ";

                object param = new { offset, fetch, managerId, request.SearchKey };

                var multiQ = await con.QueryMultipleAsync(query + count + roomQuery, param);

                IEnumerable<TheaterDto> results = await multiQ.ReadAsync<TheaterDto>();

                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

                IEnumerable<RoomDto> rooms = await multiQ.ReadAsync<RoomDto>();

                foreach (TheaterDto t in results)
                {
                    t.Rooms = rooms.Where(r => r.TheaterId == t.Id);
                    t.TotalRoom = t.Rooms.Count();
                }

                list = new PagingList<TheaterDto>(pageSize: request.PageSize, page: request.Page, results, total);
            }

            return list;
        }

        public async Task<TheaterDto> GetTheaterByIdAsync(int id)
        {
            TheaterDto theater = null;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string queryTheater =
                    " SELECT Id, Name, Address, Latitude, Longitude, CompanyId, ManagerId, Status " +
                    " FROM Theaters " +
                    " WHERE Status = 1 AND Id = @id ; ";

                string queryShowtime = 
                    @" SELECT sh.Id, sh.MovieId, sh.RoomId, sh.StartTime, sh.EndTime, sh.TheaterId, m.Id, m.Title, m.RestrictedAge, m.PosterImageUrl, m.CoverImageUrl, m.TrailerUrl, m.Duration
                       FROM Showtimes sh JOIN Movies m on sh.MovieId = m.Id
                       WHERE TheaterId = @id AND StartTime > GETUTCDATE() ";

                object param = new { id };

                var multiQ = await con.QueryMultipleAsync(queryTheater + queryShowtime, param);
                
                theater = await multiQ.ReadFirstOrDefaultAsync<TheaterDto>();

                if (theater != null)
                {
                    theater.Showtimes = multiQ.Read<ShowtimeDto, MovieModelDto, ShowtimeDto>(
                        (sh, m) =>
                        {
                            sh.Movie = m;
                            return sh;
                        }, splitOn: "Id");
                }
            }

            return theater;
        }
        public async Task CreateTheaterAsync(Theater theater)
        {
            await _context.Theaters.AddAsync(theater);
        }
        public async Task UpdateTheaterAsync(Theater theater)
        {
            Theater dbTheater = await _context.Theaters.FindAsync(theater.Id);

            if (theater == null) return;

            dbTheater.Name = theater.Name;
            dbTheater.Address = theater.Address;
            dbTheater.Latitude = theater.Latitude;
            dbTheater.Longitude = theater.Longitude;
            dbTheater.Status = theater.Status;
        }
        public async Task DeleteTheaterAsync(int id)
        {
            Theater theater = await _context.Theaters.FindAsync(id);

            if (theater == null) return;

            theater.Status = false;
        }

        public async Task<IEnumerable<TheaterDto>> GetTheaterByManagerIdAsync(long managerId)
        {
            IEnumerable<TheaterDto> theaters = new List<TheaterDto>();

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    " SELECT Id " +
                    " FROM Theaters " +
                    " WHERE ManagerId = @managerId ";
                object param = new { managerId };
                theaters = await con.QueryAsync<TheaterDto>(sql, param);
            }

            return theaters;
        }

        public async Task<IEnumerable<TheaterNameDTO>> GetAllTheaterByCompanyIdAsync(int companyid)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            List<TheaterNameDTO> result = new List<TheaterNameDTO>();
            var query = _context.Theaters
                            .Where(a => a.CompanyId == companyid)
                            .Select(xxx => new TheaterNameDTO
                            {
                                Name = xxx.Name,
                                TheaterId = xxx.Id
                            });
            if (!query.Any()) throw new Exception("EMPTYDATA");
            result = await query.ToListAsync();
            return result;
        }

        private string GetAdditionalTheaterFilter(TheaterFilterRequest request, string filter)
        {
            string sql = "";

            switch(filter)
            {
                case FILTER_THEATER_NAME:
                    if (!string.IsNullOrEmpty(request.SearchKey)) sql = " AND Name LIKE CONCAT('%', @SearchKey, '%') ";
                    break;
            }

            return sql;
        }
    }
}
