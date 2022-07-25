using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Movies.Api.Middleware;

namespace Movies.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionHandlerExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
