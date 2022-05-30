using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
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

        public async Task<IEnumerable<MovieHomePageResponse>> GetThreeLatestMovieForHomepage()
        {
            List<MovieHomePageResponse> result = new List<MovieHomePageResponse>();
            IMovieRepository movierepo = _unitOfWork.MovieRepository;
            foreach(var item in await movierepo.GetThreeLatestMovie())
            {
                MovieHomePageResponse res = new MovieHomePageResponse();
                res.Message = "Nothing wrong hihi";
                res.movieID = item.Id;
                res.coverImgURL = item.CoverImageUrl;
                result.Add(res);
            }
            return result;
        }
    }
}
