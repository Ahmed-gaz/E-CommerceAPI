using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Stripe.Checkout;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Session?>
    {
        private readonly ApplicationDbContext _context;

        public CreateOrderHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Session?> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == int.Parse(request.userId)); // يحضر الكرت

            if (cart == null || !cart.CartItems.Any()) // اذا كان الكرت خالي
                return null;

            var order = new Orderr
            {
                UserId = int.Parse(request.userId),
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
            var session = await request.StripeService.CreateCheckoutSessionAsync(order, metadata: new Dictionary<string, string>
            {
                { "orderId", order.Id.ToString() }
            });

            return session;
        }
    }
}
