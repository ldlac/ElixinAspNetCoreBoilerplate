using ElixinBackend.Shared;
using ElixinBackend.Users.UseCases.GetUserUseCase;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElixinBackend.Users.UseCases.RegisterUserUseCase
{
    public class RegisterUserCommand : IRequest<CommandResponse<UserView>>, IValidatableObject
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            return results;
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, CommandResponse<UserView>>
    {
        private readonly IMediator _mediator;

        public RegisterUserCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommandResponse<UserView>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Username);

            var isUsernameAlreadyExists = (await _mediator.Send(new GetUserByQuery(x => x.Username == user.Username))).Any();
            if (isUsernameAlreadyExists)
            {
                return CommandResponse<UserView>.FromFailure(RegisterUserCommandException.UsernameAlreadyExists);
            }

            var userCreated = await _mediator.Send(new RegisterUserQuery(user, request.Password));

            var userView = UserView.FromUser(userCreated);

            return CommandResponse<UserView>.FromSuccess(userView);
        }
    }

    public static class RegisterUserCommandException
    {
        public const string UsernameAlreadyExists = "USERNAME.ALREADY.EXISTS";
    }
}