using Microsoft.AspNetCore.Authorization;
using Term7MovieApi.BackgroundServices;
using Term7MovieApi.Handlers;
using Term7MovieApi.Requirements;
using Term7MovieApi.Requirements.RoomRequirement;
using Term7MovieApi.Requirements.ShowtimeRequirement;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Options;
using Term7MovieRepository.Cache.Implement;
using Term7MovieRepository.Cache.Interface;
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

            services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();

            services.AddScoped<ICompanyService, CompanyService>();

            services.AddScoped<ICacheProvider, CacheProvider>();

            services.AddScoped<IImageHostService, ImageHostService>();

            services.AddScoped<ITicketTypeService, TicketTypeService>();

            return services;
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOption>(config.GetSection(Constants.JWT));

            services.Configure<GoogleAuthOption>(config.GetSection(Constants.GOOGLE_CREDENTIAL));

            services.Configure<ConnectionOption>(config.GetSection(Constants.CONNECTION_STRING));

            services.Configure<GoongOption>(config.GetSection(Constants.GOONG_IO));

            services.Configure<MomoOption>(config.GetSection(Constants.MOMO_API));


            services.Configure<ImgBBOption>(config.GetSection(Constants.IMGBB_COM));

            services.Configure<ProfitFormulaOption>(config.GetSection(Constants.PROFIT_FORMULA));
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

                option.AddPolicy(Constants.POLICY_NO_OVERLAP_SHOWTIME, policy => policy.Requirements.Add(new NoOverlapShowtimeRequirement()));
                option.AddPolicy(Constants.POLICY_CREATE_SHOWTIME_SAME_MANAGER, policy => policy.Requirements.Add(new CreateShowtimeForSameManagerRequirement()));

                option.AddPolicy(Constants.POLICY_COMPANY_FILTER, policy => policy.Requirements.Add(new CompanyFilterRequirement()));


                option.AddPolicy(Constants.POLICY_MANAGER_CREATE_TICKET, policy => policy.Requirements.Add(new TicketCreateRequirement()));
            });

            services.AddTransient<IAuthorizationHandler, GeneralAuthorizationHandler>();

            return services;
        }

        public static IServiceCollection ConfigureBackgroundService(this IServiceCollection services)
        {
            services.AddHostedService<DeleteExpiredRefreshTokenService>();
            services.AddHostedService<DistributedCacheSupportService>();
            return services;
        }
    }
}
