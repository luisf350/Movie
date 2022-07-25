using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moq;
using Movies.Common.Dto;
using Movies.Entities.Entities;
using NUnit.Framework;

namespace Movies.Domain.Test
{
    public class MovieDomainTest : BaseRepositoryTest
    {
        [Test]
        public async Task GetAllMoviesTest()
        {
            // Setup
            var userId = Guid.NewGuid();
            var moviesList = new List<Movie>();
            moviesList.AddRange(CreateMovieList(20, userId, true));


            MovieRepositoryMock.Setup(x => x.CountAsync())
                .ReturnsAsync(moviesList.Count);

            // Act
            var response = await MovieDomain.GetAllMovies(1, 10, userId);

            // Assert
            Assert.IsFalse(response.HasErrors);
            Assert.AreEqual(1, response.PageNumber);
            Assert.AreEqual(10, response.PageSize);
            Assert.AreEqual(20, response.TotalRecords);
        }

        [Test]
        public async Task CreateMovieValidationErrorsTest()
        {
            // Setup
            var movie = new MovieDto();

            // Act
            var response = await MovieDomain.CreateMovie(movie);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual(3, response.Errors.Count);
        }

        [Test]
        public async Task CreateMovieAlreadyExistsTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var movie = new MovieDto
            {
                Id = id,
                Title = $"Title for {id}",
                Director = $"Director for {id}",
                Poster = $"Poster for {id}",
                Released = DateTime.UtcNow.AddDays(-10),
                IsPrivate = true,
                UserId = userId
            };

            MovieRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
                .ReturnsAsync(new Movie());

            // Act
            var response = await MovieDomain.CreateMovie(movie);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual("There is already a record with same Title and same Director", response.Errors.FirstOrDefault());
        }

        [Test]
        public async Task CreateMovieOkTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var movie = new MovieDto
            {
                Id = id,
                Title = $"Title for {id}",
                Director = $"Director for {id}",
                Poster = $"Poster for {id}",
                Released = DateTime.UtcNow.AddDays(-10),
                IsPrivate = true,
                UserId = userId
            };

            MovieRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
                .ReturnsAsync((Movie)null);

            // Act
            var response = await MovieDomain.CreateMovie(movie);

            // Assert
            Assert.IsFalse(response.HasErrors);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(typeof(Movie), response.Result.GetType());
        }

        [Test]
        public async Task UpdateMovieNoRecordTest()
        {
            // Setup
            var movie = new MovieDto();
            MovieRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
                .ReturnsAsync((Movie)null);

            // Act
            var response = await MovieDomain.UpdateMovie(movie);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual("That movie is not present in our records", response.Errors.FirstOrDefault());
        }

        [Test]
        public async Task UpdateMovieDifferentUserTest()
        {
            // Setup
            var movieDb = new Movie
            {
                UserId = Guid.NewGuid()
            };

            var movie = new MovieDto();
            MovieRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
                .ReturnsAsync(movieDb);

            // Act
            var response = await MovieDomain.UpdateMovie(movie);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual("You cant modify a movie from other person", response.Errors.FirstOrDefault());
        }

        [Test]
        public async Task UpdateMovieOkTest()
        {
            // Setup
            var userId = Guid.NewGuid();
            var movieDb = new Movie
            {
                UserId = userId
            };

            var movie = new MovieDto
            {
                UserId = userId
            };

            MovieRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
                .ReturnsAsync(movieDb);

            // Act
            var response = await MovieDomain.UpdateMovie(movie);

            // Assert
            Assert.IsFalse(response.HasErrors);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(typeof(Movie), response.Result.GetType());
        }

        [Test]
        public async Task DeleteMovieTest()
        {
            // Setup
            var userId = Guid.NewGuid();
            var movieDb = new Movie
            {
                UserId = userId
            };

            // Act
            var response = await MovieDomain.DeleteMovie(movieDb);

            // Assert
            Assert.IsFalse(response.HasErrors);
        }

        #region Private Methods

        private List<Movie> CreateMovieList(int items, Guid userId, bool isPrivate)
        {
            var moviesList = new List<Movie>();

            for (int i = 0; i < items; i++)
            {
                var movieId = Guid.NewGuid();
                moviesList.Add(new Movie
                {
                    Id = movieId,
                    Title = $"Title for {movieId}",
                    Director = $"Director for {movieId}",
                    Poster = $"Poster for {movieId}",
                    Released = DateTime.UtcNow.AddDays(-i).AddHours(-i).AddMinutes(-i),
                    IsPrivate = isPrivate,
                    UserId = userId,
                    CreationDate = DateTimeOffset.UtcNow.AddDays(i).AddHours(i).AddMinutes(i),
                    ModificationDate = DateTimeOffset.MinValue
                });
            }

            return moviesList;
        }

        #endregion
    }
}