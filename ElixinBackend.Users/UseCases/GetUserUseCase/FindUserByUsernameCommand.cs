using ElixinBackend.Shared;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElixinBackend.Users.UseCases.GetUserUseCase
{
    public class FindUserByUsernameCommand : IRequest<CommandResponse<UserView>>, IValidatableObject
    {
        [Required]
        public string Username { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            return results;
        }
    }

    public class FindUserByUsernameCommandHandler : IRequestHandler<FindUserByUsernameCommand, CommandResponse<UserView>>
    {
        private readonly IMediator _mediator;

        public FindUserByUsernameCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommandResponse<UserView>> Handle(FindUserByUsernameCommand request, CancellationToken cancellationToken)
        {
            var query = new GetUserByQuery(x => x.Username == request.Username);

            var user = (await _mediator.Send(query)).FirstOrDefault();

            if (user is null)
            {
                return CommandResponse<UserView>.FromFailure(FindUserByUsernameCommandException.UserNotFound);
            }

            var userView = UserView.FromUser(user);

            return CommandResponse<UserView>.FromSuccess(userView);
        }
    }

    public static class FindUserByUsernameCommandException
    {
        public const string UserNotFound = "USER.NOT.FOUND";
    }
}