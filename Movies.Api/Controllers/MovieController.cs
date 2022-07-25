using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movies.Common.Dto;
using Movies.Domain.Contract;
using System.Threading.Tasks;

namespace Movies.Api.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieDomain _movieDomain;

        public MovieController(IMovieDomain movieDomain, ILogger<MovieController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(logger, mapper, httpContextAccessor)
        {
            _movieDomain = movieDomain;
        }

        /// <summary>
        /// Get Movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var movie = await _movieDomain.Find(id);
            if (movie == null)
                return BadRequest("That movie is not present in our records");
            if (movie.IsPrivate && movie.UserId != UserId)
                return BadRequest("You are not allow to view this movie");

            return Ok(Mapper.Map<MovieDto>(movie));
        }

        /// <summary>
        /// Get movies (with pagination)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var result = await _movieDomain.GetAllMovies(pageNumber, pageSize, UserId);
            
            return Ok(new PagedResponse<List<MovieDto>>
            {
                Result = Mapper.Map<List<MovieDto>>(result.Result),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            });
        }

        /// <summary>
        /// Create Movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(MovieDto movie)
        {
            movie.UserId = UserId;
            var result = await _movieDomain.CreateMovie(movie);
            if (result.HasErrors)
                return BadRequest(result.Errors);

            return Created(string.Empty, Mapper.Map<MovieDto>(result.Result));
        }

        /// <summary>
        /// Update Movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(MovieDto movie)
        {
            movie.UserId = UserId;
            var result = await _movieDomain.UpdateMovie(movie);
            if (result.HasErrors)
                return BadRequest(result.Errors);

            return Ok(Mapper.Map<MovieDto>(result.Result));
        }

        /// <summary>
        /// Delete Movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _movieDomain.Find(id);
            if (movie == null)
                return BadRequest("That movie is not present in our records");
            if (movie.UserId == UserId)
                return BadRequest("You cant delete a movie from other person");

            var result = await _movieDomain.DeleteMovie(movie);
            
            return result.Result ?
                Ok() :
                NotFound();
        }
    }
}
