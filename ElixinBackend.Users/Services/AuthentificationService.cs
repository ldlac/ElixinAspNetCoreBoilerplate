using ElixinBackend.Shared;
using ElixinBackend.Users.Helpers;
using ElixinBackend.Users.UseCases.GetUserUseCase;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ElixinBackend.Users.Services.IAuthentificationService;

namespace ElixinBackend.Users.Services
{
    public interface IAuthentificationService
    {
        Task<CommandResponse<User>> Authenticate(string username, string password);

        string GetJwtToken(User user);

        public static class AuthentificationServiceException
        {
            public const string AuthentificationFailed = "AUTHENTIFICATION.FAILED";
        }
    }

    public class AuthentificationService : IAuthentificationService
    {
        private readonly AppSettings _appSettings;
        private readonly IMediator _mediator;

        public AuthentificationService(IOptions<AppSettings> appSettings, IMediator mediator)
        {
            _appSettings = appSettings.Value;
            _mediator = mediator;
        }

        public async Task<CommandResponse<User>> Authenticate(string username, string password)
        {
            var user = (await _mediator.Send(GetUserByQuery.New(x => x.Username == username))).FirstOrDefault();

            if (user is null || !PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return CommandResponse<User>.FromFailure(AuthentificationServiceException.AuthentificationFailed);
            }

            return CommandResponse<User>.FromSuccess(user);
        }

        public string GetJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}