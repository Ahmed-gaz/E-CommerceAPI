using E_CommerceAPI.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Stripe.Checkout;

namespace E_CommerceAPI.Repos
{
    public interface IShoppingOpperationsRepo
    {
        public Task<List<CartItems>?> GetProductInCart(string userId);
        public Task<Cart?> AddToCart(string productName,string userId);
        public Task<Orderr?> CreateOrder(string userId);
        public Task<CartItems?> DeleteProductInCart(int productId , string userId);
    }
}
