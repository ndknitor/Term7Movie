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
using Term7MovieCore.Data.Request.Movie;
using Term7MovieCore.Data.Dto.Movie;
using Newtonsoft.Json;
using Term7MovieCore.Data;
using Term7MovieRepository.Cache.Interface;
using Term7MovieCore.Data.Utility;
using Term7MovieCore.Data.Dto.Theater;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Extensions;

namespace Term7MovieService.Services.Implement
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieRepository movieRepository;
        private readonly ICacheProvider _cacheProvider;
        private readonly IShowtimeRepository showRepository;
        private readonly ITicketRepository tiktokRepository;

        public MovieService(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
        {
            _unitOfWork = unitOfWork;
            movieRepository = _unitOfWork.MovieRepository;
            _cacheProvider = cacheProvider;
            showRepository = _unitOfWork.ShowtimeRepository;
            tiktokRepository = _unitOfWork.TicketRepository;
        }

        public async Task<MovieListResponse> GetAllMovie(MovieFilterRequest request)
        {
            PagingList<MovieModelDto> movies;

            IEnumerable<MovieModelDto> list = await _cacheProvider.GetValueAsync<IEnumerable<MovieModelDto>>(Constants.REDIS_KEY_MOVIE);

            if (list == null)
            {
                movies = await movieRepository.GetAllMovie(request);
            } else
            {
                if (!string.IsNullOrEmpty(request.SearchKey))
                {
                    request.SearchKey = request.SearchKey.ToLower();
                    list = list.Where(m => m.Title.ToLower().Contains(request.SearchKey));
                }

                if (request.IsAvailableOnly)
                {
                    list = list.Where(m => m.IsAvailable);
                } else
                {
                    if (request.IsDisabledOnly) list = list.Where(m => !m.IsAvailable);
                }
                
                var pagingList = list.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize);

                movies = new PagingList<MovieModelDto>(request.PageSize, request.Page, pagingList, list.Count());
            }

            foreach(var m in movies.Results)
            {
                m.BeautifyActors = m.Actors.ToObject<string[]>();
                m.LanguageList = m.Languages.ToObject<string[]>();
            }

            return new MovieListResponse
            {
                Message = "Success",
                Movies = movies
            };
        }

        public async Task<ParentResultResponse> GetMovieById(int id)
        {
            IEnumerable<MovieModelDto> list = await _cacheProvider.GetValueAsync<IEnumerable<MovieModelDto>>(Constants.REDIS_KEY_MOVIE);
            
            MovieModelDto movie = null;

            if (list != null)
            {
                movie = list.FirstOrDefault(m => m.Id == id);
            } else
            {
                movie = await movieRepository.GetMovieByIdAsync(id);
            }

            if (movie == null) throw new DbNotFoundException();

            //movie.BeautifyActors = JsonConvert.DeserializeObject<string[]>("test500error");
            movie.BeautifyActors = movie.Actors.ToObject<string[]>();
            movie.LanguageList = movie.Languages.ToObject<string[]>();

            return new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = movie
            };
        }

        public async Task<IncomingMovieResponse> GetThreeLosslessIncomingMovieForHomepage()
        {
            //Handle Error
            IEnumerable<SmallMovieHomePageDTO> rawData = await movieRepository.GetLessThanThreeLosslessIncomingMovies();
            if (!rawData.Any()) 
                return new IncomingMovieResponse { Message = "Empty database" };

            //Start making process
            return new IncomingMovieResponse
            {
                LosslessMovieList = rawData,
                Message = Constants.MESSAGE_SUCCESS,
            };
        }

        public async Task<MovieLatestResponse> GetLatestMovieForNowShowingPage(ParentFilterRequest request)
        {
            IEnumerable<MovieModelDto> cachedlist = await _cacheProvider.GetValueAsync<IEnumerable<MovieModelDto>>(Constants.REDIS_KEY_MOVIE);
            if(cachedlist == null)
            {
                var rawData = await movieRepository.GetLatestMovies(request);

                var MovieLatestList = rawData.Item1;

                //Start making process
                int[] movieIds = new int[MovieLatestList.Count()];
                for (int j = 0; j < MovieLatestList.Count(); j++)
                {
                    movieIds[j] = MovieLatestList.ElementAt(j).Id;
                }
                Dictionary<int, IEnumerable<MovieType>> categories = await movieRepository.GetCategoriesFromMovieList(movieIds);
                //The code below effect RAM only
                bool DoesItNull = false;
                MovieLatestResponse mhpr = new MovieLatestResponse();
                List<MovieDTO> list = new List<MovieDTO>();
                foreach (var item in MovieLatestList)
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
                    mhpr.Message = Constants.MESSAGE_SUCCESS;
                else mhpr.Message = "Some movie categories is null";
                mhpr.movieList = new PagingList<MovieDTO>
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Results = list,
                    Total = rawData.Item2
                };
                return mhpr;
            }
            else
            {
                //we don't even touch showtime
                var LatestMoviesList = cachedlist.Where(x => x.IsAvailable
                                        && x.ReleaseDate <= DateTime.UtcNow.AddMonths(1)
                                        && x.Title.ToLower().Contains(request.SearchKey.ToLower()))
                                        .Select(x => new MovieDTO
                                        {
                                            MovieId = x.Id,
                                            CoverImgURL = x.CoverImageUrl,
                                            PosterImgURL = x.PosterImageUrl,
                                            Title = x.Title,
                                            ReleaseDate = x.ReleaseDate.Value.ToString("MMM dd, yyyy"),
                                            Duration = x.Duration,
                                            AgeRestrict = x.RestrictedAge,
                                            Categories = x.Categories.Select(xx => new MovieType
                                            {
                                                CateColor = xx.Color,
                                                CateId = xx.Id,
                                                CateName = xx.Name
                                            }),
                                        });
                //current paging is follow searchkey
                long totalrecord = LatestMoviesList.LongCount();
                LatestMoviesList = LatestMoviesList.Skip(request.PageSize * (request.Page - 1))
                                        .Take(request.PageSize);
                PagingList<MovieDTO> result = new PagingList<MovieDTO>()
                {
                    Results = LatestMoviesList,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = totalrecord
                };
                return new MovieLatestResponse
                {
                    Message = Constants.MESSAGE_SUCCESS,
                    movieList = result
                };
            }
        }

        public async Task<MoviePagingResponse> GetMovieListFollowPage(MovieListPageRequest request)
        {
            IEnumerable<Movie> rawData = new List<Movie>();
            try
            {
                rawData = await movieRepository.GetMoviesFromSpecificPage(request.PageIndex, request.PageSize, request.TitleSearch);
            }
            catch (Exception ex)
            {
                if (ex.Message == "PAGESMALLER")
                    return new MoviePagingResponse { Message = "Page index is smaller than 1" };
            }
            //checking database connection
            if (rawData == null)
                return new MoviePagingResponse { Message = "Cant access database at the moment" };
            //checking if there is any data in database
            if (!rawData.Any())
                return new MoviePagingResponse { Message = "Empty Data" };
            //checking input logical
            int maxpage = movieRepository.Count() / request.PageSize + 1;
            if (request.PageIndex > maxpage) //madness shit
                return new MoviePagingResponse { Message = "Page index is more than total page" };

            // ********* End validating or checking shet *********** //
            MoviePagingResponse mlr = new MoviePagingResponse();
            int[] movieIds = new int[rawData.Count()];
            for (int j = 0; j < rawData.Count(); j++)
            {
                movieIds[j] = rawData.ElementAt(j).Id;
            }
            Dictionary<int, IEnumerable<MovieType>> categories = await movieRepository.GetCategoriesFromMovieList(movieIds);
            //The code below effect RAM only
            bool DoesItNull = false;
            List<MovieDTO> loserlist = new List<MovieDTO>();
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
                loserlist.Add(movie);
            }
            if (!DoesItNull)
                mlr.Message = Constants.MESSAGE_SUCCESS;
            else mlr.Message = "Some movie categories is null";
            mlr.MovieList = loserlist;
            mlr.CurrentPage = request.PageIndex;
            mlr.TotalPages = maxpage;
            return mlr;



            //IEnumerable<Movie> list = await _cacheProvider.GetValueAsync<IEnumerable<Movie>>(Constants.REDIS_KEY_MOVIE);
            //mừng hụt -.-
            //if (list == null)
            //{//ko vào redis dc thì vào database
            //    IEnumerable<Movie> rawData = new List<Movie>();
            //    try
            //    {
            //        rawData = await movieRepository.GetMoviesFromSpecificPage(request.PageIndex, request.PageSize, request.TitleSearch);
            //    }
            //    catch (Exception ex)
            //    {
            //        if (ex.Message == "PAGESMALLER")
            //            return new MoviePagingResponse { Message = "Page index is smaller than 1" };
            //    }
            //    //checking database connection
            //    if (rawData == null)
            //        return new MoviePagingResponse { Message = "Cant access database at the moment" };
            //    //checking if there is any data in database
            //    if (!rawData.Any())
            //        return new MoviePagingResponse { Message = "Empty Data" };
            //    //checking input logical
            //    int maxpage = movieRepository.Count() / request.PageSize + 1;
            //    if (request.PageIndex > maxpage) //madness shit
            //        return new MoviePagingResponse { Message = "Page index is more than total page" };

            //    // ********* End validating or checking shet *********** //
            //    MoviePagingResponse mlr = new MoviePagingResponse();
            //    int[] movieIds = new int[rawData.Count()];
            //    for (int j = 0; j < rawData.Count(); j++)
            //    {
            //        movieIds[j] = rawData.ElementAt(j).Id;
            //    }
            //    Dictionary<int, IEnumerable<MovieType>> categories = await movieRepository.GetCategoriesFromMovieList(movieIds);
            //    //The code below effect RAM only
            //    bool DoesItNull = false;
            //    List<MovieDTO> loserlist = new List<MovieDTO>();
            //    foreach (var item in rawData)
            //    {
            //        MovieDTO movie = new MovieDTO();
            //        movie.MovieId = item.Id;
            //        movie.CoverImgURL = item.CoverImageUrl;
            //        movie.PosterImgURL = item.PosterImageUrl;
            //        movie.Title = item.Title;
            //        movie.AgeRestrict = item.RestrictedAge;
            //        movie.Duration = item.Duration;
            //        DateTime dt = item.ReleaseDate;
            //        movie.ReleaseDate = dt.ToString("MMM") + " " + dt.ToString("dd") + ", " + dt.ToString("yyyy");
            //        //movie.Types = categories.GetValueOrDefault(item.Id);
            //        movie.Categories = categories.GetValueOrDefault(item.Id);
            //        if (movie.Categories == null || movie.Categories.Count() == 0) DoesItNull = true;
            //        loserlist.Add(movie);
            //    }
            //    if (!DoesItNull)
            //        mlr.Message = Constants.MESSAGE_SUCCESS;
            //    else mlr.Message = "Some movie categories is null";
            //    mlr.MovieList = loserlist;
            //    mlr.CurrentPage = request.PageIndex;
            //    mlr.TotalPages = maxpage;
            //    return mlr;
            //}
            //else
            //{
            //    var pagingList = list.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize);
            //    List<MovieDTO> lazylist = new List<MovieDTO>();
            //    //I have no time for optimization. But if it go slow I will reconsidering :v RAM problem
            //    foreach(var item in pagingList)
            //    {
            //        MovieDTO movie = new MovieDTO();
            //        movie.MovieId = item.Id;
            //        movie.CoverImgURL = item.CoverImageUrl;
            //        movie.PosterImgURL = item.PosterImageUrl;
            //        movie.Title = item.Title;
            //        movie.AgeRestrict = item.RestrictedAge;
            //        movie.Duration = item.Duration;
            //        DateTime dt = item.ReleaseDate;
            //        movie.ReleaseDate = dt.ToString("MMM") + " " + dt.ToString("dd") + ", " + dt.ToString("yyyy");
            //        List<MovieType> GodDamn = new List<MovieType>();
            //        foreach(var cate in item.MovieCategories)
            //        {
            //            GodDamn.Add(new MovieType
            //            {
            //                CateColor = cate.Category.Color,
            //                CateId = cate.Category.Id,
            //                CateName = cate.Category.Name
            //            });
            //        }
            //        movie.Categories = GodDamn;
            //        lazylist.Add(movie);
            //    }
            //    return new MoviePagingResponse
            //    {
            //        Message = Constants.MESSAGE_SUCCESS,
            //        MovieList = lazylist,
            //        CurrentPage = request.PageIndex,
            //        PageSize = request.PageSize,
            //        TotalPages = list.Count() / request.PageSize + 1
            //    };
            //}

            
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
            dto.ReleaseDate = rawData.ReleaseDate.ToString("MMM") 
                + " " + rawData.ReleaseDate.ToString("dd") + ", " + rawData.ReleaseDate.ToString("yyyy");
            dto.Duration = rawData.Duration;
            dto.RestrictedAge = rawData.RestrictedAge;
            dto.PosterImageUrl = rawData.PosterImageUrl;
            dto.CoverImageUrl = rawData.CoverImageUrl;
            dto.TrailerUrl = rawData.TrailerUrl;
            dto.Description = rawData.Description;
            dto.ViewCount = rawData.ViewCount;
            dto.TotalRating = rawData.TotalRating;
            dto.Actors = string.IsNullOrEmpty(rawData.Actors) ? null :
                JsonConvert.DeserializeObject<string[]>(rawData.Actors);
            dto.Languages = string.IsNullOrEmpty(rawData.Languages) ? null :
                JsonConvert.DeserializeObject<string[]>(rawData.Languages);
            dto.Director = rawData.Director;
            //dto.DirectorId = rawData.DirectorId;
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
            mdr.Message = Constants.MESSAGE_SUCCESS;
            return mdr;
                
        }

        /* --------------------- START CUD MOVIE ------------- */

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
                //movie.DirectorId = request.DirectorId;
                Movies.Add(movie);
            }
            ParentResponse father = new ParentResponse();
            try
            {
                bool result = await movieRepository.CreateMovie(Movies);
                if (result)
                {
                    father.Message = Constants.MESSAGE_SUCCESS;
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
            foreach (var movie in requests)
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
                catch (Exception ex)
                {
                    CreateMovieError error = new CreateMovieError();
                    error.Title = movie.Title;
                    error.Status = false;
                    error.Message = ex.Message;
                    ErrorList.Add(error);
                    continue;
                }
            }
            response.Reports = ErrorList;
            if (ErrorList.All(a => a.Status == false))
                response.Message = "All movie was failed while adding";
            else if (ErrorList.Any(a => a.Status == false))
                response.Message = "Some movie was failed while adding";
            else response.Message = Constants.MESSAGE_SUCCESS;
            return response;
        }

        //public async Task<ParentResponse> UpdateMovie(MovieUpdateRequest request)
        //{
        //    ParentResponse response = new ParentResponse();
        //    try
        //    {
        //        if (await movieRepository.UpdateMovie(request))
        //            response.Message = Constants.MESSAGE_SUCCESS;
        //        else response.Message = "Some of category was failed while updating movie";
        //    }
        //    catch (Exception ex)
        //    {
        //        //if (ex.Message == Constants.DATABASE_UNAVAILABLE_MESSAGE)
        //        //    response.Message = "Data storage unaccessible.";
        //        //else response.Message = "Failed to update this movie";
        //        if (ex.Message == "MOVIENOTFOUND")
        //            response.Message = "Movie ID not found, the ID was " + request.MovieId;
        //        else
        //            throw new Exception(ex.Message);
        //    }
        //    return response;
        //}

        public async Task<ParentResponse> ReviveAMovie(int ThatMovieid) => await movieRepository.RestoreMovie(ThatMovieid);


        public async Task<ParentResponse> DeleteMovie(int movieid)
        {
            await movieRepository.DeleteMovie(movieid);

            if (await _unitOfWork.CompleteAsync())
            {
                return new ParentResponse
                {
                    Message = Constants.MESSAGE_SUCCESS
                };
            }

            throw new DbNotFoundException();
        }

        /* --------------------- END CUD MOVIE ------------- */

        /* -------------------- START GET TITLE MOVIE ------------------ */
        public async Task<MovieTitleResponse> GetMovieTitle()
        {
            var movielist = await _cacheProvider.GetValueAsync<IEnumerable<MovieModelDto>>(Constants.REDIS_KEY_MOVIE);
            if(movielist == null)
            {
                var rawData = await movieRepository.GetMoviesTitle();
                if (rawData == null) return new MovieTitleResponse { Message = "Cant access database" };
                List<MovieTitleDTO> list = new List<MovieTitleDTO>();
                foreach (var item in rawData)
                {
                    MovieTitleDTO dto = new MovieTitleDTO()
                    {
                        MovieId = item.Id,
                        Title = item.Title,
                    };
                    list.Add(dto);
                }
                return new MovieTitleResponse
                {
                    Message = Constants.MESSAGE_SUCCESS,
                    MovieTitles = list
                };
            }
            else
            {
                var titles = movielist.Where(xxx => xxx.IsAvailable
                                                && xxx.ReleaseDate?.Date <= DateTime.UtcNow.Date)
                                      .Select(xx => new MovieTitleDTO
                                      {
                                          MovieId = xx.Id,
                                          Title = xx.Title,
                                      }).ToList();
                return new MovieTitleResponse
                {
                    Message = Constants.MESSAGE_SUCCESS,
                    MovieTitles = titles
                };
            }
            
        }
        /* ---------------------- END GET TITLE MOVIE --------------- */
        
        /* --------------------- START DEALING WITH HOMEPAGE MOBILE APP --------------- */
        //public async Task<MovieHomePageResponse> GetMovieForHomePage(MovieHomePageRequest request)
        //{
        //    MovieHomePageResponse response = new MovieHomePageResponse();
        //    try
        //    {
        //        var showtimes = await showRepository.GetRecentlyShowTimeForMovieHomepage();
        //        //Đổ data ra sao cho có 3 thứ thời gian, vị trí, giá
        //        List<MovieSorter> SorterList = new List<MovieSorter>();
        //        foreach(var item in showtimes)
        //        {
        //            var Tobiroppo = await tiktokRepository.GetMinAndMaxPriceFromShowTimeId(item.ShowTimeId);
        //            Coordinate UserLocation = new Coordinate 
        //            { Latitude = request.Latitude, 
        //                Longitude = request.Longtitude };
        //            SorterList.Add(new MovieSorter
        //            {
        //                current = item,
        //                MaxPrice = Tobiroppo.Item2,
        //                MinPrice = Tobiroppo.Item1,
        //                Distance = CalculateDistanceByHaversine(item.Location, UserLocation),
        //            });
        //        }
        //        //convert thời gian ra int để so sánh ưu tiên
        //        // số càng cao thời gian càng xa ko ưu tiên :D
        //        int TimeRecommender = 1;
        //        foreach (var item in SorterList.OrderBy(a => a.current.StartTime))
        //        {
        //            item.TimeRecommendPoint = TimeRecommender;
        //            TimeRecommender++; 
        //        }
        //        decimal MaxMinPriceInSorterList = SorterList.Max(a => a.MinPrice);
        //        int Max100Percent = TimeRecommender--; //SorterList.Max(a => a.TimeRecommendPoint);
        //        double MaxDistance = SorterList.Max(a => a.Distance);
        //        //convert mọi thứ về số int 100% để dễ ưu tiên theo % 0.5 0.3 0.2 hehe
        //        for(int j = 0; j < SorterList.Count; j++)
        //        {
        //            //2 biến double và decimal đang carry % thôi nhé. WAKE UP
        //            double dummy = SorterList[j].Distance * 100 / MaxDistance;
        //            SorterList[j].DistancePoint = dummy * Max100Percent / 100;
        //            double idontcare = (double)SorterList[j].MinPrice * (double)100 / (double)MaxMinPriceInSorterList;
        //            SorterList[j].PricePoint = idontcare * (double)Max100Percent / (double)100;
        //            //để tránh bug thì tôi cast hết double thế này :v
        //            SorterList[j].RecommendPoint = SorterList[j].TimeRecommendPoint * 0.5
        //                + SorterList[j].PricePoint * 0.2 + SorterList[j].DistancePoint * 0.3;
        //        }
        //        //final sọt tôi mệt mong RAM ko cháy vì những dòng này
        //        SorterList = SorterList.OrderByDescending(a => a.RecommendPoint).Take(3).ToList();
        //        IEnumerable<Movie> rawData = await
        //            movieRepository.GetRemainInformationForHomePage(SorterList.Select(a => a.current.MovieId).ToArray());
        //        //now query some data for the remain process
        //        List<MovieHomePageDTO> result = new List<MovieHomePageDTO>();
        //        int linear = 1;
        //        foreach(var item in SorterList)
        //        {
        //            result.Add(new MovieHomePageDTO
        //            {
        //                Title = rawData.Single(a => a.Id == item.current.MovieId).Title,
        //                PosterImgURL = rawData.Single(a => a.Id == item.current.MovieId).PosterImageUrl,
        //                MaxPrice = item.MaxPrice,
        //                MovieId = item.current.MovieId,
        //                MinPrice = item.MinPrice,
        //                RecommendPoint = 1,
        //                TheaterId = item.current.TheaterId,
        //            });
        //            linear++;
        //        }
        //        response.MovieHomePages = result;
        //        response.Message = Constants.MESSAGE_SUCCESS;
        //        return response;

        //    }
        //    catch(Exception ex)
        //    {
        //        if (ex.Message == Constants.DATABASE_UNAVAILABLE_MESSAGE)
        //            return new MovieHomePageResponse { Message = "Can't access database" };
        //        if (ex.Message == "NOT ENOUGH MANA")
        //        {
        //            var rawData = await movieRepository.GetLessThanThreeLosslessLatestMovies();
        //            List<MovieHomePageDTO> result = new List<MovieHomePageDTO>();
        //            foreach(var item in rawData)
        //            {
        //                result.Add(new MovieHomePageDTO
        //                {
        //                    MovieId = item.Id,
        //                    PosterImgURL = item.PosterImageUrl,
        //                    Title = item.Title,
        //                });
        //            }
        //            response.MovieHomePages = result;
        //            response.Message = "Not enough data to show suggestion movie with showtime.";
        //            return response;
        //        }
        //        throw new Exception(ex.Message);
        //    }
        //}
        public async Task<MovieHomePageResponse> GetMovieRecommendationForHomePage(MovieHomePageRequest request)
        {
            MovieHomePageResponse response = new MovieHomePageResponse();
            try
            {
                var rawdata = await showRepository.GetRecentlyShowTimeWithMinutesRemain(
                    new Coordinate { Latitude = request.Latitude, Longitude = request.Longtitude });
                List<MovieHomePageDTO> result = new List<MovieHomePageDTO>();
                foreach(var item in rawdata)
                {
                    var MinMax = await tiktokRepository.GetMinAndMaxPriceFromShowTimeId(item.ShowTimeId);
                    //this query may look stupid but better than joining a lot whole shet in repository
                    result.Add(new MovieHomePageDTO
                    {
                        MovieId = item.MovieId,
                        Title = item.MovieTitle,
                        TheaterId = item.TheaterId,
                        CoverImgURL = item.CoverImageURL,
                        MinPrice = MinMax.Item1,
                        MaxPrice = MinMax.Item2,
                        RecommendPoint = item.MinutesRemain * 0.8 + item.DistanceFromUser * 0.2,
                        ShowTimeId = item.ShowTimeId,
                        StartTime = item.StartTime,
                        DistanceCalculated = item.DistanceFromUser,
                        MinutesRemain = (int)item.MinutesRemain,
                        //FormattedStartTime = item.StartTime.ToString("dd") + "/" +
                        //    item.StartTime.ToString("MM") + "/" + item.StartTime.ToString("yyyy")
                        //    + " " + item.StartTime.ToString("HH") + ":" + item.StartTime.ToString("mm")
                        FormattedStartTime = item.StartTime.ToString("dd/MM/yyyy HH:mm"),
                        TheaterName = item.TheaterName
                    });
                }
                response.MovieHomePages = result;
                response.Message = Constants.MESSAGE_SUCCESS;
                return response;
            }
            catch(Exception ex)
            {
                if (ex.Message == Constants.DATABASE_UNAVAILABLE_MESSAGE)
                    return new MovieHomePageResponse { Message = "Can't access database" };
                if (ex.Message == "NOT ENOUGH MANA")
                {
                    var rawData = await movieRepository.GetLessThanThreeLosslessIncomingMovies();
                    List<MovieHomePageDTO> result = new List<MovieHomePageDTO>();
                    foreach (var item in rawData)
                    {
                        result.Add(new MovieHomePageDTO
                        {
                            MovieId = item.MovieId,
                            CoverImgURL = item.CoverImgURL,
                            Title = item.Title,
                            
                        });
                    }
                    response.MovieHomePages = result;
                    response.Message = "Not enough data to show suggestion movie with showtime.";
                    return response;
                }
                throw new Exception(ex.Message);
            }
        }
        /* --------------------- END DEALING WITH HOMEPAGE MOBILE APP --------------- */


        /* --------------------- START PRIVATE FUNCTION -------------- */
        private double CalculateDistanceByHaversine(Coordinate Start, Coordinate Destination) //Unit Meters
        {//Look fresh enough mlem mlem
            var d1 = Start.Latitude * (Math.PI / 180.0);
            var num1 = Start.Longitude * (Math.PI / 180.0);
            var d2 = Destination.Latitude * (Math.PI / 180.0);
            var num2 = Destination.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        /* --------------------- END PRIVATE FUNCTION ------------ */

        /* ------------ START FAKE DATA ZONE ------------------ */
        public IncomingMovieResponse FakeIncomingMovie()
        {
            IncomingMovieResponse response = new IncomingMovieResponse();
            List<SmallMovieHomePageDTO> list = new List<SmallMovieHomePageDTO>();
            SmallMovieHomePageDTO dto = new SmallMovieHomePageDTO();
            dto.MovieId = 2629;
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/2eLVCBcMyDjGi7uxAoGZYKjR1NE.jpg";
            list.Add(dto);
            dto.MovieId = 2638;
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/a88afINFZ7afG4PjgRtwG9JdWQ7.jpg";
            list.Add(dto);
            dto.MovieId = 2667;
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/2MiG2aG2OrOgnPpbv8xnuS984xQ.jpg";
            list.Add(dto);
            response.LosslessMovieList = list;
            response.Message = "hàng pha ke";
            return response;
        }

        public MovieHomePageResponse FakeShowingMovie()
        {
            MovieHomePageResponse response = new MovieHomePageResponse();
            List<MovieDTO> list = new List<MovieDTO>();
            MovieDTO dto = new MovieDTO();
            dto.MovieId = 4324;
            dto.Title = "Be Mine, Valentine";
            List<MovieType> tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 10770,
                    CateName = "TV Movie"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 90;
            dto.ReleaseDate = "Jun 07, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/9ybX2cR1sgSABJbFoWZT0XBK1XB.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/zrNnWwVZTt1nwlrJr7OvqokZYZG.jpg";
            list.Add(dto);
            dto = new MovieDTO();


            dto.MovieId = 4327;
            dto.Title = "As a Prelude to Fear";
            tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 27,
                    CateName = "Horror"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 97;
            dto.ReleaseDate = "Jun 07, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/jvUt8hC3K5B6kQ8rXFNhCo1XNDw.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/1JgJo3MU8HcgacjrRYzPrhNMSYz.jpg";
            list.Add(dto);
            dto = new MovieDTO();


            dto.MovieId = 4387;
            dto.Title = "Father of the Bride";
            tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 10749,
                    CateName = "Romance"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 117;
            dto.ReleaseDate = "Jun 05, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/oQMqMLIJXZAVMplga9RhDyZInAW.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/aRvwJoqO7121AIpEnIMgP0omNj6.jpg";
            list.Add(dto);
            dto = new MovieDTO();


            dto.MovieId = 4418;
            dto.Title = "Buried In Barstow";
            tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 53,
                    CateName = "Thriller"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 90;
            dto.ReleaseDate = "Jun 04, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/xY6smX9kNLNqODRjT7pWwTHpRiH.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/6wnxj7jPpANISwMlfQ0Z09VViYv.jpg";
            list.Add(dto);
            dto = new MovieDTO();


            dto.MovieId = 4421;
            dto.Title = "Hidden Gems";
            tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 10770,
                    CateName = "TV Movie"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 90;
            dto.ReleaseDate = "Jun 04, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/4z6k3BuINspRCVEQT5vg2RBj29m.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/vgcxiduPt1siWT5E2QnQDZGNExF.jpg";
            list.Add(dto);
            dto = new MovieDTO();


            dto.MovieId = 4424;
            dto.Title = "Met Opera 2021/22: Brett Dean HAMLET";
            tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 10402,
                    CateName = "Music"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 195;
            dto.ReleaseDate = "Jun 04, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/zCdGgTL5LmsmXc0mNUa4zl5cS4w.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/e7Ei7pwkw4X00lZT7QTzJqaB29V.jpg";
            list.Add(dto);
            dto = new MovieDTO();


            dto.MovieId = 4450;
            dto.Title = "Hollywood Stargirl";
            tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 10402,
                    CateName = "Music"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 90;
            dto.ReleaseDate = "Jun 03, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/dOxFhcigrZQLlKglX958yjcaCo0.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/jaqb26TPRjyRJmJHMQW2E3eoI56.jpg";
            list.Add(dto);
            dto = new MovieDTO();


            dto.MovieId = 4460;
            dto.Title = "Fire Island";
            tempList = new List<MovieType>()
            {
                new MovieType()
                {
                    CateId = 35,
                    CateName = "Comedy"
                },
            };
            dto.Categories = tempList;
            dto.AgeRestrict = 0;
            dto.Duration = 105;
            dto.ReleaseDate = "Jun 03, 2022";
            dto.CoverImgURL = "https://image.tmdb.org/t/p/w500/iac7xBu5T8hLK4DadOhFzqgglCB.jpg";
            dto.PosterImgURL = "https://image.tmdb.org/t/p/w500/2vVUdYoqUX5rK8plxPGERGGjQLI.jpg";
            list.Add(dto);
            response.MovieHomePages = null;
            response.Message = "Hàng pha ke";
            return response;
        }

        public MovieDetailResponse FakeDetailMovieFor69(int movieId = 69)
        {
            MovieDetailResponse response = new MovieDetailResponse();
            MovieDetailDTO dto = new MovieDetailDTO();
            dto.Id = 69;
            dto.Title = "Scream: Legacy";
            //dto.ReleaseDate = new DateTime(2022, 3, 25);
            dto.Duration = 65;
            dto.RestrictedAge = 0;
            dto.PosterImageUrl = "https://image.tmdb.org/t/p/w500/CJwKE5dVGCEYB26PXgCSqQqrit.jpg";
            dto.CoverImageUrl = "";
            dto.TrailerUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
            dto.Description = "";
            dto.ViewCount = 1;
            dto.TotalRating = 5;
            //dto.DirectorId = null;
            List<MovieType> list = new List<MovieType>()
            {
                new MovieType
                {
                    CateId = 27,
                    CateName = "Horror"
                },
                new MovieType
                {
                    CateId = 53,
                    CateName = "Thriller"
                },
                new MovieType
                {
                    CateId = 9648,
                    CateName = "Mystery"
                }
            };

            dto.movieTypes = list;

            response.MovieDetail = dto;
            response.Message = "Hàng pha ke";
            return response;
        }

        /// <summary>
        /// Saved for someday
        /// </summary>
        private void ActorsFakeData()
        {
            //try
            //{
            //    List<string> actors = new List<string>();
            //    foreach (string line in System.IO.File.ReadLines("Actors.txt"))
            //    {
            //        actors.Add(line);
            //    }
            //    int counter = 1;
            //    Random random = new Random();
            //    using (var _context = new F_CINEMAContext())
            //    {
            //        foreach (var movie in _context.Movies.ToList())
            //        {
            //            int HowManyActorsDoYouWant = random.Next(3, 6);
            //            string[] ChoosenActor = new string[HowManyActorsDoYouWant];
            //            for (int i = 0; i < HowManyActorsDoYouWant; i++)
            //            {
            //                ChoosenActor[i] = actors[random.Next(actors.Count)];
            //            }
            //            movie.Actors = JsonConvert.SerializeObject(ChoosenActor);
            //            _context.Movies.Update(movie);
            //            await _context.SaveChangesAsync();
            //            Console.WriteLine(counter + " movies has been updated");
            //            counter++;
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
        
        /* ------------ END FAKE DATA ZONE ------------------- */
    }
}
