using Microsoft.Extensions.Options;
using Term7MovieCore.Data.Options;
using Term7MovieRepository.Repositories.Implement;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieApi.BackgroundServices
{
    public class DeleteExpiredRefreshTokenService : IHostedService, IDisposable
    {
        private Timer timer = null;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly ILogger<DeleteExpiredRefreshTokenService> _logger;

        public DeleteExpiredRefreshTokenService(IOptions<ConnectionOption> connectionOption, ILogger<DeleteExpiredRefreshTokenService> logger)
        {
            refreshTokenRepository = new RefreshTokenRepository(null, connectionOption.Value);
            _logger = logger;
        }

        public void Dispose()
        {
            if (timer != null) timer.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DateTime next23H59 = now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            // refreshTokenRepository.DeleteExpiredRefreshToken();
            timer = new Timer(DeleteExpiredRefreshToken, null, next23H59 - now, TimeSpan.FromDays(1));

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (timer != null) timer.Change(Timeout.Infinite, 0);

            await Task.CompletedTask;
        }

        private void DeleteExpiredRefreshToken(object state)
        {
            _logger.LogInformation(DateTime.UtcNow + "Start delete expired refresh tokens ");
            refreshTokenRepository.DeleteExpiredRefreshToken();
        }
    }
}
