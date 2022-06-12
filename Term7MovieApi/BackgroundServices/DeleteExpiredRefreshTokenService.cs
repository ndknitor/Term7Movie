using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieApi.BackgroundServices
{
    public class DeleteExpiredRefreshTokenService : IHostedService, IDisposable
    {
        private Timer timer = null;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpiredRefreshTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        void IDisposable.Dispose()
        {
            if (timer != null) timer.Dispose();
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DateTime next23H59 = now.Date.AddHours(23).AddMinutes(59).AddSeconds(58);
            
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
            _unitOfWork.RefreshTokenRepository.DeleteExpiredRefreshToken();
        }
    }
}
