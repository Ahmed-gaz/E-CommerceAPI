using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetProductInCartHandler : IRequestHandler<GetProductInCartQuery, List<CartItems>?>
    {
        private readonly IShoppingOpperationsRepo _repo;
        public GetProductInCartHandler(IShoppingOpperationsRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<CartItems>?> Handle(GetProductInCartQuery request, CancellationToken cancellationToken)
        {
           var cartItemsInCart = await _repo.GetProductInCart(request.userId);
           
            return cartItemsInCart;
        }
    }
}
