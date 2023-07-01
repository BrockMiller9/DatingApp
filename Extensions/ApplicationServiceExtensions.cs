
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<DataContext>(opt =>
             {
                 opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
             });



            services.AddCors(); // this will add the cors service to our application

            services.AddScoped<ITokenService, TokenService>();// this will add the token service to our application

            services.AddScoped<IUserRepository, UserRepository>();// this will add the user repository to our application

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // this will add the automapper to our application

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); // this will add the cloudinary settings to our application
            services.AddScoped<IPhotoService, PhotoService>(); // this will add the photo service to our application
            services.AddScoped<LogUserActivity>(); // this will add the log user activity service to our application
            services.AddScoped<ILikesRepository, LikesRepository>(); // this will add the likes repository to our application

            return services;
        }
    }
}