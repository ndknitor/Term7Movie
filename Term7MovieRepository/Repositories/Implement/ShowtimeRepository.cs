using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Dto.Theater;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Utility;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;

        private const string FILTER_NOT_SHOWED_YET = "NOT_SHOWED";
        private const string FILTER_BY_MOVIE = "BY_MOVIE";
        private const string FILTER_BY_THEATER = "BY_THEATER";
        private const string FILTER_BY_DATE = "BY_DATE";

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
                    " SELECT sh.Id, sh.MovieId, sh.RoomId, sh.TheaterId, sh.StartTime, sh.EndTime " + 
                    (request.IsIncludeMovie ? ", m.Id, m.Title, m.Duration, m.RestrictedAge, m.PosterImageUrl ": "")  +
                    " FROM Showtimes sh " +
                    (request.IsIncludeMovie ? " JOIN Movies m ON sh.MovieId = m.Id " : "") +
                    " WHERE 1 = 1 " +

                    GetAdditionQueryString(request, FILTER_NOT_SHOWED_YET) +
                    GetAdditionQueryString(request, FILTER_BY_MOVIE) +
                    GetAdditionQueryString(request, FILTER_BY_THEATER) +
                    GetAdditionQueryString(request, FILTER_BY_DATE) +

                    " ORDER BY sh.Id " +
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ; ";

                string count =
                    " SELECT COUNT(1) " +
                    " FROM Showtimes sh " +
                    " WHERE 1 = 1 " +

                    GetAdditionQueryString(request, FILTER_NOT_SHOWED_YET) +
                    GetAdditionQueryString(request, FILTER_BY_MOVIE) +
                    GetAdditionQueryString(request, FILTER_BY_THEATER) +
                    GetAdditionQueryString(request, FILTER_BY_DATE) +
                    " ; ";
                    

                string showtimeTiketType = 
                    !request.IsIncludeTicketType ? "" : 
                   @" SELECT shtt.Id, shtt.ShowtimeId, shtt.TicketTypeId, shtt.ReceivePrice, tt.Id, tt.Name, tt.CompanyId 
                      FROM ShowtimeTicketTypes shtt JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id ";


                object param = new { request.TheaterId, request.Date?.Date, request.MovieId, offset, fetch };

                var multiQ = await con.QueryMultipleAsync(query + count + showtimeTiketType, param);

                IEnumerable<ShowtimeDto> results;

                if (request.IsIncludeMovie)
                {
                    results = multiQ.Read<ShowtimeDto, MovieModelDto, ShowtimeDto>((sh, m) =>
                    {
                        sh.Movie = m;
                        return sh;
                    }, splitOn: "Id");
                } 
                else
                {
                    results = await multiQ.ReadAsync<ShowtimeDto>();
                }

                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

                if (request.IsIncludeTicketType)
                {
                    IEnumerable<ShowtimeTicketTypeDto> showtimeTicketTypes = multiQ.Read<ShowtimeTicketTypeDto, TicketTypeDto, ShowtimeTicketTypeDto>((shtt, tt) =>
                    {
                        shtt.TicketType = tt;
                        return shtt;
                    });

                    foreach (ShowtimeDto showtime in results)
                    {
                        showtime.ShowtimeTicketTypes = showtimeTicketTypes.Where(shtt => shtt.ShowtimeId == showtime.Id);
                    }
                }

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
                    " WHERE th.ManagerId = @managerId " + 

                    GetAdditionQueryString(request, FILTER_NOT_SHOWED_YET) +

                    " ORDER BY StartTime " +
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ; ";

                string count =
                    " SELECT COUNT(1) " +
                    " FROM Showtimes sh JOIN Theaters th ON sh.TheaterId = th.Id " +
                    " WHERE th.ManagerId = @managerId " +

                    GetAdditionQueryString(request, FILTER_NOT_SHOWED_YET) + 
                    " ; ";

                string showtimeTiketType =
                   @" SELECT shtt.Id, shtt.ShowtimeId, shtt.TicketTypeId, shtt.ReceivePrice, tt.Id, tt.Name, tt.CompanyId 
                      FROM ShowtimeTicketTypes shtt JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id ";

                object param = new { managerId, offset, fetch };

                var multiQ = await con.QueryMultipleAsync(query + count + showtimeTiketType, param);

                IEnumerable<ShowtimeDto> results = multiQ.Read<ShowtimeDto, MovieModelDto, RoomDto,ShowtimeDto>((sh, m, r) =>
                {
                    sh.Movie = m;
                    sh.Room = r;
                    return sh;
                }, splitOn: "Id");
                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

                IEnumerable<ShowtimeTicketTypeDto> showtimeTicketTypes = multiQ.Read<ShowtimeTicketTypeDto, TicketTypeDto, ShowtimeTicketTypeDto>((shtt, tt) =>
                {
                    shtt.TicketType = tt;
                    return shtt;
                });

                foreach (ShowtimeDto showtime in results)
                {
                    showtime.ShowtimeTicketTypes = showtimeTicketTypes.Where(shtt => shtt.ShowtimeId == showtime.Id);
                }

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
                    " WHERE sh.Id = @id ; ";

                string showtimeTiketType =
                   @" SELECT shtt.Id, shtt.ShowtimeId, shtt.TicketTypeId, shtt.ReceivePrice, tt.Id, tt.Name, tt.CompanyId 
                      FROM ShowtimeTicketTypes shtt JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id 
                      WHERE shtt.ShowtimeId = @id ";

                object param = new { id };

                var multiQ = await con.QueryMultipleAsync(query + showtimeTiketType, param);

                var list = multiQ.Read<ShowtimeDto, MovieModelDto, RoomDto, ShowtimeDto>((sh, m, r) =>
                {
                    sh.Movie = m;
                    sh.Room = r;
                    return sh;
                }, splitOn: "Id");

                showtime = list.FirstOrDefault();

                if (showtime == null) throw new DbNotFoundException();

                showtime.ShowtimeTicketTypes = multiQ.Read<ShowtimeTicketTypeDto, TicketTypeDto, ShowtimeTicketTypeDto>((shtt, tt) =>
                {
                    shtt.TicketType = tt;
                    return shtt;
                });
            }
            return showtime;
        }
        public async Task<long> CreateShowtimeAsync(ShowtimeCreateRequest request)
        {
            long scopeIdentity = 0;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                await con.OpenAsync();
                var transaction = await con.BeginTransactionAsync();
                try
                {
                    string sql =
                    @" INSERT INTO Showtimes (MovieId, RoomId, StartTime, EndTime, TheaterId) 
                       SELECT @MovieId, @RoomId, @StartTime, DATEADD(MINUTE, Duration, @StartTime), @TheaterId 
                       FROM Movies 
                       WHERE Id = @MovieId ; ";

                    string getIdentity = @" SELECT SCOPE_IDENTITY() ; ";

                    scopeIdentity = await con.QueryFirstOrDefaultAsync<long>(sql + getIdentity, request, transaction:transaction);

                    string insertShowtimeTicketType =
                        @" INSERT INTO ShowtimeTicketTypes (Id, ShowtimeId, TicketTypeId, ReceivePrice) 
                           VALUES (NEWID(), @scopeIdentity, @TicketTypeId, @ReceivePrice) ";

                    object param;

                    int count = 0;
                    foreach (var showtimeTicketType in request.ShowtimeTicketTypes)
                    {
                        param = new
                        {
                            showtimeTicketType.TicketTypeId,
                            //showtimeTicketType.OriginalPrice,
                            showtimeTicketType.ReceivePrice,
                            scopeIdentity
                        };

                        count += await con.ExecuteAsync(insertShowtimeTicketType, param, transaction: transaction);
                    }

                    if (count == request.ShowtimeTicketTypes.Count())
                    {
                        transaction.Commit();
                        return scopeIdentity;
                    }
                }
                catch(DbUpdateException e)
                {
                    await transaction.RollbackAsync();
                    throw e;
                }
            }
            return scopeIdentity;
        }
        public async Task UpdateShowtimeAsync(Showtime showtime)
        {
            Showtime dbShowtime = await _context.Showtimes.FindAsync(showtime.Id);

            if (dbShowtime == null) return;
            if (dbShowtime.StartTime < DateTime.UtcNow) return;

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
                string checkStartime =
                    @" ; WITH CTE_GETDURATION_BY_MOVIEID(Duration) AS (

                            SELECT Duration
                            FROM Movies
                            WHERE Id = @MovieId
                        )

                       SELECT 1
                       FROM Showtimes
                       WHERE RoomId = @RoomId AND TheaterId = @TheaterId 
                                        AND @StartTime BETWEEN StartTime AND EndTime
                                        AND (
                                                SELECT DATEADD(MINUTE, Duration, @StartTime)
                                                FROM CTE_GETDURATION_BY_MOVIEID
                                            ) BETWEEN StartTime AND EndTime ; ";

                int count = await con.QueryFirstOrDefaultAsync<int>(checkStartime, request);

                valid = count == 0;
            }

            return valid;
        }

        public async Task<bool> IsShowtimeNotOverlap(ShowtimeUpdateRequest request)
        {
            bool valid = false;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    @" ; WITH CTE_GETDURATION_BY_MOVIEID(Duration) AS (

                            SELECT Duration
                            FROM Movies
                            WHERE Id = @MovieId
                        )

                       SELECT 1
                       FROM Showtimes sh
                       WHERE Id = @Id 
                              AND @StartTime BETWEEN StartTime AND EndTime
                              AND (
                                      SELECT DATEADD(MINUTE, Duration, @StartTime)
                                      FROM CTE_GETDURATION_BY_MOVIEID
                                  ) BETWEEN StartTime AND EndTime ; ";

                int count = await con.QueryFirstOrDefaultAsync<int>(sql, request);

                valid = count == 0;
            }

            return valid;
        }

        private string GetAdditionQueryString(ShowtimeFilterRequest request, string filter)
        {
            string query = "";
            switch(filter)
            {
                case FILTER_NOT_SHOWED_YET:
                    if (request.IsNotShowedYet) query = " AND sh.StartTime > GETUTCDATE() ";
                    break;
                case FILTER_BY_THEATER:
                    if (request.TheaterId != null) query = " AND sh.TheaterId = @TheaterId ";
                    break;
                case FILTER_BY_MOVIE:
                    if (request.MovieId != null) query = " AND sh.MovieId = @MovieId ";
                    break;
                case FILTER_BY_DATE:
                    if (request.Date != null) query = " AND CAST(sh.StartTime AS DATE) = @Date ";
                    break;
            }

            return query;
        }

        public async Task<IEnumerable<TheaterShowTimeLocationDTO>> GetRecentlyShowTimeForMovieHomepage()
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            var result = new List<TheaterShowTimeLocationDTO>();       //TEMPORARY 60 days
            var query = await _context.Showtimes
                                            .Include(x => x.Theater)
                                            .Where(x => x.StartTime < DateTime.UtcNow.AddDays(60))
                                            .DistinctBy(a => a.MovieId)
                                            .Select(xxx => new TheaterShowTimeLocationDTO
                                            {
                                                MovieId = xxx.MovieId,
                                                ShowTimeId = xxx.Id,
                                                StartTime = xxx.StartTime,
                                                TheaterId = xxx.TheaterId != null ? xxx.TheaterId.Value : -1,
                                                Location = new Coordinate
                                                {
                                                    Latitude = double.Parse(xxx.Theater.Latitude),
                                                    Longitude = double.Parse(xxx.Theater.Longitude),
                                                }
                                            })
                                            .ToListAsync();
            if (!query.Any() || query.Count < 3) throw new Exception("NOT ENOUGH MANA");
            result = query;
            //first round
            return result;

        }

        public async Task<IEnumerable<TheaterShowTimeDTO>> GetRecentlyShowTimeWithMinutesRemain(Coordinate userlocation)
        {
            //So distinct, group by or anyshit isn't working so i have to manually reject those duplicated movie
            //sorry performance-sama
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            //var result = new List<TheaterShowTimeDTO>();       //TEMPORARY 45 days
            DateTime rightnow = DateTime.UtcNow;
            var query = await _context.Showtimes
                                            .Include(x => x.Theater)
                                            .Include(x => x.Movie)
                                            .Where(x => x.StartTime < rightnow.AddDays(45)
                                                && x.StartTime > rightnow)
                                            //.AsQueryable()
                                            //.DistinctBy(a => a.MovieId)
                                            .Select(xxx => new TheaterShowTimeDTO
                                            {
                                                MovieId = xxx.MovieId,
                                                ShowTimeId = xxx.Id,
                                                MinutesRemain = (xxx.StartTime - rightnow).TotalMinutes,
                                                MovieTitle = xxx.Movie.Title,
                                                CoverImageURL = xxx.Movie.CoverImageUrl,
                                                TheaterId = xxx.TheaterId != null ? xxx.TheaterId.Value : -1,
                                                DistanceFromUser = CalculateDistanceByHaversine(userlocation,
                                                    new Coordinate
                                                    {
                                                        Latitude = double.Parse(xxx.Theater.Latitude),
                                                        Longitude = double.Parse(xxx.Theater.Longitude)
                                                    }),
                                                StartTime = xxx.StartTime,
                                                TheaterName = xxx.Theater.Name,
                                            })
                                            .ToListAsync();
            if (!query.Any() || query.Count < 3) throw new Exception("NOT ENOUGH MANA");
            return query;
            //int firstmovieid = query.FirstOrDefault().MovieId;
            //double firsminutesremain = query.FirstOrDefault().MinutesRemain; //đóng vài trò làm biến min nựa
            //king crimson
            //vì 1 cái list movieid lung tung nên chả biết nó ở đâu mà lần huhu
            //First step lọc ra số movieid unique
            //IEnumerable<int> movieids = query.Select(x => x.MovieId).Distinct();
            //foreach (int movieid in movieids)
            //{
            //    foreach (var movie in query.Where(x => x.MovieId == movieid))
            //    {
            //        double ChoosenRecommend = Math.Abs(0.8 * movie.MinutesRemain) + movie.DistanceFromUser * 0.2;
            //        if (ChoosenRecommend <= recommend)
            //        {
            //            movie.RecommendPoint = ChoosenRecommend;
            //            result.Add(movie);
            //            break;
            //        }
            //    }
            //    kono power...
            //    var nuggest = query.Where(lmao => lmao.MovieId == movieid)
            //        .MaxBy(x => Math.Abs(x.MinutesRemain * 0.8) + x.DistanceFromUser * 0.2);
            //    result.Add(nuggest);
            //    var item = query.First(a => a.MovieId == movieid);
            //    double recommendpoint = //Min
            //        (item.MinutesRemain * 0.8) + item.DistanceFromUser * 0.2;
            //    foreach (var movie in query.Where(x => x.MovieId == movieid))
            //    {
            //        double currentRecommendpoint = Math.Abs(movie.MinutesRemain * 0.8) + movie.DistanceFromUser * 0.2;
            //        bool hmmmm = currentRecommendpoint > recommendpoint;
            //        if (currentRecommendpoint > recommendpoint) //nếu tìm thấy phim ko ưu tiên skip
            //            result.Remove(movie);
            //        else if (currentRecommendpoint <= recommendpoint) //tìm thấy dòng thời gian hợp lý nhét dô đỡ phải tính bên sơ vịt
            //            movie.RecommendPoint = currentRecommendpoint;
            //    }
            //}
            //if (result.Count < 3) throw new Exception("NOT ENOUGH MANA");
            //return result;
        }

        //memory lost some how?
        private static double CalculateDistanceByHaversine(Coordinate Start, Coordinate Destination) //Unit Meters
        {//Look fresh enough mlem mlem
            var d1 = Start.Latitude * (Math.PI / 180.0);
            var num1 = Start.Longitude * (Math.PI / 180.0);
            var d2 = Destination.Latitude * (Math.PI / 180.0);
            var num2 = Destination.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public async Task<bool> CanManagerCreateTicket(long managerId, IEnumerable<Guid> showtimeTicketTypeId, IEnumerable<long> seatId)
        {
            bool valid = false;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" SELECT COUNT(*) OVER()
                       FROM Showtimes sh JOIN Theaters th ON sh.TheaterId = th.Id
                            JOIN Seats s ON s.RoomId = sh.RoomId
                            JOIN ShowtimeTicketTypes shtt ON shtt.ShowtimeId = sh.Id
                       WHERE th.ManagerId = @managerId 
                            AND shtt.Id IN @showtimeTicketTypeId 
                            AND s.Id IN @seatId
                            AND sh.StartTime > GETUTCDATE()
                       GROUP BY s.Id ";   // count(*) over(): count after group by

                object param = new { managerId, showtimeTicketTypeId, seatId };

                int count = await con.QueryFirstOrDefaultAsync<int>(sql, param);

                if (count == seatId.Count()) return true;
            }

            return valid;
        }

        public async Task<ShowtimeQuanityDTO> GetQuickShowtimeQuanity(long managerid,
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            int TotalShowtime = await _context.Showtimes.Include(a => a.Theater)
                                            .Where(xxx => xxx.Theater.ManagerId == managerid)
                                            .CountAsync();
            int ShowtimePreviousWeek = await _context.Showtimes.Include(a => a.Theater)
                                            .Where(xxx => xxx.Theater.ManagerId == managerid &&
                                            xxx.StartTime <= SundayPreviousWeek && xxx.StartTime >= MondayPreviousWeek)
                                            .CountAsync();
            int ShowtimeThisWeek = await _context.Showtimes.Include(a => a.Theater)
                                            .Where(xxx => xxx.Theater.ManagerId == managerid &&
                                            xxx.StartTime <= DateTime.UtcNow && xxx.StartTime >= ThisMondayWeek)
                                            .CountAsync();
            if(TotalShowtime == 0 && ShowtimePreviousWeek == 0 && ShowtimeThisWeek == 0)
            {
                TotalShowtime = 1;
                ShowtimePreviousWeek = 1;
                ShowtimeThisWeek = 1;
            }
            ShowtimeQuanityDTO dto = new ShowtimeQuanityDTO();
            dto.TotalShowtimeQuantity = TotalShowtime;
            dto.OldShowtimeQuantity = ShowtimePreviousWeek;
            dto.NewShowtimeQuantity = ShowtimeThisWeek;
            if(ShowtimeThisWeek > ShowtimePreviousWeek)
            {
                dto.PercentShowtimeChange = (float)100F - ((float)ShowtimePreviousWeek * (float)100F / (float)ShowtimeThisWeek);
                dto.IsShowtimeUpOrDown = true;
            }
            else if(ShowtimeThisWeek < ShowtimePreviousWeek)
            {
                dto.PercentShowtimeChange = (float)100F - ((float)ShowtimeThisWeek * (float)100F / (float)ShowtimePreviousWeek);
                dto.IsShowtimeUpOrDown = false;
            }
            else if(ShowtimeThisWeek == ShowtimePreviousWeek)
            {
                dto.PercentShowtimeChange = 0.69F;
                dto.IsShowtimeUpOrDown = true; //be positive, why not (:
            }
            return dto;
        }

        public async Task<ShowtimeQuanityDTO> GetQuickShowtimeQuanity(DateTime ThisMondayWeek, 
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            int TotalShowtime = await _context.Showtimes
                                            .CountAsync();
            int ShowtimePreviousWeek = await _context.Showtimes
                                            .Where(xxx => xxx.StartTime <= SundayPreviousWeek 
                                                        && xxx.StartTime >= MondayPreviousWeek)
                                                        .CountAsync();
            int ShowtimeThisWeek = await _context.Showtimes
                                            .Where(xxx => xxx.StartTime <= DateTime.UtcNow 
                                                        && xxx.StartTime >= ThisMondayWeek)
                                                        .CountAsync();
            if (TotalShowtime == 0 && ShowtimePreviousWeek == 0 && ShowtimeThisWeek == 0)
            {
                TotalShowtime = 1;
                ShowtimePreviousWeek = 1;
                ShowtimeThisWeek = 1;
            }
            ShowtimeQuanityDTO dto = new ShowtimeQuanityDTO();
            dto.TotalShowtimeQuantity = TotalShowtime;
            dto.OldShowtimeQuantity = ShowtimePreviousWeek;
            dto.NewShowtimeQuantity = ShowtimeThisWeek;
            if (ShowtimeThisWeek > ShowtimePreviousWeek)
            {
                dto.PercentShowtimeChange = (float)100F - ((float)ShowtimePreviousWeek * (float)100F / (float)ShowtimeThisWeek);
                dto.IsShowtimeUpOrDown = true;
            }
            else if (ShowtimeThisWeek < ShowtimePreviousWeek)
            {
                dto.PercentShowtimeChange = (float)100F - ((float)ShowtimeThisWeek * (float)100F / (float)ShowtimePreviousWeek);
                dto.IsShowtimeUpOrDown = false;
            }
            else if (ShowtimeThisWeek == ShowtimePreviousWeek)
            {
                dto.PercentShowtimeChange = 0.69F;
                dto.IsShowtimeUpOrDown = true; //be positive, why not (:
            }
            return dto;
        }
    }
}
