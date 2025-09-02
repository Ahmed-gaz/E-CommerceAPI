using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_CommerceAPI.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingOpperationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        public ShoppingOpperationsController(ApplicationDbContext context, IMediator mediator)
        {
            _mediator = mediator;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductInCart()
        {
            var userId = User.FindFirstValue("uid");
            var cart = await _mediator.Send(new GetProductInCartQuery(userId));

            if (cart is null)
                return NotFound("User has no cart");

            return Ok(cart);
        }


        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddProductToCart(string productName)
        {
            var userId = User.FindFirstValue("uid");

            var addedProduct = await _mediator.Send(new AddToCartCommand(productName, userId));

            if (addedProduct is null)
                return NotFound("product not Found");

            return Ok(new
            {
                Message = "seved in cart",
                addedProduct
            });
        }




        [HttpPost("CreateOrder")]
        public async Task<IActionResult> Checkout([FromServices] StripeService stripeService)
        {
            var userId = User.FindFirstValue("uid");
            var session = await _mediator.Send(new CreateOrderCommand(stripeService,userId));

            return Ok(new { url = session.Url });
        }


        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductInCart(int productId)
        {
            var userId = User.FindFirstValue("uid");
            
            var cartItem = await _mediator.Send(new DeleteProductInCartCommand(productId,userId));

            if (cartItem is null)
                return NotFound("cart or product not found");

 

            return Ok(new
            {
                Message = "deleted",
                CartItem = cartItem
            });
        }
    }
}
