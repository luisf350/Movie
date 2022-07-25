using System.Threading.Tasks;
using Movies.Entities.Entities;
using Movies.Repositories.Infrastructure;

namespace Movies.Repositories.Repositories
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task<int> CountAsync();
    }
}