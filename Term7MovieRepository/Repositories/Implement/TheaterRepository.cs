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

                string query =
                    " SELECT Id, Name, Address, Latitude, Longitude, CompanyId, ManagerId, Status " +
                    " FROM Theaters " +
                    " WHERE Status = 1 ";
                string count =
                    " ; SELECT Count(1) " +
                    " FROM Theaters " +
                    " WHERE Status = 1 ";

                string sql = ConcatQueryWithFilter(query, count, request);

                object param = new { offset, fetch, CompanyId = request.CompanyId ?? 0 };

                var multiQ = await con.QueryMultipleAsync(sql, param);

                IEnumerable<TheaterDto> results = await multiQ.ReadAsync<TheaterDto>();
                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

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
                return null;
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

        private string ConcatQueryWithFilter(string query, string count, TheaterFilterRequest request)
        {
            if (request.CompanyId != null)
            {
                query += " AND CompanyId = @CompanyId ";
                count += " AND CompanyId = @CompanyId ";
            }

            query += 
                " ORDER BY Id " + // offset phai co order by...
                " OFFSET @offset ROWS " +
                " FETCH NEXT @fetch ROWS ONLY ";

            return query + count;
        }
    }
}
