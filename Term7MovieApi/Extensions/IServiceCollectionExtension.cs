using Microsoft.AspNetCore.Authorization;
using Term7MovieApi.Handlers;
using Term7MovieApi.Requirements;
using Term7MovieApi.Requirements.RoomRequirement;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Options;
using Term7MovieService.Services.Implement;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection InjectProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IMovieService, MovieService>();

            services.AddScoped<IRoomService, RoomService>();

            services.AddScoped<ISeatService, SeatService>();

            services.AddScoped<ISeatTypeService, SeatTypeService>();

            services.AddScoped<ITheaterService, TheaterService>();

            services.AddScoped<ILocationService, LocationService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IShowtimeService, ShowtimeService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ITicketService, TicketService>();

            services.AddScoped<ITransactionService, TransactionService>();

            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOption>(config.GetSection(Constants.JWT));

            services.Configure<GoogleAuthOption>(config.GetSection(Constants.GOOGLE_CREDENTIAL));

            services.Configure<ConnectionOption>(config.GetSection(Constants.CONNECTION_STRING));

            services.Configure<GoongOption>(config.GetSection(Constants.GOONG_IO));

            services.Configure<MomoOption>(config.GetSection(Constants.MOMO_API));

            return services;
        }

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                option.AddPolicy(Constants.POLICY_CREATE_ROOM_SAME_THEATER, policy => policy.Requirements.Add(new CreateRoomWithSameTheaterRequirement()));
                option.AddPolicy(Constants.POLICY_UPDATE_ROOM_SAME_THEATER, policy => policy.Requirements.Add(new UpdateRoomWithSameTheaterRequirement()));
                option.AddPolicy(Constants.POLICY_DELETE_ROOM_SAME_THEATER, policy => policy.Requirements.Add(new DeleteRoomWithSameTheaterRequirement()));

                option.AddPolicy(Constants.POLICY_CREATE_TRANSACTION_TICKET_SAME_SHOWTIME, policy => policy.Requirements.Add(new CreateTransactionTicketSameShowtimeRequirement()));
            });

            services.AddTransient<IAuthorizationHandler, GeneralAuthorizationHandler>();

            return services;
        }
    }
}
