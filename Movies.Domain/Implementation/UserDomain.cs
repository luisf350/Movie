using FluentValidation;
using Movies.Common.Dto;
using Movies.Domain.Contract;
using Movies.Entities.Entities;
using Movies.Repositories.Repositories;
using System;
using System.Threading.Tasks;
using Movies.Common.Utils;

namespace Movies.Domain.Implementation
{
    public class UserDomain : DomainBase<User>, IUserDomain
    {
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public UserDomain(IUserRepository repository, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator) : base(repository)
        {
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        /// <summary>
        /// User Register
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> Register(RegisterDto register)
        {
            var result = new ResponseDto<bool>();
            var validationResult = await _registerValidator.ValidateAsync(register);
            if (!validationResult.IsValid)
            {
                result.HasErrors = true;

                foreach (var error in validationResult.Errors)
                    result.Errors.Add(error.ErrorMessage);

                return result;
            }

            if (await EmailExists(register.Email))
            {
                result.HasErrors = true;
                result.Errors.Add("Email already exists in the system");
                return result;
            }

            PasswordUtils.CreatePasswordHash(register.Password, out var passwordHash, out var passwordSalt);

            await Create(new User
            {
                Id = Guid.NewGuid(),
                Email = register.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FullName = register.FullName
            });
            result.Result = true;

            return result;
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<ResponseDto<User>> Login(LoginDto login)
        {
            var result = new ResponseDto<User>();
            var validationResult = await _loginValidator.ValidateAsync(login);

            if (!validationResult.IsValid)
            {
                result.HasErrors = true;

                foreach (var error in validationResult.Errors)
                    result.Errors.Add(error.ErrorMessage);

                return result;
            }

            var user = await Repository.FirstOfDefaultAsync(x => x.Email == login.Email);
            if (user == null || !PasswordUtils.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            {
                result.HasErrors = true;
                result.Errors.Add("Email and/or Password are not valid");
                return result;
            }

            user.LastLogin = DateTime.Now;
            await Repository.Update(user);

            result.Result = user;
            return result;
        }

        private async Task<bool> EmailExists(string email)
        {
            var userDb = await Repository.FirstOfDefaultAsync(x => x.Email == email);

            return userDb != null;
        }
    }
}