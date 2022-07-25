using System;
using System.Linq;
using System.Threading.Tasks;
using Movies.Entities.Entities;
using NUnit.Framework;

namespace Movies.Repositories.Test
{
    public class UserRepositoryTest:BaseRepositoryTest
    {
        [Test]
        public async Task GetAllTest()
        {
            // Setup
            await GenerateDbRecords(10);

            // Act
            var result = await UserRepository.GetAll();

            // Assert
            Assert.AreEqual(result.Count(), 10);
        }

        [Test]
        public async Task GetTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id);

            // Act
            var result = await UserRepository.GetById(id);

            // Assert
            Assert.AreEqual(result.Id, id);
        }

        [Test]
        public async Task GetNotFoundTest()
        {
            // Setup
            var id = Guid.NewGuid();

            // Act
            var result = await UserRepository.GetById(id);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var model = new User
            {
                Id = id,
                FullName = $"FullName for{ id}",
                Email = $"Email for{ id}",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5],
                CreationDate = DateTimeOffset.UtcNow,
                ModificationDate = DateTimeOffset.MinValue
            };

            // Act
            var result = await UserRepository.Create(model);
            var dbRecord = await UserRepository.GetById(model.Id);

            // Assert
            Assert.AreEqual(result, 1);
            Assert.AreEqual(model.FullName, dbRecord.FullName);
            Assert.AreEqual(model.Email, dbRecord.Email);
            Assert.AreEqual(model.CreationDate, dbRecord.CreationDate);
            Assert.AreEqual(model.ModificationDate, dbRecord.ModificationDate);
        }

        [Test]
        public async Task UpdateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            const string newName = "Updated Name";
            await GenerateDbRecord(id);

            var model = await UserRepository.GetById(id);
            model.FullName = newName;

            var oldModificationDate = model.ModificationDate;

            // Act
            var result = await UserRepository.Update(model);
            var dbRecord = await UserRepository.GetById(model.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(model.FullName, newName);
            Assert.AreEqual(model.CreationDate, dbRecord.CreationDate);
            Assert.AreNotEqual(model.ModificationDate, oldModificationDate);
        }

        [Test]
        public async Task DeleteTest()
        {
            // Setup
            var id = Guid.NewGuid();
            await GenerateDbRecord(id);
            var model = await UserRepository.GetById(id);

            // Act
            var result = await UserRepository.Delete(model);
            var dbRecord = await UserRepository.GetById(model.Id);

            // Assert
            Assert.AreEqual(result, 1);
            Assert.IsNull(dbRecord);
        }

        #region Private methods

        private async Task GenerateDbRecords(int numberRecords)
        {
            for (int i = 0; i < numberRecords; i++)
            {
                var id = Guid.NewGuid();
                await GenerateDbRecord(id, i);
            }

        }

        private async Task GenerateDbRecord(Guid id, int iterator = 0)
        {
            Context.Users.Add(new User
            {
                Id = id,
                FullName = $"FullName for {id}",
                Email = $"Email for {id}",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5],
                CreationDate = DateTimeOffset.UtcNow.AddDays(iterator).AddHours(iterator).AddMinutes(iterator),
                ModificationDate = DateTimeOffset.MinValue
            });

            await Context.SaveChangesAsync();
        }

        #endregion

    }
}