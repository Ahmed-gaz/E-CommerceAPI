using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, Cart?>
    {
        private readonly ApplicationDbContext _context;
        public AddToCartHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Cart?> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == Int32.Parse(request.userId), cancellationToken);

            if (cart is null)
            {
                cart = new Cart
                {

                    UserId = Int32.Parse(request.userId),
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    CartItems = new List<CartItems>()
                };
                _context.Carts.Add(cart);
            }

            var addedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == request.productName);

            if (addedProduct is null)
                return null;

            var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == addedProduct.Id);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.CartItems.Add(new CartItems
                {
                    ProductId = addedProduct.Id,
                    Quantity = 1
                });
            }
            addedProduct.QuantityInStock--;

            await _context.SaveChangesAsync();

            return cart;
        }
    }
}
