using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Term7MovieCore.Entities
{
    public class AppDbContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, UserRole, UserLogin, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieCategory> MovieCategories { get; set; }
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
        public virtual DbSet<TicketType> TicketTypes { set; get; }
        public virtual DbSet<ShowtimeTicketType> ShowtimeTicketTypes { set; get; }
        public virtual DbSet<MomoPaymentCreateRequest> PaymentRequests { set; get; }
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

            builder.Entity<MovieCategory>()
                .HasKey(mc => new { mc.MovieId, mc.CategoryId });

            builder.Entity<RefreshToken>()
                .HasIndex(r => r.Jti)
                .IsUnique();

            builder.Entity<MovieRating>()
                .HasKey(mr => new { mr.MovieId, mr.UserId });

            // builder.Entity<Movie>()
            //    .HasIndex(m => m.ExternalId)
            //    .IsUnique();

            builder.Entity<Movie>()
                .Property(m => m.TrailerUrl)
                .IsRequired(false);

            builder.Entity<Movie>()
                .Property(m => m.CoverImageUrl)
                .IsRequired(false);

            builder.Entity<Movie>()
                .Property(m => m.PosterImageUrl)
                .IsRequired(false);

            builder.Entity<Movie>()
                .Property(m => m.Description)
                .IsRequired(false);

            builder.Entity<Room>()
                .HasIndex(r => new { r.No, r.TheaterId })
                .IsUnique();

            builder.Entity<Seat>()
                .HasIndex(r => new { r.RoomId, r.RowPos, r.ColumnPos })
                .IsUnique();

            builder.Entity<Theater>()
                .Property(t => t.Latitude)
                .IsRequired(false);

            builder.Entity<Theater>()
                .Property(t => t.Longitude)
                .IsRequired(false);

            builder.Entity<User>()
                .Property(t => t.CompanyId)
                .IsRequired(false);

            builder.Entity<TheaterCompany>()
                .HasOne(t => t.Manager)
                .WithOne(u => u.Company)
                .HasForeignKey<User>(u => u.CompanyId)
                .IsRequired(false);

            builder.Entity<User>()
                .HasOne(u => u.Company)
                .WithOne(c => c.Manager)
                .HasForeignKey<TheaterCompany>(t => t.ManagerId)
                .IsRequired(false);

            builder.Entity<ShowtimeTicketType>()
                .HasOne(stt => stt.Showtime)
                .WithMany(sh => sh.ShowtimeTicketTypes)
                .HasForeignKey(stt => stt.ShowtimeId);

            builder.Entity<ShowtimeTicketType>()
                .HasOne(stt => stt.TicketType)
                .WithMany(tt => tt.ShowtimeTicketTypes)
                .HasForeignKey(stt => stt.TicketTypeId);

            builder.Entity<ShowtimeTicketType>()
                .HasIndex(shtt => new { shtt.ShowtimeId, shtt.TicketTypeId })
                .IsUnique(true);

            builder.Entity<TicketType>()
                .HasIndex(tt => new { tt.Name, tt.CompanyId })
                .IsUnique(true);

            builder.Entity<Ticket>()
                .HasIndex(t => new { t.SeatId, t.ShowTimeId })
                .IsUnique(true);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var name = entityType.GetTableName();
                if (name.StartsWith("AspNet")) entityType.SetTableName(name.Substring(6));
            }
        }
    }
}
