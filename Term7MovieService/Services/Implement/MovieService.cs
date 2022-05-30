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

        public async Task<MovieHomePageResponse> GetEightLatestMovieForHomepage()
        {
            //Handle Error
            IMovieRepository movierepo = _unitOfWork.MovieRepository;
            if (movierepo == null) 
                return new MovieHomePageResponse { Message = "REPOSITORY NULL" };
            IEnumerable<Movie> rawData = await movierepo.GetThreeLatestMovie();
            if (!rawData.Any()) 
                return new MovieHomePageResponse { Message = "DATABASE IS EMPTY" };

            //Start making process
            MovieHomePageResponse mhpr = new MovieHomePageResponse();
            List<MovieHomePageDTO> list = new List<MovieHomePageDTO>();
            foreach(var item in rawData)
            {
                MovieHomePageDTO cover = new MovieHomePageDTO();
                cover.MovieId = item.Id;
                cover.coverImgURL = item.CoverImageUrl;
                cover.posterImgURL = item.PosterImageUrl;
                list.Add(cover);
            }
            mhpr.Message = "Succesfully";
            mhpr.movieList = list;
            return mhpr;
        }
    }
}
