using E_CommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
        public async Task<IActionResult> GetProduct()
        {
            var userId = User.FindFirstValue("uid");
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(c => c.Product).FirstOrDefaultAsync(c => c.UserId.ToString() == userId);

            if (cart is null)
                return NotFound("user has no cart");

            return Ok(cart.CartItems.ToList());
        }


        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddProductToCart(string productName)
        {
            var userId = User.FindFirstValue("uid");
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == Int32.Parse(userId));

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
                addedProduct.QuantityInStock--;
            }
            else
            {
                cart.CartItems.Add(new CartItems
                {
                    ProductId = addedProduct.Id,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();


            return Ok(new
            {
                Message = "seved in cart",
                Cart = cart
            });
        }
    }
}
