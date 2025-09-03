using E_CommerceAPI.Models;

namespace E_CommerceAPI.Repos
{
    public interface IOrderRepo
    {
        public Task<Payment> StripeWebhook();
    }
}
