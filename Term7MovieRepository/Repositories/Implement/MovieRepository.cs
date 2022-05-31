using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Term7MovieRepository.Repositories.Implement
{
    public class MovieRepository : IMovieRepository
    {

        private AppDbContext _context;
        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Movie> GetAllMovie()
        {
            IEnumerable<Movie> list = new List<Movie>();
            return list;
        }
        public Movie GetMovieById(int id)
        {
            Movie movie = null;
            return movie;
        }
        public int CreateMovie(Movie movie)
        {
            int count = 0;
            return count;
        }
        public int UpdateMovie(Movie movie)
        {
            int count = 0;
            return count;
        }
        public int DeleteMovie(int id)
        {
            int count = 0;
            return count;
        }
        public int Count()
        {
            int count = 0;
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

        public async Task<IEnumerable<Movie>> GetEightLosslessLatestMovies()
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception();
                //return null;
            List<Movie> movies = new List<Movie>();
            var query = _context.Movies
                .Where(a => a.ReleaseDate < DateTime.Now 
                            && !string.IsNullOrEmpty(a.CoverImageUrl)
                            && !string.IsNullOrEmpty(a.PosterImageUrl))
                .OrderByDescending(a => a.ReleaseDate)
                .Select(a => new Movie
                {
                    Id = a.Id,
                    CoverImageUrl = a.CoverImageUrl,
                    PosterImageUrl = a.PosterImageUrl
                })
                .Take(8);
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

        public async Task<Dictionary<int, Dictionary<int, string>>> GetCategoriesFromMovieList(int[] MovieIds)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception();
            Dictionary<int, Dictionary<int, string>> result = new Dictionary<int, Dictionary<int, string>>();
            foreach(int movieId in MovieIds)
            {
                var list = _context.MovieCategories
                                                .Include(a => a.Category)
                                                .Where(a => a.MovieId == movieId).ToList();
                Dictionary<int, string> categories = new Dictionary<int, string>();
                foreach(var category in list)
                {
                    categories.Add(category.CategoryId, category.Category.Name);
                }
                result.Add(movieId, categories);
            }


            return result;
        }
        /* ------------- END QUERYING FOR MOVIE SHOW ON HOMEPAGE --------------------- */
    }
}
