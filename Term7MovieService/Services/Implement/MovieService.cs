using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieService.Services.Interface;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;

namespace Term7MovieService.Services.Implement
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieRepository movieRepository;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            movieRepository = _unitOfWork.MovieRepository;
        }

        public async Task<MovieListResponse> GetAllMovie(ParentFilterRequest request)
        {
            PagingList<MovieModelDto> movies = await movieRepository.GetAllMovie(request);

            return new MovieListResponse
            {
                Message = "Success",
                Movies = movies
            };
        }

        public async Task<IncomingMovieResponse> GetEightLosslessLatestMovieForHomepage()
        {
            //Handle Error
            IMovieRepository movierepo = _unitOfWork.MovieRepository;
            if (movierepo == null) 
                return new IncomingMovieResponse { Message = "REPOSITORY NULL" };
            IEnumerable<Movie> rawData = await movierepo.GetLessThanThreeLosslessLatestMovies();
            if (!rawData.Any()) 
                return new IncomingMovieResponse { Message = "DATABASE IS EMPTY" };

            //Start making process
            IncomingMovieResponse IMR = new IncomingMovieResponse();
            List<SmallMovieHomePageDTO> list = new List<SmallMovieHomePageDTO>();
            foreach(var item in rawData)
            {
                SmallMovieHomePageDTO smp = new SmallMovieHomePageDTO();
                smp.MovieId = item.Id;
                //cover.CoverImgURL = item.CoverImageUrl;
                smp.PosterImgURL = item.PosterImageUrl;
                list.Add(smp);
            }
            if (list.Count == 0) IMR.Message = "No data for incoming movies :(";
            else if (list.Count < 3) IMR.Message = "Missing incoming movie from database";
            else IMR.Message = "Succesfully";
            IMR.LosslessMovieList = list;
            return IMR;
        }

        public async Task<MovieHomePageResponse> GetEightLatestMovieForHomepage()
        {
            //Handle Error
            IMovieRepository movierepo = _unitOfWork.MovieRepository;
            if (movierepo == null)
                return new MovieHomePageResponse { Message = "REPOSITORY NULL" };
            IEnumerable<Movie> rawData = await movierepo.GetEightLatestMovies();
            if (!rawData.Any())
                return new MovieHomePageResponse { Message = "DATABASE IS EMPTY" };

            //rawData = rawData.ToList().OrderByDescending(a => a.ReleaseDate).Take(8);
            
            //Start making process
            int[] movieIds = new int[rawData.Count()];
            for(int j = 0; j < rawData.Count(); j++)
            {
                movieIds[j] = rawData.ElementAt(j).Id;
            }
            Dictionary<int, IEnumerable<MovieType>> categories = await movierepo.GetCategoriesFromMovieList(movieIds);
            //The code below effect RAM only
            bool DoesItNull = false;
            MovieHomePageResponse mhpr = new MovieHomePageResponse();
            List<MovieDTO> list = new List<MovieDTO>();
            foreach (var item in rawData)
            {
                MovieDTO movie = new MovieDTO();
                movie.MovieId = item.Id;
                movie.CoverImgURL = item.CoverImageUrl;
                movie.PosterImgURL = item.PosterImageUrl;
                movie.Title = item.Title;
                movie.AgeRestrict = item.RestrictedAge;
                movie.Duration = item.Duration;
                DateTime dt = item.ReleaseDate;
                movie.ReleaseDate = dt.ToString("MMM") + " " + dt.ToString("dd") + ", " + dt.ToString("yyyy");
                //movie.Types = categories.GetValueOrDefault(item.Id);
                if (movie.Categories == null || movie.Categories.Count() == 0) DoesItNull = true;
                movie.Categories = categories.GetValueOrDefault(item.Id);
                list.Add(movie);
            }
            if (!DoesItNull)
                mhpr.Message = "Succesfully";
            else mhpr.Message = "Some movie categories is null";
            mhpr.movieList = list;
            return mhpr;
        }

        public async Task<TemptMoviePagingResponse> GetMovieListFollowPage(MovieListPageRequest request)
        {
            IMovieRepository movieRepo = _unitOfWork.MovieRepository;
            //checking singleton went wrong
            if (movieRepo == null)
                return new TemptMoviePagingResponse { Message = "REPOSITORY IS NULL" };
            IEnumerable<Movie> rawData = await movieRepo.GetMoviesFromSpecificPage(request.PageIndex, request.PageSize);
            //checking database connection
            if (rawData == null)
                return new TemptMoviePagingResponse { Message = "Unresponsible database" };
            //checking if there is any data in database
            if (!rawData.Any())
                return new TemptMoviePagingResponse { Message = "Empty Data" };
            //checking input logical
            int maxpage = movieRepo.Count() / 16 + 1;
            if (request.PageIndex > maxpage) //madness shit
                return new TemptMoviePagingResponse { Message = "Sao lại để hacker tràn dô nhà dị cha" };

            // ********* End validating or checking shet *********** //
            TemptMoviePagingResponse mlr = new TemptMoviePagingResponse();
            int[] movieIds = new int[rawData.Count()];
            for (int j = 0; j < rawData.Count(); j++)
            {
                movieIds[j] = rawData.ElementAt(j).Id;
            }
            Dictionary<int, IEnumerable<MovieType>> categories = await movieRepo.GetCategoriesFromMovieList(movieIds);
            //The code below effect RAM only
            bool DoesItNull = false;
            List<MovieDTO> list = new List<MovieDTO>();
            foreach (var item in rawData)
            {
                MovieDTO movie = new MovieDTO();
                movie.MovieId = item.Id;
                movie.CoverImgURL = item.CoverImageUrl;
                movie.PosterImgURL = item.PosterImageUrl;
                movie.Title = item.Title;
                movie.AgeRestrict = item.RestrictedAge;
                movie.Duration = item.Duration;
                DateTime dt = item.ReleaseDate;
                movie.ReleaseDate = dt.ToString("MMM") + " " + dt.ToString("dd") + ", " + dt.ToString("yyyy");
                //movie.Types = categories.GetValueOrDefault(item.Id);
                movie.Categories = categories.GetValueOrDefault(item.Id);
                if (movie.Categories == null || movie.Categories.Count() == 0) DoesItNull = true;
                list.Add(movie);
            }
            if (!DoesItNull)
                mlr.Message = "Succesfully";
            else mlr.Message = "Some movie categories is null";
            mlr.MovieList = list;
            mlr.CurrentPage = request.PageIndex;
            mlr.TotalPages = maxpage;
            return mlr;
        }
    }
}
