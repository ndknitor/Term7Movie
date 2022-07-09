using Microsoft.Extensions.Options;
using Term7MovieCore.Data.Options;
using Term7MovieRepository.Repositories.Implement;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieApi.BackgroundServices
{
    public class CancelExpiredPendingTransactionService : IDisposable, IHostedService
    {
        private readonly ITransactionRepository transactionRepository;
        public CancelExpiredPendingTransactionService(IOptions<ConnectionOption> options)
        {
            transactionRepository = new TransactionRepository(null, options.Value);
        }

        Timer timer;
        public void Dispose()
        {
            timer?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            await Task.CompletedTask;
        }

        public void DoWork(object state)
        {
            transactionRepository.CancelAllExpiredTransaction();
        }
    }
}
