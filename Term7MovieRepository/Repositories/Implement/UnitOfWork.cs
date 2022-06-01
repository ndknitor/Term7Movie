using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public UnitOfWork(AppDbContext context, IOptions<ConnectionOption> connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption.Value;
        }

        public IActorRepository ActorRepository { get => new ActorRepository(_context); }
        public ICategoryRepository CategoryRepository { get => new CategoryRepository(_context); }
        public ICompanyRepository CompanyRepository { get => new CompanyRepository(_context); }
        public ILanguageRepository LanguageRepository { get => new LanguageRepository(_context); }
        public IMovieActorRepository MovieActorRepository { get => new MovieActorRepository(_context); }
        public IMovieCategoryRepository MovieCategoryRepository { get => new MovieCategoryRepository(_context); }
        public IMovieLanguageRepository MovieLanguageRepository { get => new MovieLanguageRepository(_context); }
        public IMovieRepository MovieRepository { get => new MovieRepository(_context); }
        public IRefreshTokenRepository RefreshTokenRepository { get => new RefreshTokenRepository(_context, _connectionOption); }
        public IRoomRepository RoomRepository { get => new RoomRepository(_context, _connectionOption); }
        public ISeatRepository SeatRepository { get => new SeatRepository(_context); }
        public ISeatTypeRepository SeatTypeRepository { get => new SeatTypeRepository(_context); }
        public IShowtimeRepository ShowtimeRepository { get => new ShowtimeRepository(_context); }
        public ITheaterRepository TheaterRepository { get => new TheaterRepository(_context); }
        public ITicketRepository TicketRepository { get => new TicketRepository(_context); }
        public ITicketStatusRepository TicketStatusRepository { get => new TicketStatusRepository(_context); }
        public ITransactionHistoryRepository TransactionHistoryRepository { get => new TransactionHistoryRepository(_context); }
        public ITransactionRepository TransactionRepository { get => new TransactionRepository(_context); }
        public ITransactionStatusRepository TransactionStatusRepository { get => new TransactionStatusRepository(_context); }
        public IUserRepository UserRepository { get => new UserRepository(_context, _connectionOption); }
        public IPromotionCodeRepository PromotionCodeRepository { get => new PromotionCodeRepository(_context); }
        public IPromotionTypeRepository PromotionTypeRepository { get => new PromotionTypeRepository(_context); }
        public IUserLoginRepository UserLoginRepository { get => new UserLoginRepository(_context); }
        public IRoleRepository RoleRepository { get => new RoleRepository(_context); }
        public bool HasChange()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task<bool> CompleteAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
