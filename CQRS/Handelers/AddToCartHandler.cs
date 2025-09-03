using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, Cart?>
    {
        private readonly IShoppingOpperationsRepo _repo;
        public AddToCartHandler(IShoppingOpperationsRepo repo)
        {
            _repo = repo;
        }
        public async Task<Cart?> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repo.AddToCart(request.productName,request.userId);

            return cart;
        }
    }
}
