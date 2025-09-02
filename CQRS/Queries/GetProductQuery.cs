using E_CommerceAPI.Models;
using MediatR;

namespace E_CommerceAPI.CQRS.Queries
{
    public record GetProductQuery :  IRequest<List<Product>>;
    
}
