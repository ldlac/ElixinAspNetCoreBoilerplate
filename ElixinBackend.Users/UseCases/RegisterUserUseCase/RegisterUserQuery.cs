using ElixinBackend.Users.Helpers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ElixinBackend.Users.UseCases.RegisterUserUseCase
{
    public class RegisterUserQuery : IRequest<User>
    {
        public RegisterUserQuery(User user, string password)
        {
            User = user;
            Password = password;
        }

        public User User { get; set; }
        public string Password { get; set; }

        public class RegisterUserQueryHandler : IRequestHandler<RegisterUserQuery, User>
        {
            private readonly IUserDbContext _context;

            public RegisterUserQueryHandler(IUserDbContext context)
            {
                _context = context;
            }

            public async Task<User> Handle(RegisterUserQuery request, CancellationToken cancellationToken)
            {
                var user = request.User;

                PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return user;
            }
        }
    }
}