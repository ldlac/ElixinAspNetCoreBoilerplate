using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ElixinBackend.Users.UseCases.GetUserUseCase
{
    public class GetUserByQuery : IRequest<List<User>>
    {
        public GetUserByQuery(Expression<Func<User, bool>> x)
        {
            Where = x;
        }

        public static GetUserByQuery New(Expression<Func<User, bool>> x)
        {
            return new GetUserByQuery(x);
        }

        public Expression<Func<User, bool>> Where { get; set; }

        public class GetUserByQueryHandler : IRequestHandler<GetUserByQuery, List<User>>
        {
            private readonly IUserDbContext _context;

            public GetUserByQueryHandler(IUserDbContext context)
            {
                _context = context;
            }

            public async Task<List<User>> Handle(GetUserByQuery request, CancellationToken cancellationToken)
            {
                return await _context.Users.Where(request.Where).ToListAsync();
            }
        }
    }
}