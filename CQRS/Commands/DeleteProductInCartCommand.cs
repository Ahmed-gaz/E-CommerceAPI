using E_CommerceAPI.Models;
using MediatR;

namespace E_CommerceAPI.CQRS.Commands
{
    public record DeleteProductInCartCommand(int productId , string? userId) : IRequest<CartItems>;
    
    
}
