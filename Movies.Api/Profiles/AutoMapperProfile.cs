using AutoMapper;
using Movies.Common.Dto;
using Movies.Entities.Entities;

namespace Movies.Api.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
