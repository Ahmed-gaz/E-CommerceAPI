using E_CommerceAPI.Models;
using MediatR;

namespace E_CommerceAPI.CQRS.Commands
{
    public record AddToCartCommand(string productName , string userId) : IRequest<Cart>;
    
    
}
