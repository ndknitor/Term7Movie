using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Options;
using Term7MovieRepository.Cache.Implement;
using Term7MovieRepository.Cache.Interface;
using Term7MovieRepository.Repositories.Implement;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieApi.BackgroundServices
{
    public class DistributedCacheSupportService : IHostedService, IDisposable
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ILogger<DistributedCacheSupportService> _logger;
        private readonly IMovieRepository movieRepo;
        private Timer timer = null;

        private const int DELAY_FIRST_START_IN_MINUTE = 1;

        public DistributedCacheSupportService(IDistributedCache distributedCache, ILogger<DistributedCacheSupportService> logger, IOptions<ConnectionOption> option)
        {
            _cacheProvider = new CacheProvider(distributedCache);
            _logger = logger;
            movieRepo = new MovieRepository(null, option.Value);
        }
        public void Dispose()
        {
            timer?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Timer timer = new Timer(DoWork, null, TimeSpan.FromMinutes(DELAY_FIRST_START_IN_MINUTE), TimeSpan.FromMinutes(Constants.REDIS_CACHE_LOAD_IN_MINUTE));

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            await Task.CompletedTask;
        }

        public void DoWork(object state)
        {
            _logger.LogInformation(DateTime.UtcNow + " - Redis start setting values");

            IEnumerable<MovieModelDto> movies = movieRepo.GetAllMovie();

            _logger.LogInformation(DateTime.UtcNow + " - Redis set values");
            _cacheProvider.Remove(Constants.REDIS_KEY_MOVIE);
            _cacheProvider.SetValue(Constants.REDIS_KEY_MOVIE, movies);
        }
    }
}
