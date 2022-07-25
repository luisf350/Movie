using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Movies.Common.Dto;
using Movies.Common.Validators;
using Movies.Domain.Contract;
using Movies.Domain.Implementation;
using Movies.Repositories.Repositories;

namespace Movies.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DiExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Fluent Validations
            services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
            services.AddScoped<IValidator<LoginDto>, LoginValidator>();
            services.AddScoped<IValidator<MovieDto>, MovieValidator>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            // Domains
            services.AddScoped<IUserDomain, UserDomain>();
            services.AddScoped<IMovieDomain, MovieDomain>();
        }
    }
}
