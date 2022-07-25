using Movies.Entities.Entities;
using Movies.Repositories.Infrastructure;

namespace Movies.Repositories.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
    }
}