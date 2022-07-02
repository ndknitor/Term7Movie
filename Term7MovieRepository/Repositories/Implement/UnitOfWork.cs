using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Entities;
using Term7MovieRepository.Cache.Interface;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        private readonly ICacheProvider _cacheProvider;
        private readonly ProfitFormulaOption _profitFormulaOption;
        public UnitOfWork(AppDbContext context, ICacheProvider cacheProvider, IOptions<ConnectionOption> connectionOption, IOptions<ProfitFormulaOption> profitFormulaOption)
        {
            _context = context;
            _connectionOption = connectionOption.Value;
            _cacheProvider = cacheProvider;
            _profitFormulaOption = profitFormulaOption.Value;
        }

        public ICategoryRepository CategoryRepository { get => new CategoryRepository(_context, _connectionOption); }
        public ICompanyRepository CompanyRepository { get => new CompanyRepository(_context, _connectionOption); }
        public IMovieCategoryRepository MovieCategoryRepository { get => new MovieCategoryRepository(_context); }
        public IMovieRepository MovieRepository { get => new MovieRepository(_context, _connectionOption); }
        public IRefreshTokenRepository RefreshTokenRepository { get => new RefreshTokenRepository(_context, _connectionOption); }
        public IRoomRepository RoomRepository { get => new RoomRepository(_context, _connectionOption); }
        public ISeatRepository SeatRepository { get => new SeatRepository(_context, _connectionOption); }
        public ISeatTypeRepository SeatTypeRepository { get => new SeatTypeRepository(_context, _connectionOption); }
        public IShowtimeRepository ShowtimeRepository { get => new ShowtimeRepository(_context, _connectionOption); }
        public ITheaterRepository TheaterRepository { get => new TheaterRepository(_context, _connectionOption); }
        public ITicketRepository TicketRepository { get => new TicketRepository(_context, _connectionOption, _profitFormulaOption, _cacheProvider); }
        public ITicketStatusRepository TicketStatusRepository { get => new TicketStatusRepository(_context); }
        public ITransactionHistoryRepository TransactionHistoryRepository { get => new TransactionHistoryRepository(_context, _connectionOption); }
        public ITransactionRepository TransactionRepository { get => new TransactionRepository(_context); }
        public ITransactionStatusRepository TransactionStatusRepository { get => new TransactionStatusRepository(_context); }
        public IUserRepository UserRepository { get => new UserRepository(_context, _connectionOption); }
        public IPromotionCodeRepository PromotionCodeRepository { get => new PromotionCodeRepository(_context); }
        public IPromotionTypeRepository PromotionTypeRepository { get => new PromotionTypeRepository(_context); }
        public IUserLoginRepository UserLoginRepository { get => new UserLoginRepository(_context); }
        public IRoleRepository RoleRepository { get => new RoleRepository(_context); }

        public IPaymentRequestRepository PaymentRequestRepository { get => new PaymentRequestRepository(_context, _connectionOption); }
        public ITicketTypeRepository TicketTypeRepository { get => new TicketTypeRepository(_context, _connectionOption); }
        public IShowtimeTicketTypeRepository ShowtimeTicketTypeRepository { get => new ShowtimeTicketTypeRepository(_context, _connectionOption); }
        public bool HasChange()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task<bool> CompleteAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public bool Complete()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
