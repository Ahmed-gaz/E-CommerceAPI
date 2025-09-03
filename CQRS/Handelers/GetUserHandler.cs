using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, List<Models.User>>
    {
        private IAuthenticationRepo _repo;
        public GetUserHandler(IAuthenticationRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<Models.User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetUser();
        }
    }
}
