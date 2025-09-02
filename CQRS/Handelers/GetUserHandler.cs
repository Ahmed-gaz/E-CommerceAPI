using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, List<Models.User>>
    {
        private ApplicationDbContext _context;
        public GetUserHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Models.User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_context.Users.ToList());
        }
    }
}
