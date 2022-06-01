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

            return services;
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOption>(config.GetSection(Constants.JWT));

            services.Configure<GoogleAuthOption>(config.GetSection(Constants.GOOGLE_CREDENTIAL));

            services.Configure<ConnectionOption>(config.GetSection(Constants.CONNECTION_STRING));

            return services;
        }
    }
}
