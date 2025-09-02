using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using Google.Apis.Calendar.v3.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;


namespace E_CommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public OrderController(ApplicationDbContext context,IMediator mediator )
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpPost("stripe-webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            if (!Request.Headers.ContainsKey("Stripe-Signature"))
                return BadRequest("Missing Stripe-Signature header.");

            var signatureHeader = Request.Headers["Stripe-Signature"].ToString();

            var result = await _mediator.Send(new StripeWebhookCommand(json, signatureHeader));

            return Ok(result);
        }



    }
}
