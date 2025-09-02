using E_CommerceAPI.DTOs;
using MediatR;
using Stripe.Climate;
using E_CommerceAPI.Models;


namespace E_CommerceAPI.CQRS.Commands
{
    public record UpdateProductCommand(ProductDto productDto , int productId) : IRequest<Models.Product>;
    
    
}
