using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Errors;
using Term7MovieService.Services.Interface;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Response.Movie;

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
            IEnumerable<Movie> rawData = await movieRepository.GetLessThanThreeLosslessLatestMovies();
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
            IEnumerable<Movie> rawData = await movieRepository.GetEightLatestMovies();
            if (!rawData.Any())
                return new MovieHomePageResponse { Message = "DATABASE IS EMPTY" };

            //rawData = rawData.ToList().OrderByDescending(a => a.ReleaseDate).Take(8);
            
            //Start making process
            int[] movieIds = new int[rawData.Count()];
            for(int j = 0; j < rawData.Count(); j++)
            {
                movieIds[j] = rawData.ElementAt(j).Id;
            }
            Dictionary<int, IEnumerable<MovieType>> categories = await movieRepository.GetCategoriesFromMovieList(movieIds);
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
            IEnumerable<Movie> rawData = await movieRepository.GetMoviesFromSpecificPage(request.PageIndex, request.PageSize);
            //checking database connection
            if (rawData == null)
                return new TemptMoviePagingResponse { Message = "Page index is smaller than 1" };
            //checking if there is any data in database
            if (!rawData.Any())
                return new TemptMoviePagingResponse { Message = "Empty Data" };
            //checking input logical
            int maxpage = movieRepository.Count() / 16 + 1;
            if (request.PageIndex > maxpage) //madness shit
                return new TemptMoviePagingResponse { Message = "Page index is more than total page" };

            // ********* End validating or checking shet *********** //
            TemptMoviePagingResponse mlr = new TemptMoviePagingResponse();
            int[] movieIds = new int[rawData.Count()];
            for (int j = 0; j < rawData.Count(); j++)
            {
                movieIds[j] = rawData.ElementAt(j).Id;
            }
            Dictionary<int, IEnumerable<MovieType>> categories = await movieRepository.GetCategoriesFromMovieList(movieIds);
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

        public async Task<MovieDetailResponse> GetMovieDetailFromMovieId(int movieId)
        {
            Movie rawData = await movieRepository.GetMovieById(movieId);
            //checking if there is any data in database
            if (rawData == null)
                return new MovieDetailResponse { Message = "Movie not found" };
            MovieDetailResponse mdr = new MovieDetailResponse();
            MovieDetailDTO dto = new MovieDetailDTO();
            dto.Id = rawData.Id;
            dto.Title = rawData.Title;
            dto.ReleaseDate = rawData.ReleaseDate;
            dto.Duration = rawData.Duration;
            dto.RestrictedAge = rawData.RestrictedAge;
            dto.PosterImageUrl = rawData.PosterImageUrl;
            dto.CoverImageUrl = rawData.CoverImageUrl;
            dto.TrailerUrl = rawData.TrailerUrl;
            dto.Description = rawData.Description;
            dto.ViewCount = rawData.ViewCount;
            dto.TotalRating = rawData.TotalRating;
            dto.DirectorId = rawData.DirectorId;
            mdr.MovieDetail = dto;
            //Sublime text 4 is the best. for this damn situation
            //checking if there is any categories for this movie
            IEnumerable<MovieType> categories = await movieRepository.GetCategoryFromSpecificMovieId(rawData.Id);
            if (!categories.Any())
            {
                mdr.Message = "No category was found for this movie.";
                return mdr;
            }
            dto.movieTypes = categories;
            mdr.Message = "Successful";
            return mdr;
                
        }

        public async Task<ParentResponse> CreateMovieWithoutBusinessLogic(MovieCreateRequest[] requests)
        {
            List<Movie> Movies = new List<Movie>();
            foreach(var request in requests)
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
                Movies.Add(movie);
            }
            ParentResponse father = new ParentResponse();
            try
            {
                bool result = await movieRepository.CreateMovie(Movies);
                if (result)
                {
                    father.Message = "Successful";
                    return father;
                }
                father.Message = "Create Movie Failed";
                return father;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<MovieCreateResponse> CreateMovie(MovieCreateRequest[] requests)
        {
            MovieCreateResponse response = new MovieCreateResponse();
            List<CreateMovieError> ErrorList = new List<CreateMovieError>();
            foreach(var movie in requests)
            {
                try
                {
                    CreateMovieError error = await movieRepository.CreateMovieWithCategory(movie);
                    if (error == null)
                        return null;
                    if (!error.Status)
                        error.Message = "Some of category was failed to add in this film";
                    ErrorList.Add(error);
                }
                catch
                {
                    CreateMovieError error = new CreateMovieError();
                    error.Title = movie.Title;
                    error.Status = false;
                    error.Message = "Failed to create this movie";
                    ErrorList.Add(error);
                    continue;
                }
            }
            response.Reports = ErrorList;
            if (ErrorList.All(a => a.Status == false))
                response.Message = "All movie was failed while adding";
            else if (ErrorList.Any(a => a.Status == false))
                response.Message = "Some movie was failed while adding";
            else response.Message = "Successfully";
            return response;
        }
    }
}
