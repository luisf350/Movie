using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.Common.Dto;
using Movies.Domain.Contract;
using Movies.Entities.Entities;
using Movies.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Domain.Implementation
{
    public class MovieDomain : DomainBase<Movie>, IMovieDomain
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<MovieDto> _movieValidator;

        public MovieDomain(IMovieRepository repository, IValidator<MovieDto> movieValidator) : base(repository)
        {
            _movieRepository = repository;
            _movieValidator = movieValidator;
        }

        public async Task<PagedResponse<List<Movie>>> GetAllMovies(int pageNumber, int pageSize, Guid userId)
        {
            var result = new PagedResponse<List<Movie>>
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var dataQuery = await Repository.GetAll(x => x.UserId == userId || !x.IsPrivate);

            result.Result = dataQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            result.TotalRecords = await _movieRepository.CountAsync();
            return result;
        }

        public async Task<ResponseDto<Movie>> CreateMovie(MovieDto movie)
        {
            var result = new ResponseDto<Movie>();
            var validationResult = await _movieValidator.ValidateAsync(movie);
            if (!validationResult.IsValid)
            {
                result.HasErrors = true;

                foreach (var error in validationResult.Errors)
                    result.Errors.Add(error.ErrorMessage);

                return result;
            }

            var dbRecord =
                await Repository.FirstOfDefaultAsync(x => x.Title == movie.Title && x.Director == movie.Director);
            if (dbRecord != null)
            {
                result.HasErrors = true;
                result.Errors.Add("There is already a record with same Title and same Director");
                return result;
            }

            var newMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = movie.Title,
                Poster = movie.Poster,
                Director = movie.Director,
                Released = movie.Released.Date,
                IsPrivate = movie.IsPrivate,
                UserId = movie.UserId
            };

            await Repository.Create(newMovie);

            result.Result = newMovie;
            return result;
        }

        public async Task<ResponseDto<Movie>> UpdateMovie(MovieDto movie)
        {
            var result = new ResponseDto<Movie>();
            var dbRecord =
                await Repository.FirstOfDefaultAsync(x => x.Id == movie.Id);
            if (dbRecord == null)
            {
                result.HasErrors = true;
                result.Errors.Add("That movie is not present in our records");
                return result;
            }
            if (dbRecord.UserId != movie.UserId)
            {
                result.HasErrors = true;
                result.Errors.Add("You cant modify a movie from other person");
                return result;
            }

            dbRecord.Title = movie.Title;
            dbRecord.Poster = movie.Poster;
            dbRecord.Director = movie.Director;
            dbRecord.Released = movie.Released.Date;
            dbRecord.IsPrivate = movie.IsPrivate;

            await Repository.Update(dbRecord);

            result.Result = dbRecord;
            return result;
        }

        public async Task<ResponseDto<bool>> DeleteMovie(Movie movie)
        {
            return new ResponseDto<bool>
            {
                Result = await Repository.Update(movie)
            };
        }
    }
}