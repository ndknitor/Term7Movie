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
using Term7MovieCore.Data.Request.Movie;
using Term7MovieCore.Data.Dto.Movie;
using Newtonsoft.Json;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data;

namespace Term7MovieRepository.Repositories.Implement
{
    public class MovieRepository : IMovieRepository
    {
        private const int Index = -1;
        private AppDbContext _context;
        private readonly ConnectionOption _connectionOption;

        private const string FILTER_BY_TITLE = "Title";
        private const string FILTER_BY_AVAILABLE = "Available";
        private const string FILTER_BY_DISABLED = "Disabled";

        public MovieRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }
        public async Task<PagingList<MovieModelDto>> GetAllMovie(MovieFilterRequest request)
        {
            PagingList<MovieModelDto> pagingList;
            int fetch = request.PageSize;
            int offset = (request.Page - 1) * request.PageSize;
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string query =
                    @" SELECT Id, Title, ReleaseDate, Duration, RestrictedAge, PosterImageUrl, CoverImageUrl, TrailerUrl, Description, ViewCount, TotalRating, Actors, Director, Languages, IsAvailable 
                       FROM Movies 
                       WHERE 1=1  " +

                    GetAdditionalMovieFilter(request, FILTER_BY_TITLE) +
                    GetAdditionalMovieFilter(request, FILTER_BY_AVAILABLE) +
                    GetAdditionalMovieFilter(request, FILTER_BY_DISABLED) +

                    @" ORDER BY ReleaseDate DESC 
                       OFFSET @offset ROWS 
                       FETCH NEXT @fetch ROWS ONLY ; ";

                string count =
                    @" SELECT COUNT(1) 
                       FROM Movies 
                       WHERE 1=1 " +
                    GetAdditionalMovieFilter(request, FILTER_BY_TITLE) +
                    GetAdditionalMovieFilter(request, FILTER_BY_AVAILABLE) +
                    GetAdditionalMovieFilter(request, FILTER_BY_DISABLED) +
                    " ; ";

                string category = 
                    " SELECT mc.MovieId, c.Id, c.Name, c.Color " +
                    " FROM MovieCategories mc JOIN Categories c ON mc.CategoryId = c.Id ";

                object param = new { offset, fetch };
                
                var multiQ = await con.QueryMultipleAsync(query + count + category, param);

                IEnumerable<MovieModelDto> list = await multiQ.ReadAsync<MovieModelDto>();

                long total = await multiQ.ReadFirstAsync<long>();

                IEnumerable<MovieCategory> movieCategories = multiQ.Read<MovieCategory, Category, MovieCategory>((mc, c) =>
                {
                    mc.Category = c;
                    return mc;
                });

                foreach(MovieModelDto m in list)
                {
                    m.Categories = movieCategories.Where(mc => m.Id == mc.MovieId).Select(c => new CategoryDTO {Id = c.Category.Id, Name = c.Category.Name, Color = c.Category.Color });
                }

                pagingList = new PagingList<MovieModelDto>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
        }

        public IEnumerable<MovieModelDto> GetAllMovie()
        {
            IEnumerable<MovieModelDto> list;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string query =
                    " SELECT Id, Title, ReleaseDate, Duration, RestrictedAge, PosterImageUrl, CoverImageUrl, TrailerUrl, Description, ViewCount, TotalRating, Actors, Director, Languages, IsAvailable " +
                    " FROM Movies " +
                    " ORDER BY ReleaseDate DESC ; ";

                string category =
                    " SELECT mc.MovieId, c.Id, c.Name, c.Color " +
                    " FROM MovieCategories mc JOIN Categories c ON mc.CategoryId = c.Id ; ";

                var multiQ = con.QueryMultiple(query + category);

                list = multiQ.Read<MovieModelDto>();

                IEnumerable<MovieCategory> movieCategories = multiQ.Read<MovieCategory, Category, MovieCategory>((mc, c) =>
                {
                    mc.Category = c;
                    return mc;
                });

                foreach (MovieModelDto m in list)
                {
                    m.Categories = movieCategories.Where(mc => m.Id == mc.MovieId).Select(c => new CategoryDTO { Id = c.Category.Id, Name = c.Category.Name, Color = c.Category.Color });
                }
            }

            return list;
        }

        public async Task<MovieModelDto> GetMovieByIdAsync(int id)
        {
            MovieModelDto movie = null;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string query =
                    " SELECT Id, Title, ReleaseDate, Duration, RestrictedAge, PosterImageUrl, CoverImageUrl, TrailerUrl, Description, ViewCount, TotalRating, Actors, Director, Languages, IsAvailable " +
                    " FROM Movies " +
                    " WHERE Id = @id ";

                string category =
                    " SELECT mc.MovieId, c.Id, c.Name, c.Color " +
                    " FROM MovieCategories mc JOIN Categories c ON mc.CategoryId = c.Id " +
                    " WHERE mc.MovieId = @id  ; ";

                object param = new { id };

                var multiQ = con.QueryMultiple(query + category, param);

                movie = await multiQ.ReadFirstOrDefaultAsync<MovieModelDto>();

                if (movie != null)
                {
                    IEnumerable<MovieCategory> movieCategories = multiQ.Read<MovieCategory, Category, MovieCategory>((mc, c) =>
                    {
                        mc.Category = c;
                        return mc;
                    });

                    movie.Categories = movieCategories.Where(mc => id == mc.MovieId).Select(c => new CategoryDTO { Id = c.Category.Id, Name = c.Category.Name, Color = c.Category.Color });
                }
            }

            return movie;
        }

        private string GetAdditionalMovieFilter(MovieFilterRequest request, string filter)
        {
            string sql = "";

            switch(filter)
            {
                case FILTER_BY_TITLE:
                    if (!string.IsNullOrEmpty(request.SearchKey))
                    {
                        sql = " AND Title LIKE CONCAT('%', @SearchKey, '%') ";
                    }
                    break;
                case FILTER_BY_AVAILABLE:
                    if (request.IsAvailableOnly)
                    {
                        sql = " AND IsAvailable = 1 ";
                    }
                    break;
                case FILTER_BY_DISABLED:
                    if (!request.IsAvailableOnly && request.IsDisabledOnly)
                    {
                        sql = " AND IsAvailable = 0 ";
                    }
                    break;
            }
            return sql;
        }

        public IEnumerable<Movie> MovieEntityToList()
        {
            //if (!_context.Database.CanConnect())
            //    return null;
            var query = _context.Movies
                                    .Include(a => a.MovieCategories)
                                    .ThenInclude(a => a.Category)
                                    .ToList();
            return query;
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


        public async Task DeleteMovie(int movieId)
        {
            Movie movie = await _context.Movies
                                        .Include(a => a.MovieShowtimes)
                                        .SingleOrDefaultAsync(x => x.Id == movieId);
            //checking
            if (movie == null) throw new DbNotFoundException();

            if(movie.MovieShowtimes.Where(x => x.StartTime > DateTime.UtcNow).Any())
                throw new DbBusinessLogicException("Cannot disable this movie because it's already been sold.");

            movie.IsAvailable = false;
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

        public async Task<IEnumerable<SmallMovieHomePageDTO>> GetLessThanThreeLosslessLatestMovies()
        {
            if (!await _context.Database.CanConnectAsync())    
                throw new DbOperationException(Constants.DATABASE_UNAVAILABLE_MESSAGE);
            List<SmallMovieHomePageDTO> movies = new List<SmallMovieHomePageDTO>();
            var query = _context.Movies
                .Where(a => a.ReleaseDate > DateTime.Now
                            && a.ReleaseDate < DateTime.Now.AddMonths(1)
                            && !string.IsNullOrEmpty(a.CoverImageUrl)
                            && !string.IsNullOrEmpty(a.PosterImageUrl))
                .OrderByDescending(a => a.ReleaseDate)
                .Select(a => new SmallMovieHomePageDTO
                {
                    MovieId = a.Id,
                    CoverImgURL = a.CoverImageUrl,
                    PosterImgURL = a.PosterImageUrl,
                    Title = a.Title
                })
                .Take(3);
            movies = query.ToList();
            return movies;
        }

        public async Task<Tuple<IEnumerable<Movie>, long>> GetLatestMovies(ParentFilterRequest request)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException(Constants.DATABASE_UNAVAILABLE_MESSAGE);
            //return null;
            IEnumerable<Movie> movies = new List<Movie>();
            var query = _context.Movies
                .Where(a => a.IsAvailable
                            && a.ReleaseDate > DateTime.UtcNow.AddMonths(-1)
                            && a.Title.ToLower().Contains(request.SearchKey.ToLower()))
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
                });
            movies = query.ToList();
            long totalrecord = movies.LongCount();
            movies = movies.Skip(request.PageSize * (request.Page - 1))
                .Take(request.PageSize);
            return Tuple.Create(movies, totalrecord);

        }

        public async Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds)
        {
            if (!await _context.Database.CanConnectAsync())
                return null; //if you can get the movie just skip
            Dictionary<int, IEnumerable<MovieType>> result = new Dictionary<int, IEnumerable<MovieType>>();
            foreach(int movieId in MovieIds)
            {
                //var list = _context.MovieCategories
                //                                .Include(a => a.Category)
                //                                .Where(a => a.MovieId == movieId).ToList();
                //List<MovieType> categories = new List<MovieType>();
                //foreach(var category in list)
                //{
                //    MovieType mt = new MovieType();
                //    mt.CateId = category.CategoryId;
                //    mt.CateName = category.Category.Name;
                //    categories.Add(mt);
                //}
                //result.Add(movieId, categories);
                var query = _context.MovieCategories
                                        .Include(a => a.Category)
                                        .Where(a => a.MovieId == movieId)
                                        .Select(xx => new MovieType
                                        {
                                            CateColor = xx.Category.Color,
                                            CateId = xx.CategoryId,
                                            CateName = xx.Category.Name
                                        });
                result.Add(movieId, query);
            }


            return result;
        }

        public async Task<IEnumerable<Movie>> GetRemainInformationForHomePage(int[] MovieIds)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException(Constants.DATABASE_UNAVAILABLE_MESSAGE);
            List <Movie> result = new List<Movie>();
            for(int i = 0; i < MovieIds.Length; i++)
            {
                Movie movie = await _context.Movies.FirstOrDefaultAsync(a => a.Id == MovieIds[i]);
                result.Add(movie); //handle null later am too tired to think about it
            }
            return result;
        }
        /* ------------- END QUERYING FOR MOVIE SHOW ON HOMEPAGE --------------------- */
        
        /* ------------- START QUERYING PAGING MOVIE INTO LIST ----------------------------- */
        public async Task<IEnumerable<Movie>> GetMoviesFromSpecificPage(int page, int pageCapacity, string searchtitle)
        {
            if (page <= 0) throw new Exception("PAGESMALLER");
            if (!await _context.Database.CanConnectAsync())
                return null;
            List<Movie> movies = new List<Movie>();
            var query = _context.Movies
                                    .Where(xxx => xxx.Title.Contains(searchtitle))
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
                movie.Actors = JsonConvert.SerializeObject(request.Actors.Distinct());
                movie.Director = request.Director;
                movie.Languages = JsonConvert.SerializeObject(request.Languages.Distinct());
                movie.IsAvailable = true;
                //movie.DirectorId = request.DirectorId;
                movie.ExternalId = null;
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
                foreach (int cateID in request.CategoryIDs.Distinct()) //thank for reminding me to validate duplicated cateid
                                                                       //but am too lazy for that :D
                {
                    Category category = await _context.Categories.FindAsync(cateID);
                    if (category == null)
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
                await transaction.CommitAsync();
                //result.MovieId = movie.Id;
                result.Title = movie.Title;
                if (result.Status) result.Message = "Successfully added this film";
                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        /* ------------- END CREATING MOVIE --------------------------- */

        /* ----------------- START UPDATE MOVIE ---------------------- */
        public Task<bool> UpdateMovie(MovieUpdateRequest request)
        {
            throw new NotImplementedException();
            //if (!await _context.Database.CanConnectAsync())
            //    throw new DbOperationException(Constants.DATABASE_UNAVAILABLE_MESSAGE);

            //Movie movie = await _context.Movies.FindAsync(request.MovieId);
            //if (movie == null)
            //    throw new DbBusinessLogicException("Can't find movie id: " + request.MovieId);
            //bool DoesItGood = true;
            //using var transaction = await _context.Database.BeginTransactionAsync();
            //try
            //{
            //    //Movie movie = _context.Movies.SingleOrDefault(a => a.Id == request.MovieId);
            //    movie.Title = request.Title;
            //    movie.ReleaseDate = request.ReleasedDate;
            //    movie.Duration = request.Duration;
            //    movie.RestrictedAge = request.RestrictedAge;
            //    movie.PosterImageUrl = request.PosterImgURL;
            //    movie.CoverImageUrl = request.CoverImgURL;
            //    movie.TrailerUrl = request.TrailerURL;
            //    movie.Description = request.Description;
            //    movie.Actors = JsonConvert.SerializeObject(request.Actors.Distinct());
            //    movie.Director = request.Director;
            //    movie.IsAvailable = request.isAvailable;
            //    movie.Languages = JsonConvert.SerializeObject(request.Language.Distinct());
            //    //movie.DirectorId = request.DirectorId;
            //    movie.ExternalId = null;
            //    _context.Update(movie);
            //    await _context.SaveChangesAsync();
            //    //dark dark buh buh
            //    _context.MovieCategories.RemoveRange(
            //        _context.MovieCategories.Where(a => a.MovieId == movie.Id));
            //    await _context.SaveChangesAsync();
            //    foreach (int cateID in request.CategoryIDs)
            //    {
            //        Category category = await _context.Categories.FindAsync(cateID);
            //        if (category == null && DoesItGood == true)
            //        {
            //            DoesItGood = false;
            //            continue;
            //        }
            //        MovieCategory mc = new MovieCategory();
            //        mc.MovieId = movie.Id;
            //        mc.CategoryId = category.Id;
            //        await _context.MovieCategories.AddAsync(mc);
            //        await _context.SaveChangesAsync();
            //    }
            //    await transaction.CommitAsync();
            //    return DoesItGood;
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();
            //    throw new Exception(ex.Message);
            //}
        }

        public async Task<ParentResponse> RestoreMovie(int movieid)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException(Constants.DATABASE_UNAVAILABLE_MESSAGE);
            Movie movie = await _context.Movies.FindAsync(movieid);
            if (movie == null)
                throw new DbBusinessLogicException("Movie id: " + movieid + " not found");
            movie.IsAvailable = true;
            await _context.SaveChangesAsync();
            return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
        }
        /* ----------------- END UPDATE MOVIE ---------------------- */

        /* ----------------- START GET MOVIE TITLE --------------- */
        public async Task<IEnumerable<Movie>> GetMoviesTitle()
        {// does thing making sense now?
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException(Constants.DATABASE_UNAVAILABLE_MESSAGE);
            List<Movie> result = new List<Movie>();
            var query = _context.Movies
                        .Where(a => a.IsAvailable
                                    && a.ReleaseDate >= DateTime.UtcNow.AddMonths(-1))
                        .Select(xxx => new Movie
                        {
                            Id = xxx.Id,
                            Title = xxx.Title,
                        });
            result = await query.ToListAsync();
            return result;
        }


        /* ------------------ END GET MOVIE TITLE --------------- */

        /* ----------------- START PRIVATE FUNCTION ------------------ */
        //private async Task<int> GetExternalId() //Don't use it
        //{
        //    //it gonna cost medium performance for this but i have no choice
        //    for(int i = 1; i <= int.MaxValue; i++)
        //    {
        //        var movie = _context.Movies.FirstOrDefault(a => a.Id == i);
        //        if (movie == null) return i;
        //    }
        //    return Index;
        //}

        /* ----------------- END PRIVATE FUNCTION --------------------- */

    }
}
