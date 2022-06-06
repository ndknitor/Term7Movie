using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Options;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Request;
using Dapper;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto.Errors;
using Term7MovieCore.Data.Request.CRUDMovie;

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
        public async Task<PagingList<MovieModelDto>> GetAllMovie(ParentFilterRequest request)
        {
            PagingList<MovieModelDto> pagingList;
            int fetch = request.PageSize;
            int offset = (request.Page - 1) * request.PageSize;
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string query =
                    " SELECT Id, Title, ReleaseDate, Duration, RestrictedAge, PosterImageUrl, CoverImageUrl, TrailerUrl, Description, ViewCount, TotalRating " +
                    " FROM Movies " +
                    " WHERE ReleaseDate BETWEEN ( GETDATE() - 30) AND (GETDATE() + 30) " +
                    " ORDER BY ReleaseDate DESC " +
                    " OFFSET @offset ROWS " +
                    " FETCH NEXT @fetch ROWS ONLY ; ";
                string count = " SELECT COUNT(1) FROM Movies ";

                object param = new { offset, fetch };
                
                var multiQ = await con.QueryMultipleAsync(query + count, param);
                IEnumerable<MovieModelDto> list = await multiQ.ReadAsync<MovieModelDto>();
                long total = await multiQ.ReadFirstAsync<long>();

                pagingList = new PagingList<MovieModelDto>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
        }
        public async Task<Movie> GetMovieById(int id)
        {
            Movie movie = await _context.Movies.FindAsync(id);
            return movie;
        }
        public async Task<bool> CreateMovie(IEnumerable<Movie> movie)
        {
            if (!await _context.Database.CanConnectAsync())
                //throw new Exception();
                return false;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Movies.AddRangeAsync(movie);
                await _context.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
                //return false;
            }
        }

        public async Task<bool> UpdateMovie(Movie movie)
        {
            if (!await _context.Database.CanConnectAsync())
                //throw new Exception();
                return false;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return false;
            }
        }


        public async Task<bool> DeleteMovie(Movie movie)
        {
            if (!await _context.Database.CanConnectAsync())
                //throw new Exception();
                return false;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return false;
            }
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
                //throw new Exception();
                return null;
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
                return null;
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
                //.AsNoTracking()
                .Take(8);
            movies = query.ToList();
            return movies;

        }

        public async Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds)
        {
            if (!await _context.Database.CanConnectAsync())
                return null;
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
            if (page <= 0) return null;
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

        /* ------------- START QUERYING MOVIE FOR DETAIL ------------------------ */
        public async Task<IEnumerable<MovieType>> GetCategoryFromSpecificMovieId(int movieId)
        {
            if (!await _context.Database.CanConnectAsync())
                return null;
            List<MovieType> result = new List<MovieType>();
            var query = _context.MovieCategories
                                        .Include(a => a.Category)
                                        .Where(a => a.MovieId == movieId);
                                        //.ToListAsync();
            foreach(var category in query)
            {
                result.Add(new MovieType { CateId = category.CategoryId, CateName = category.Category.Name });
            }
            return result;
        }
        /* ------------- END QUERYING MOVIE FOR DETAIL ----------------------- */

        /* ------------- START CREATING MOVIE ------------------------- */
        public async Task<CreateMovieError> CreateMovieWithCategory(MovieCreateRequest request)
        {
            if (!await _context.Database.CanConnectAsync())
                return null;
            CreateMovieError result = new CreateMovieError();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Movie movie = new Movie();
                movie.Title = request.Title;
                movie.ReleaseDate = request.ReleasedDate;
                movie.Duration = request.Duration;
                movie.RestrictedAge = request.RestrictedAge;
                movie.PosterImageUrl = request.PosterImgURL;
                movie.CoverImageUrl = request.CoverImgURL;
                movie.TrailerUrl = request.TrailerURL;
                movie.Description = request.Description;
                movie.DirectorId = request.DirectorId;
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
                foreach(int cateID in request.CategoryIDs)
                {
                    Category category = await _context.Categories.FindAsync(cateID);
                    if(category == null && result.Status == true)
                    {
                        result.Status = false;
                        continue;
                    }
                    MovieCategory mc = new MovieCategory();
                    mc.MovieId = movie.Id;
                    mc.CategoryId = category.Id;
                    await _context.MovieCategories.AddAsync(mc);
                    await _context.SaveChangesAsync();
                }
                result.MovieId = movie.Id;
                result.Title = movie.Title;
                if (result.Status) result.Message = "Successfully added this film";
                return result;
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        /* ------------- END CREATING MOVIE --------------------------- */

        /* ----------------- START UPDATE MOVIE ---------------------- */
        public async Task<bool> UpdateMovie(MovieUpdateRequest request)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            bool DoesItGood = true;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Movie movie = new Movie();
                movie.Id = request.MovieId;
                movie.Title = request.Title;
                movie.ReleaseDate = request.ReleasedDate;
                movie.Duration = request.Duration;
                movie.RestrictedAge = request.RestrictedAge;
                movie.PosterImageUrl = request.PosterImgURL;
                movie.CoverImageUrl = request.CoverImgURL;
                movie.TrailerUrl = request.TrailerURL;
                movie.Description = request.Description;
                movie.DirectorId = request.DirectorId;
                _context.Update(movie);
                await _context.SaveChangesAsync();
                //dark dark buh buh
                _context.MovieCategories.RemoveRange(
                    _context.MovieCategories.Where(a => a.MovieId == movie.Id));
                await _context.SaveChangesAsync();
                foreach (int cateID in request.CategoryIDs)
                {
                    Category category = await _context.Categories.FindAsync(cateID);
                    if (category == null && DoesItGood == true)
                    {
                        DoesItGood = false;
                        continue;
                    }
                    MovieCategory mc = new MovieCategory();
                    mc.MovieId = movie.Id;
                    mc.CategoryId = category.Id;
                    await _context.MovieCategories.AddAsync(mc);
                    await _context.SaveChangesAsync();
                }
                return DoesItGood;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        /* ----------------- END UPDATE MOVIE ---------------------- */
    }
}
