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

            return services;
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOption>(config.GetSection(Constants.JWT));

            services.Configure<GoogleAuthOption>(config.GetSection(Constants.GOOGLE_CREDENTIAL));

            services.Configure<ConnectionOption>(config.GetSection(Constants.CONNECTION_STRING));

            services.Configure<GoongOption>(config.GetSection(Constants.GOONG_IO));

            return services;
        }
    }
}
