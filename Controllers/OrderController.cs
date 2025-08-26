using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using Google.Apis.Calendar.v3.Data;
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

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("stripe-webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            // قراءة الـ body
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // التحقق من وجود هيدر Stripe-Signature
            if (!Request.Headers.ContainsKey("Stripe-Signature"))
                return BadRequest("Missing Stripe-Signature header.");

            var signatureHeader = Request.Headers["Stripe-Signature"].ToString();

            try
            {
                // إنشاء الـ event مع Stripe
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    secret: "whsec_67690e0c88517587d8894d2749494f9bea00aeb57906674ca864a2d045df462f"
                );

                // التعامل مع حدث Checkout Session Completed
                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    if (session?.Metadata == null || !session.Metadata.ContainsKey("orderId"))
                        return BadRequest("Missing orderId in session metadata.");

                    var orderId = int.Parse(session.Metadata["orderId"]);
                    var order = await _context.Orders
                        .Include(o => o.Payment)
                        .FirstOrDefaultAsync(o => o.Id == orderId);

                    if (order != null)
                    {
                        var payment = new Payment
                        {
                            OrderId = order.Id,
                            Method = Models.PaymentMethod.Stripe,
                            Amount = (int)((session.AmountTotal ?? 0) / 100),
                            PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                            Status = PaymentStatus.Success,
                            TransactionId = session.Id
                        };

                        _context.Payments.Add(payment);
                        await _context.SaveChangesAsync();

                        order.PaymentId = payment.Id;
                        order.State = "Paid";
                        await _context.SaveChangesAsync();

                        var cart = await _context.Carts
                            .Include(c => c.CartItems)
                            .FirstOrDefaultAsync(c => c.UserId == order.UserId);
                        if (cart != null)
                        {
                            cart.CartItems.Clear();
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                // أي خطأ من Stripe
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                // أي خطأ آخر
                return BadRequest(ex.Message);
            }
        }


    }
}
