using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Term7MovieCore.Entities
{
    public class AppDbContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, UserRole, UserLogin, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Director> Directors { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieActor> MoversActors { get; set; }
        public virtual DbSet<MovieCategory> MovieCategories { get; set; }
        public virtual DbSet<MovieLanguage> MovieLanguages { set; get; }
        public virtual DbSet<RefreshToken> RefreshTokens { set; get; }
        public virtual DbSet<Room> Rooms { set; get; }
        public virtual DbSet<Seat> Seats { set; get; }
        public virtual DbSet<SeatType> SeatTypes { set; get; }
        public virtual DbSet<Showtime> Showtimes { set; get; }
        public virtual DbSet<Theater> Theaters { set; get; }
        public virtual DbSet<TheaterCompany> Companies { set; get; }
        public virtual DbSet<Ticket> Tickets { set; get; }
        public virtual DbSet<TicketStatus> TicketStatuses { set; get; }
        public virtual DbSet<Transaction> Transactions { set; get; }
        public virtual DbSet<TransactionHistory> TransactionHistories { set; get; }
        public virtual DbSet<TransactionStatus> TransactionStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .Property(u => u.Address)
                .IsRequired(false);

            builder.Entity<User>()
                .Property(u => u.PictureUrl)
                .IsRequired(false);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<UserLogin>()
                .HasKey(ul => new { ul.ProviderKey, ul.LoginProvider });

            builder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            builder.Entity<MovieCategory>()
                .HasKey(mc => new { mc.MovieId, mc.CategoryId });

            builder.Entity<MovieLanguage>()
                .HasKey(ml => new { ml.MovieId, ml.LanguageId });

            builder.Entity<RefreshToken>()
                .HasIndex(r => r.Jti)
                .IsUnique();

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var name = entityType.GetTableName();
                if (name.StartsWith("AspNet")) entityType.SetTableName(name.Substring(6));
            }
        }
    }
}
