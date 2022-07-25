using System.Threading.Tasks;
using Movies.Common.Dto;
using Movies.Entities.Entities;

namespace Movies.Domain.Contract
{
    public interface IUserDomain : IDomainBase<User>
    {
        Task<ResponseDto<bool>> Register(RegisterDto register);
        Task<ResponseDto<User>> Login(LoginDto login);
    }
}