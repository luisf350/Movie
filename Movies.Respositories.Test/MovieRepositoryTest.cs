using System;
using System.Linq;
using System.Threading.Tasks;
using Movies.Entities.Entities;
using NUnit.Framework;

namespace Movies.Repositories.Test
{
    public class MovieRepositoryTest : BaseRepositoryTest
    {
        [Test]
        public async Task GetAllTest()
        {
            // Setup
            var userId = Guid.NewGuid();
            await GenerateUserDbRecord(userId);
            await GenerateDbRecords(5, userId, true);

            // Act
            var result = await MovieRepository.GetAll();

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public async Task GetNotFoundTest()
        {
            // Setup
            var id = Guid.NewGuid();

            // Act
            var result = await MovieRepository.GetById(id);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            await GenerateUserDbRecord(userId);
            var model = new Movie
            {
                Id = id,
                Title = $"Title for {id}",
                Director = $"Director for {id}",
                Poster = $"Poster for {id}",
                Released = DateTime.UtcNow.AddDays(-10),
                IsPrivate = true,
                UserId = userId,
                CreationDate = DateTimeOffset.UtcNow,
                ModificationDate = DateTimeOffset.MinValue
            };

            // Act
            var result = await MovieRepository.Create(model);
            var dbRecord = await MovieRepository.GetById(model.Id);

            // Assert
            Assert.AreEqual(result, 1);
            Assert.AreEqual(model.Title, dbRecord.Title);
            Assert.AreEqual(model.Director, dbRecord.Director);
            Assert.AreEqual(model.Poster, dbRecord.Poster);
            Assert.AreEqual(model.Released, dbRecord.Released);
            Assert.AreEqual(model.IsPrivate, dbRecord.IsPrivate);
            Assert.AreEqual(model.UserId, dbRecord.UserId);
            Assert.AreEqual(model.CreationDate, dbRecord.CreationDate);
            Assert.AreEqual(model.ModificationDate, dbRecord.ModificationDate);

            await MovieRepository.Delete(dbRecord);
        }

        #region Private methods

        private async Task GenerateDbRecords(int numberRecords, Guid userId, bool isPrivate)
        {
            for (int i = 0; i < numberRecords; i++)
            {
                var id = Guid.NewGuid();
                await GenerateDbRecord(id, userId, isPrivate, i);
            }
        }

        private async Task GenerateDbRecord(Guid id, Guid userId, bool isPrivate, int iterator = 0)
        {
            Context.Movies.Add(new Movie
            {
                Id = id,
                Title = $"Title for {id}",
                Director = $"Director for {id}",
                Poster = $"Poster for {id}",
                Released = DateTime.UtcNow.AddDays(-iterator).AddHours(-iterator).AddMinutes(-iterator),
                IsPrivate = isPrivate,
                UserId = userId,
                CreationDate = DateTimeOffset.UtcNow.AddDays(iterator).AddHours(iterator).AddMinutes(iterator),
                ModificationDate = DateTimeOffset.MinValue
            });

            await Context.SaveChangesAsync();
        }

        private async Task GenerateUserDbRecord(Guid id)
        {
            Context.Users.Add(new User
            {
                Id = id,
                FullName = $"FullName for {id}",
                Email = $"Email for {id}",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5],
                CreationDate = DateTimeOffset.UtcNow,
                ModificationDate = DateTimeOffset.MinValue
            });

            await Context.SaveChangesAsync();
        }

        #endregion
    }
}