using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace ElixinBackend.Users.UseCases.RegisterUserUseCase
{
    public class RegisterUserCommand : IRequest<UserView>, IValidatableObject
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

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserView>
    {
        private readonly IMediator _mediator;

        public RegisterUserCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<UserView> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Username);

            var query = new RegisterUserQuery(user, request.Password);

            var userCreated = await _mediator.Send(query);

            return UserView.FromUser(userCreated);
        }
    }
}