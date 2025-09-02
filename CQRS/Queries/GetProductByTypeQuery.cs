using E_CommerceAPI.Models;
using MediatR;

namespace E_CommerceAPI.CQRS.Queries
{
    public record GetProductByTypeQuery(int CategoryId) : IRequest<List<Product>>;
    
}
