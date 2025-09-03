using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using E_CommerceAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Stripe.Checkout;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Session?>
    {
        private readonly IShoppingOpperationsRepo _repo;

        public CreateOrderHandler(IShoppingOpperationsRepo repo)
        {
            _repo = repo;
        }

        public async Task<Session?> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.CreateOrder(request.userId);

            if (order is null)
                return null;
            //  إنشاء جلسة الدفع عبر Stripe مع Metadata للربط بالـ Order
            var session = await request.StripeService.CreateCheckoutSessionAsync(order, metadata: new Dictionary<string, string>
            {
                { "orderId", order.Id.ToString() }
            });

            return session;
        }
    }
}
