using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Movies.Common.Dto;
using Movies.Entities.Entities;

namespace Movies.Domain.Contract
{
    public interface IMovieDomain : IDomainBase<Movie>
    {
        Task<PagedResponse<List<Movie>>> GetAllMovies(int pageNumber, int pageSize, Guid userId);
        Task<ResponseDto<Movie>> CreateMovie(MovieDto movie);
        Task<ResponseDto<Movie>> UpdateMovie(MovieDto movie);
        Task<ResponseDto<bool>> DeleteMovie(Movie movie);
    }
}