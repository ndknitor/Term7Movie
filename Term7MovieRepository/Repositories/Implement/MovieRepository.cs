using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieRepository.Repositories.Interfaces;
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

        public async Task<IEnumerable<Movie>> GetThreeLatestMovie()
        {
            if (await _context.Database.CanConnectAsync())
                throw new Exception("Database Dead");
            List<Movie> movies = new List<Movie>();
            var query = _context.Movies
                .Where(a => a.ReleaseDate < DateTime.Now)
                .OrderByDescending(a => a.ReleaseDate)
                .Select(a => new Movie
                {
                    Id = a.Id,
                    CoverImageUrl = a.CoverImageUrl
                })
                .Take(3);
            movies = query.ToList();
            return movies;
        }

    }
}
