using E_CommerceAPI.Models;

namespace E_CommerceAPI.Repos
{
    public class OrderRepo : IOrderRepo
    {
        public Task<Payment> StripeWebhook()
        {
            throw new NotImplementedException();
        }
    }
}
