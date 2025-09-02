using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;

public class StripeWebhookHandler : IRequestHandler<StripeWebhookCommand, Payment?>
{
    private readonly ApplicationDbContext _context;

    public StripeWebhookHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> Handle(StripeWebhookCommand request, CancellationToken cancellationToken)
    {
        var stripeEvent = EventUtility.ConstructEvent(
            request.Json,
            request.SignatureHeader,
            secret: "whsec_67690e0c88517587d8894d2749494f9bea00aeb57906674ca864a2d045df462f"
        );

        if (stripeEvent.Type == "checkout.session.completed")
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (session?.Metadata == null || !session.Metadata.ContainsKey("orderId"))
                return null;

            var orderId = int.Parse(session.Metadata["orderId"]);
            var order = await _context.Orders
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order != null)
            {
                var payment = new Payment
                {
                    OrderId = order.Id,
                    Method = E_CommerceAPI.Models.PaymentMethod.Stripe,
                    Amount = (int)((session.AmountTotal ?? 0) / 100),
                    PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = PaymentStatus.Success,
                    TransactionId = session.Id
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync(cancellationToken);

                order.PaymentId = payment.Id;
                order.State = "Paid";
                await _context.SaveChangesAsync(cancellationToken);

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == order.UserId, cancellationToken);

                if (cart != null)
                {
                    cart.CartItems.Clear();
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return payment;
            }
        }

        return null;
    }
}
