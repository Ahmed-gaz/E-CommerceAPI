using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using MediatR;
using Stripe.Checkout;

namespace E_CommerceAPI.CQRS.Commands
{
    public record CreateOrderCommand(StripeService StripeService , string? userId) : IRequest<Session>; 
    
}
