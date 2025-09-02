using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class DeleteProductInCartHandler : IRequestHandler<DeleteProductInCartCommand, CartItems?>
    {
        private readonly ApplicationDbContext _context;
        public DeleteProductInCartHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CartItems?> Handle(DeleteProductInCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(c => c.UserId == int.Parse(request.userId));

            if (cart is null)
                return null;

            var cartItem = cart.CartItems.FirstOrDefault(p => p.ProductId == request.productId);

            if (cartItem is null)
                return null;

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                cart.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();

            return cartItem;
        }
    }
}
