using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        #region members

        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        #endregion

        #region constructors

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        #endregion

        public IDataResult<User> Register(RegisterUserDto registerUserDto)
        {
            HashingHelper.CreateHash(registerUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _userService.Add(user);

            return new SuccessDataResult<User>(user, "Kayıt oldu");
        }

        public IDataResult<User> Login(LoginUserDto loginUserDto)
        {
            var userToCheck = _userService.GetByMail(loginUserDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserDoesNotExists);
            }

            if (!HashingHelper.VerifyHash(loginUserDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.InvalidPassword);
            }

            return new SuccessDataResult<User>(userToCheck, Messages.Success);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult("Kullanıcı mevcut");
            }

            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken<AccessToken>(user, claims);

            return new SuccessDataResult<AccessToken>(accessToken, "Token oluşturuldu");
        }
    }
}