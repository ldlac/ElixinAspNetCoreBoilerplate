using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElixinBackend.Users.UseCases.GetUserUseCase
{
    public class GetUserByCommand : IRequest<UserView>, IValidatableObject
    {
        [Required]
        public string Username { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            return results;
        }
    }

    public class GetUserByCommandHandler : IRequestHandler<GetUserByCommand, UserView>
    {
        private readonly IMediator _mediator;

        public GetUserByCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<UserView> Handle(GetUserByCommand request, CancellationToken cancellationToken)
        {
            var query = new GetUserByQuery(x => x.Username == request.Username);

            var user = (await _mediator.Send(query)).FirstOrDefault();

            if (user is null) throw new UserNotFoundException();

            return UserView.FromUser(user);
        }
    }
}