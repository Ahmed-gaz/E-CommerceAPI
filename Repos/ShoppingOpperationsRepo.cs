using E_CommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Threading;

namespace E_CommerceAPI.Repos
{
    public class ShoppingOpperationsRepo : IShoppingOpperationsRepo
    {
        private readonly ApplicationDbContext _context;
        public ShoppingOpperationsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItems>?> GetProductInCart(string userId)
        {
            var cartItemsInCart = await _context.Carts.Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(c => c.UserId.ToString() == userId);
            
            if (cartItemsInCart is null)
                return null;

            return cartItemsInCart.CartItems.ToList();
        }

        public async Task<Cart?> AddToCart(string productName ,string userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == Int32.Parse(userId));

            if (cart is null)
            {
                cart = new Cart
                {

                    UserId = Int32.Parse(userId),
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    CartItems = new List<CartItems>()
                };
                _context.Carts.Add(cart);
            }

            var addedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);

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

        public async Task<Orderr?> CreateOrder(string userId)
        {
            var cart = await _context.Carts
                            .Include(c => c.CartItems)
                            .ThenInclude(ci => ci.Product)
                            .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId)); // يحضر الكرت

            if (cart == null || !cart.CartItems.Any()) // اذا كان الكرت خالي
                return null;

            var order = new Orderr
            {
                UserId = int.Parse(userId),
                Date = DateOnly.FromDateTime(DateTime.Now),
                State = "Pending",
                Payment = null,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<CartItems?> DeleteProductInCart(int productId , string userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

            if(cart is null)
            {
                cart = new Cart
                {
                    UserId = Int32.Parse(userId),
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    CartItems = new List<CartItems>()
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }


            var cartItem = cart.CartItems.FirstOrDefault(p => p.ProductId == productId);

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
