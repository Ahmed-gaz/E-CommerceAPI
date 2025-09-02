using E_CommerceAPI.Models;
using MediatR;

namespace E_CommerceAPI.CQRS.Commands
{
    public record DeleteProductCommand(int productId) : IRequest<Models.Product>;
   
}
