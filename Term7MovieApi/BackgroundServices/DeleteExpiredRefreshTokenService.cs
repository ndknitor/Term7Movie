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

        public DeleteExpiredRefreshTokenService(IOptions<ConnectionOption> connectionOption)
        {
            refreshTokenRepository = new RefreshTokenRepository(null, connectionOption.Value);
        }

        void IDisposable.Dispose()
        {
            if (timer != null) timer.Dispose();
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DateTime next23H59 = now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            refreshTokenRepository.DeleteExpiredRefreshToken();
            timer = new Timer(DeleteExpiredRefreshToken, null, next23H59 - now, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            if (timer != null) timer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DeleteExpiredRefreshToken(object state)
        {
            refreshTokenRepository.DeleteExpiredRefreshToken();
        }
    }
}
