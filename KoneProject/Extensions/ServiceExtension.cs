using KoneProject.Interfaces;
using KoneProject.Repository;
using KoneProject.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using KoneProject.Mappings;

namespace KoneProject.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Injecte tes services ici
            services.AddScoped<IBookServices, BookServices>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<UserInterface, UserService>();
            services.AddSingleton<IJwtUtils, JwtUtils>();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingBooks).Assembly);

            return services;
        }
    }
}
