using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Options;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Request;
using Dapper;
using Term7MovieCore.Data.Collections;

namespace Term7MovieRepository.Repositories.Implement
{
    public class MovieRepository : IMovieRepository
    {

        private AppDbContext _context;
        private readonly ConnectionOption _connectionOption;

        public MovieRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }
        public async Task<PagingList<MovieDto>> GetAllMovie(ParentFilterRequest request)
        {
            PagingList<MovieDto> pagingList;
            int fetch = request.PageSize;
            int offset = (request.Page - 1) * request.PageSize;
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string query =
                    " SELECT Id, Title, ReleaseDate, Duration, RestrictedAge, PosterImageUrl, CoverImageUrl, TrailerUrl, Description, ViewCount, TotalRating " +
                    " FROM view_movies_sorted_by_release_date_desc " +
                    " ORDER BY ReleaseDate DESC " +
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ; ";
                string count = " SELECT COUNT(1) FROM Movies ";

                object param = new { offset, fetch };
                
                var multiQ = await con.QueryMultipleAsync(query + count, param);
                IEnumerable<MovieDto> list = await multiQ.ReadAsync<MovieDto>();
                long total = await multiQ.ReadFirstAsync<long>();

                pagingList = new PagingList<MovieDto>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
        }
        public async Task<Movie> GetMovieById(int id)
        {
            Movie movie = null;
            movie = await _context.Movies.FindAsync(id);
            return movie;
        }
        public async Task CreateMovie(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            //return count;
        }
        public async Task UpdateMovie(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
        public int Count()
        {
            int count = _context.Movies.Count();
            return count;
        }

        /* ------------- START QUERYING FOR MOVIE SHOW ON HOMEPAGE --------------------- */
        //public async Task<IEnumerable<MovieSoldDTO>> GetFourMoviesForFun()
        //{
        //    List<MovieSoldDTO> list = new List<MovieSoldDTO>();
        //    //SELECT COUNT(th.Id) AS SOLD, m.Id AS Movie_ID FROM
        //    //Movies m LEFT JOIN ShowTimes st ON m.Id = st.MovieId
        //    //LEFT JOIN Tickets tick ON tick.ShowTimeId = st.Id
        //    //LEFT JOIN TransactionHistories th ON tick.Id = th.TicketId
        //    //WHERE th.Id IS NOT NULL
        //    //GROUP BY m.Id 
        //    var query = from xxx in _context.Movies
        //                join st in _context.Showtimes on xxx.Id equals st.MovieId
        //                join tick in _context.Tickets on st.
        //}

        public async Task<IEnumerable<Movie>> GetLessThanThreeLosslessLatestMovies()
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception();
                //return null;
            List<Movie> movies = new List<Movie>();
            var query = _context.Movies
                .Where(a => a.ReleaseDate > DateTime.Now
                            && a.ReleaseDate < DateTime.Now.AddMonths(1)
                            && !string.IsNullOrEmpty(a.CoverImageUrl)
                            && !string.IsNullOrEmpty(a.PosterImageUrl))
                .OrderByDescending(a => a.ReleaseDate)
                .Select(a => new Movie
                {
                    Id = a.Id,
                    //overImageUrl = a.CoverImageUrl,
                    PosterImageUrl = a.PosterImageUrl
                })
                .Take(3);
            movies = query.ToList();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetEightLatestMovies()
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception();
            //return null;
            List<Movie> movies = new List<Movie>();
            var query = _context.Movies
                //lấy phim tính từ 1 tháng trước đến bây giờ
                //order by sẽ được tối ưu hơn khi chỉ lấy phim trong vòng 1 tháng (nếu performance chưa lên thì sẽ chơi trò khác :D)
                .Where(a => a.ReleaseDate < DateTime.Now
                            && a.ReleaseDate > DateTime.Now.AddMonths(-1)
                            && !string.IsNullOrEmpty(a.CoverImageUrl)
                            && !string.IsNullOrEmpty(a.PosterImageUrl))
                .OrderByDescending(a => a.ReleaseDate)
                .Select(a => new Movie
                {
                    Id = a.Id,
                    CoverImageUrl = a.CoverImageUrl,
                    PosterImageUrl = a.PosterImageUrl,
                    Title = a.Title,
                    ReleaseDate = a.ReleaseDate,
                    Duration = a.Duration,
                    RestrictedAge = a.RestrictedAge
                })
                .Take(8);
            movies = query.ToList();
            return movies;

        }

        public async Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception();
            Dictionary<int, IEnumerable<MovieType>> result = new Dictionary<int, IEnumerable<MovieType>>();
            foreach(int movieId in MovieIds)
            {
                var list = _context.MovieCategories
                                                .Include(a => a.Category)
                                                .Where(a => a.MovieId == movieId).ToList();
                List<MovieType> categories = new List<MovieType>();
                foreach(var category in list)
                {
                    MovieType mt = new MovieType();
                    mt.CateId = category.CategoryId;
                    mt.CateName = category.Category.Name;
                    categories.Add(mt);
                }
                result.Add(movieId, categories);
            }


            return result;
        }
        /* ------------- END QUERYING FOR MOVIE SHOW ON HOMEPAGE --------------------- */
        
        /* ------------- START QUERYING PAGING MOVIE INTO LIST ----------------------------- */
        //high end paging - flow: paging on database rather than the above shit
        public async Task<IEnumerable<Movie>> GetMoviesFromSpecificPage(int page, int pageCapacity)
        {
            if (!await _context.Database.CanConnectAsync())
                return null;
            List<Movie> movies = new List<Movie>();
            var query = _context.Movies
                                    .OrderBy(a => a.Id)
                                    .Skip((page - 1) * pageCapacity)
                                    .Take(pageCapacity);
            movies = await query.ToListAsync();
            return movies;
        }
        /* ------------- END QUERYING PAGING MOVIE INTO LIST ----------------------- */    
    }
}
