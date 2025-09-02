using E_CommerceAPI.Models;
using MediatR;

namespace E_CommerceAPI.CQRS.Queries
{
    public record GetUserQuery : IRequest<List<Models.User>>;
   
}
