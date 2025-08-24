using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Stripe.Climate;
using System.Security.Claims;

namespace E_CommerceAPI.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingOpperationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShoppingOpperationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductInCart()
        {
            var userId = User.FindFirstValue("uid");
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(c => c.Product).ThenInclude(c => c.Category).FirstOrDefaultAsync(c => c.UserId.ToString() == userId);

            if (cart is null)
                return NotFound("user has no cart");

            return Ok(cart.CartItems.ToList());
        }


        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddProductToCart(string productName)
        {
            var userId = User.FindFirstValue("uid");
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == Int32.Parse(userId));

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
                return NotFound("yok");

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

           

            return Ok(new
            {
                Message = "seved in cart",
                Cart = cart
            });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductInCart(int productId)
        {
            var userId = User.FindFirstValue("uid");
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

            if (cart is null)
                return NotFound("cart not found");
        
            var cartItem = cart.CartItems.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem is null)
                return NotFound("product not found");

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                cart.CartItems.Remove(cartItem);
               
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "deleted",
                Cart = cart
            });
        }




        [HttpPost("CreateOrder")]
        public async Task<IActionResult> Checkout([FromServices] StripeService stripeService)
        {
            var userId = User.FindFirstValue("uid");
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId)); // يحضر الكرت
            
            if (cart == null || !cart.CartItems.Any()) // اذا كان الكرت خالي
                return BadRequest("Cart is empty");

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

            //  إنشاء جلسة الدفع عبر Stripe مع Metadata للربط بالـ Order
            var session = await stripeService.CreateCheckoutSessionAsync(order, metadata: new Dictionary<string, string>
            {
                { "orderId", order.Id.ToString() }
            });


            return Ok(new { url = session.Url });
        }

    }
}
