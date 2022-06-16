using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public ShowtimeRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<PagingList<ShowtimeDto>> GetShowtimesByTheaterIdAsync(ShowtimeFilterRequest request)
        {
            PagingList<ShowtimeDto> list = new();
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize * (request.Page - 1 );
                int fetch = request.PageSize;

                string query =
                    " SELECT sh.Id, sh.MovieId, sh.RoomId, sh.TheaterId, sh.StartTime, sh.EndTime, m.Id, m.Title, m.Duration, m.RestrictedAge, m.PosterImageUrl " +
                    " FROM Showtimes sh JOIN Movies m ON sh.MovieId = m.Id " +
                    " WHERE TheaterId = @TheaterId AND StartTime > GETUTCDATE() " +
                    " ORDER BY StartTime " +
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ; ";

                string count =
                    " SELECT COUNT(1) " +
                    " FROM Showtimes " +
                    " WHERE TheaterId = @TheaterId AND StartTime > GETUTCDATE() ";
                object param = new { request.TheaterId,  offset, fetch };

                var multiQ = await con.QueryMultipleAsync(query + count, param);
                IEnumerable<ShowtimeDto> results = multiQ.Read<ShowtimeDto, MovieModelDto, ShowtimeDto>((sh, m) =>
                {
                    sh.Movie = m;
                    return sh;
                }, splitOn : "Id");
                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

                list = new PagingList<ShowtimeDto>(request.PageSize, request.Page, results, total);
            }
            return list;
        }

        public async Task<PagingList<ShowtimeDto>> GetShowtimesByManagerIdAsync(ShowtimeFilterRequest request, long managerId)
        {
            PagingList<ShowtimeDto> list = new();
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize * (request.Page - 1);
                int fetch = request.PageSize;

                string query =
                    " SELECT sh.Id, sh.MovieId, sh.RoomId, sh.TheaterId, th.Name as 'TheaterName', sh.StartTime, sh.EndTime, m.Id, m.Title, m.Duration, m.RestrictedAge, m.PosterImageUrl, r.Id, r.No " +
                    " FROM Showtimes sh JOIN Movies m ON sh.MovieId = m.Id " +
                    "   JOIN Theaters th ON sh.TheaterId = th.Id " +
                    "   JOIN Rooms r ON sh.RoomId = r.Id " +
                    " WHERE th.ManagerId = @managerId AND StartTime > GETUTCDATE() " +
                    " ORDER BY StartTime " +
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ; ";

                string count =
                    " SELECT COUNT(1) " +
                    " FROM Showtimes sh JOIN Theaters th ON sh.TheaterId = th.Id " +
                    " WHERE th.ManagerId = @managerId AND StartTime > GETUTCDATE() ";
                object param = new { managerId, offset, fetch };

                var multiQ = await con.QueryMultipleAsync(query + count, param);
                IEnumerable<ShowtimeDto> results = multiQ.Read<ShowtimeDto, MovieModelDto, RoomDto,ShowtimeDto>((sh, m, r) =>
                {
                    sh.Movie = m;
                    sh.Room = r;
                    return sh;
                }, splitOn: "Id");
                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

                list = new PagingList<ShowtimeDto>(request.PageSize, request.Page, results, total);
            }
            return list;
        }

        public async Task<ShowtimeDto> GetShowtimeByIdAsync(long id)
        {
            ShowtimeDto showtime = null;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string query =
                    " SELECT sh.Id, sh.MovieId, sh.RoomId, sh.TheaterId, sh.StartTime, sh.EndTime, m.Id, m.Title, m.Duration, m.RestrictedAge, m.PosterImageUrl, r.Id, r.No, r.TheaterId, r.NumberOfRow, r.NumberOfColumn, r.Status " +
                    " FROM Showtimes sh JOIN Movies m ON sh.MovieId = m.Id " +
                    "       JOIN Rooms r ON sh.RoomId = r.Id " +
                    " WHERE sh.Id = @id AND sh.StartTime > GETUTCDATE() ";

                object param = new { id };

                var list = await con.QueryAsync<ShowtimeDto, MovieModelDto, RoomDto, ShowtimeDto>(query, (sh, m, r) =>
                {
                    sh.Movie = m;
                    sh.Room = r;
                    return sh;
                }, param, splitOn: "Id");

                if (list != null)
                {
                    showtime = list.FirstOrDefault();
                }
            }
            return showtime;
        }
        public async Task<long> CreateShowtimeAsync(Showtime showtime)
        {
            long scopeIdentity = 0;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    " INSERT INTO Showtimes (MovieId, RoomId, StartTime, EndTime, TheaterId) " +
                    " SELECT @MovieId, @RoomId, @StartTime, DATEADD(MINUTE, Duration, @StartTime), @TheaterId " +
                    " FROM Movies " +
                    " WHERE Id = @MovieId ; ";

                string getIdentity = @" SELECT SCOPE_IDENTITY() ; ";

                scopeIdentity = await con.QueryFirstOrDefaultAsync<long>(sql + getIdentity, showtime);
            }
            return scopeIdentity;
        }
        public async Task UpdateShowtimeAsync(Showtime showtime)
        {
            Showtime dbShowtime = await _context.Showtimes.FindAsync(showtime.Id);

            if (dbShowtime == null) return;

            double duration = (dbShowtime.StartTime - dbShowtime.EndTime).TotalMinutes;
            dbShowtime.StartTime = showtime.StartTime;

            dbShowtime.EndTime = showtime.StartTime.AddMinutes(duration);
        }
        public int DeleteShowtimeById(long id)
        {
            int count = 0;
            return count;
        }

        public async Task<bool> CanManagerCreateShowtime(ShowtimeCreateRequest request, long managerId)
        {
            bool valid = false;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT 1
                                FROM Rooms r JOIN Theaters th ON r.TheaterId = th.Id  
                                WHERE r.Id = @RoomId AND TheaterId = @TheaterId AND th.ManagerId = @managerId ";

                object param = new { request.TheaterId, request.RoomId, managerId };

                int count = await con.QueryFirstOrDefaultAsync<int>(sql, param);

                valid = count == 1;
            }

            return valid;
        }

        public async Task<bool> CanManagerUpdateShowtime(ShowtimeUpdateRequest request, long managerId)
        {
            bool valid = false;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT 1
                                FROM Showtimes sh JOIN Theaters th ON sh.TheaterId = th.Id 
                                WHERE Id = @Id AND th.ManagerId = @managerId ";

                object param = new { request.Id, managerId };

                int count = await con.QueryFirstOrDefaultAsync<int>(sql, param);

                valid = count == 1;
            }

            return valid;
        }

        public async Task<bool> IsShowtimeNotOverlap(ShowtimeCreateRequest request)
        {
            bool valid = false;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT 1
                                FROM Showtimes 
                                WHERE RoomId = @RoomId AND TheaterId = @TheaterId AND @StartTime BETWEEN StartTime AND EndTime ";

                int count = await con.QueryFirstOrDefaultAsync<int>(sql, request);

                valid = count == 0;
            }

            return valid;
        }

        public async Task<bool> IsShowtimeNotOverlap(ShowtimeUpdateRequest request)
        {
            bool valid = false;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT 1
                                FROM Showtimes 
                                WHERE Id = @Id AND @StartTime BETWEEN StartTime AND EndTime ";

                int count = await con.QueryFirstOrDefaultAsync<int>(sql, request);

                valid = count == 0;
            }

            return valid;
        }
    }
}
