namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IActorRepository ActorRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        IMovieActorRepository MovieActorRepository { get; }
        IMovieCategoryRepository MovieCategoryRepository { get; }
        IMovieLanguageRepository MovieLanguageRepository { get; }
        IMovieRepository MovieRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IRoomRepository RoomRepository { get; }
        ISeatRepository SeatRepository { get; }
        ISeatTypeRepository SeatTypeRepository { get; }
        IShowtimeRepository ShowtimeRepository { get; }
        ITheaterRepository TheaterRepository { get; }
        ITicketRepository TicketRepository { get; }
        ITicketStatusRepository TicketStatusRepository { get; }
        ITransactionHistoryRepository TransactionHistoryRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        ITransactionStatusRepository TransactionStatusRepository { get; }
        IUserRepository UserRepository { get; }
        IPromotionCodeRepository PromotionCodeRepository { get; }
        IPromotionTypeRepository PromotionTypeRepository { get; }
        IUserLoginRepository UserLoginRepository { get; }
        IRoleRepository RoleRepository { get; }
        bool HasChange();

        Task<bool> CompleteAsync();

    }
}
