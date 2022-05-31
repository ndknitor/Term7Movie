using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieService.Services.Interface;
using Term7MovieRepository.Repositories.Interfaces;


namespace Term7MovieService.Services.Implement
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieHomePageResponse> GetEightLosslessLatestMovieForHomepage()
        {
            //Handle Error
            IMovieRepository movierepo = _unitOfWork.MovieRepository;
            if (movierepo == null) 
                return new MovieHomePageResponse { Message = "REPOSITORY NULL" };
            IEnumerable<Movie> rawData = await movierepo.GetEightLosslessLatestMovies();
            if (!rawData.Any()) 
                return new MovieHomePageResponse { Message = "DATABASE IS EMPTY" };

            //Start making process
            MovieHomePageResponse mhpr = new MovieHomePageResponse();
            List<MovieHomePageDTO> list = new List<MovieHomePageDTO>();
            foreach(var item in rawData)
            {
                MovieHomePageDTO cover = new MovieHomePageDTO();
                cover.MovieId = item.Id;
                cover.CoverImgURL = item.CoverImageUrl;
                cover.PosterImgURL = item.PosterImageUrl;
                list.Add(cover);
            }
            mhpr.Message = "Succesfully";
            mhpr.movieList = list;
            return mhpr;
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

            //Start making process
            int[] movieIds = new int[rawData.Count()];
            for(int j = 0; j < rawData.Count(); j++)
            {
                movieIds[j] = rawData.ElementAt(j).Id;
            }
            Dictionary<int, Dictionary<int, string>> categories = await movierepo.GetCategoriesFromMovieList(movieIds);
            //The code below effect RAM only
            bool DoesItNull = false;
            MovieHomePageResponse mhpr = new MovieHomePageResponse();
            List<MovieHomePageDTO> list = new List<MovieHomePageDTO>();
            foreach (var item in rawData)
            {
                MovieHomePageDTO movie = new MovieHomePageDTO();
                movie.MovieId = item.Id;
                movie.CoverImgURL = item.CoverImageUrl;
                movie.PosterImgURL = item.PosterImageUrl;
                movie.Title = item.Title;
                movie.AgeRestrict = item.RestrictedAge;
                movie.Duration = item.Duration;
                movie.ReleaseDate = item.ReleaseDate;
                //movie.Types = categories.GetValueOrDefault(item.Id);
                movie.Categories = categories.GetValueOrDefault(item.Id);
                if (movie.Categories == null || movie.Categories.Count == 0) DoesItNull = true;
                list.Add(movie);
            }
            if (!DoesItNull)
                mhpr.Message = "Succesfully";
            else mhpr.Message = "Some movie categories is null";
            mhpr.movieList = list;
            return mhpr;
        }
    }
}
