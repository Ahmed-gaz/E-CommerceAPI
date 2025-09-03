using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class DeleteProductInCartHandler : IRequestHandler<DeleteProductInCartCommand, CartItems?>
    {
        private readonly IShoppingOpperationsRepo _repo;
        public DeleteProductInCartHandler(IShoppingOpperationsRepo repo)
        {
            _repo = repo;
        }
        public async Task<CartItems?> Handle(DeleteProductInCartCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _repo.DeleteProductInCart(request.productId, request.userId);

            return cartItem;
        }
    }
}
