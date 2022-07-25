using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Movies.Common.Dto;
using Movies.Domain.Contract;
using Movies.Entities.Entities;

namespace Movies.Api.Controllers
{
    public class DataController : BaseController
    {
        private readonly IUserDomain _userDomain;
        private readonly IMovieDomain _movieDomain;

        public DataController(IUserDomain userDomain, IMovieDomain movieDomain, ILogger<DataController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(logger, mapper, httpContextAccessor)
        {
            _userDomain = userDomain;
            _movieDomain = movieDomain;
        }

        [HttpPost("CreateMovies")]
        public async Task<IActionResult> CreateMovies()
        {
            var user = await _userDomain.FirstOfDefaultAsync(x => true);

            if (user == null)
            {
                var userId = Guid.NewGuid();
                user = new User
                {
                    Id = userId,
                    FullName = $"FullName for {userId}",
                    Email = $"Email for {userId}",
                    PasswordHash = new byte[10],
                    PasswordSalt = new byte[5],
                    CreationDate = DateTimeOffset.UtcNow,
                    ModificationDate = DateTimeOffset.MinValue
                };

                await _userDomain.Create(user);
            }

            await GenerateDbRecords(5, user.Id, true);
            await GenerateDbRecords(5, user.Id, false);

            return Ok();
        }

        [HttpPost("DeleteAllMovies")]
        public async Task<IActionResult> DeleteAllMovies()
        {
            foreach (var item in await _movieDomain.GetAll())
            {
                await _movieDomain.Delete(item);
            }

            foreach (var item in await _userDomain.GetAll())
            {
                await _userDomain.Delete(item);
            }

            return Ok();
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
            await _movieDomain.Create(new Movie
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
        }

        #endregion
    }
}
