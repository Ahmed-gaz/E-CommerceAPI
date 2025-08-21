using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using E_CommerceAPI.Models;

namespace E_CommerceAPI.Services
{
    public class StripeService
    {
        private readonly string _secretKey;

        public StripeService(IConfiguration configuration)
        {
            _secretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<Session> CreateCheckoutSessionAsync(Orderr order, Dictionary<string, string> metadata)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = order.OrderItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = item.Product.Price * 100, // السعر بالـ "cents"
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Quantity,
                }).ToList(),
                Mode = "payment",
                SuccessUrl = "https://localhost:5001/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:5001/cancel",
                Metadata = metadata
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }
    }
}
