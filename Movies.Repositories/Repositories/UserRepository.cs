using Microsoft.Extensions.Logging;
using Movies.Entities.Context;
using Movies.Entities.Entities;
using Movies.Repositories.Infrastructure;

namespace Movies.Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger) : base(context, logger)
        {
        }
    }
}