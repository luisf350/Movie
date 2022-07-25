using Moq;
using Movies.Common.Validators;
using Movies.Domain.Implementation;
using Movies.Repositories.Repositories;
using NUnit.Framework;

namespace Movies.Domain.Test
{
    public class BaseRepositoryTest
    {
        protected UserDomain UserDomain;
        protected MovieDomain MovieDomain;

        protected Mock<IUserRepository> UserRepositoryMock;
        protected Mock<IMovieRepository> MovieRepositoryMock;

        [SetUp]
        public void Setup()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            MovieRepositoryMock = new Mock<IMovieRepository>();

            UserDomain = new UserDomain(UserRepositoryMock.Object, new RegisterValidator(), new LoginValidator());
            MovieDomain = new MovieDomain(MovieRepositoryMock.Object, new MovieValidator());
        }
    }
}