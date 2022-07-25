using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Entities.Context;
using Movies.Entities.Entities;
using Movies.Repositories.Infrastructure;
using System.Threading.Tasks;

namespace Movies.Repositories.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(AppDbContext context, ILogger<MovieRepository> logger) : base(context, logger)
        {
        }

        public async Task<int> CountAsync()
        {
            return await Context.Movies.CountAsync();
        }
    }
}