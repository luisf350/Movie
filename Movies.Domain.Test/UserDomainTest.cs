using Moq;
using Movies.Common.Dto;
using Movies.Entities.Entities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Movies.Domain.Test
{
    public class UserDomainTest : BaseRepositoryTest
    {
        [Test]
        public async Task RegisterValidationErrorsTest()
        {
            // Setup
            var registerDto = new RegisterDto();

            // Act
            var response = await UserDomain.Register(registerDto);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual(2, response.Errors.Count);
        }

        [Test]
        public async Task RegisterEmailExistsTest()
        {
            // Setup
            var registerDto = new RegisterDto
            {
                Email = "some@email.com",
                Password = "S@m3P@asswor1d",
                FullName = "Some Name"
            };

            UserRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new User());

            // Act
            var response = await UserDomain.Register(registerDto);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual("Email already exists in the system", response.Errors.FirstOrDefault());
        }

        [Test]
        public async Task RegisterOkTest()
        {
            // Setup
            var registerDto = new RegisterDto
            {
                Email = "some@email.com",
                Password = "S@m3P@asswor1d",
                FullName = "Some Name"
            };

            UserRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act
            var response = await UserDomain.Register(registerDto);

            // Assert
            Assert.IsFalse(response.HasErrors);
        }

        [Test]
        public async Task LoginValidationErrorsTest()
        {
            // Setup
            var loginDto = new LoginDto();

            // Act
            var response = await UserDomain.Login(loginDto);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual(2, response.Errors.Count);
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public async Task LoginNotValidTest(bool userExist)
        {
            // Setup
            var loginDto = new LoginDto
            {
                Email = "some@email.com",
                Password = "S@m3P@asswor1d"
            };

            User user = null;
            if (userExist)
                user = new User
                {
                    PasswordSalt = new byte[5],
                    PasswordHash = new byte[10]
                };

            UserRepositoryMock.Setup(x => x.FirstOfDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Act
            var response = await UserDomain.Login(loginDto);

            // Assert
            Assert.IsTrue(response.HasErrors);
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual("Email and/or Password are not valid", response.Errors.FirstOrDefault());
        }

    }
}