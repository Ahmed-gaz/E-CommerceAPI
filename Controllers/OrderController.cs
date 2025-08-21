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
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    secret: "whsec_67690e0c88517587d8894d2749494f9bea00aeb57906674ca864a2d045df462f" // ضع هنا Webhook Secret
                );

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    // 1. جلب Order باستخدام Metadata
                    var orderId = int.Parse(session.Metadata["orderId"]);
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                    if (order != null)
                    {
                        // 1. إنشاء Payment جديد
                        var payment = new Payment
                        {
                            OrderId = order.Id,
                            Method = Models.PaymentMethod.Stripe,                // طريقة الدفع
                            Amount = (int)((session.AmountTotal ?? 0) / 100), // Stripe Amount بالـ cents، نحوله للوحدات الصحيحة
                            PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                            Status = PaymentStatus.Success,              // بما أن الدفع اكتمل
                            TransactionId = session.Id                   // رقم المعاملة من Stripe
                        };
                        _context.Payments.Add(payment);
                        await _context.SaveChangesAsync();

                        // 2. تحديث Order
                        order.PaymentId = payment.Id; // إذا كان لديك FK
                        order.State = "Paid";
                        await _context.SaveChangesAsync();

                        // 3. حذف العناصر من Cart
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
                return BadRequest(e.Message);
            }
        }


    }
}
