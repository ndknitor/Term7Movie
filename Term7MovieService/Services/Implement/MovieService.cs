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

        public async Task<MovieHomePageResponse> GetThreeLatestMovieForHomepage()
        {
            MovieHomePageResponse mhpr = new MovieHomePageResponse();
            List<MovieCoverDTO> list = new List<MovieCoverDTO>();
            IMovieRepository movierepo = _unitOfWork.MovieRepository;
            foreach(var item in await movierepo.GetThreeLatestMovie())
            {
                MovieCoverDTO cover = new MovieCoverDTO();
                cover.MovieId = item.Id;
                cover.coverImgURL = item.CoverImageUrl;
                list.Add(cover);
            }
            if(list.Any(a => string.IsNullOrEmpty(a.coverImgURL)))
            {
                mhpr.Message = "One or more movies have null cover img";
                mhpr.movieCoverList = list;
                return mhpr;
            }
            mhpr.Message = "Perfection";
            mhpr.movieCoverList = list;
            return mhpr;
        }
    }
}
