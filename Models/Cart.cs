using System.Text.Json.Serialization;

namespace E_CommerceAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }

        public decimal TotalPrice => PriceHelper(CartItems ?? new List<CartItems>());

        private decimal PriceHelper(ICollection<CartItems> cartItems)
        {
            decimal c = 0;
            foreach (CartItems cartItem in cartItems) {
                c += cartItem.Product.Price * cartItem.Quantity; 
            }
            return c;
        }

        [JsonIgnore]
        public ICollection<CartItems> CartItems { get; set; }
        
        [JsonIgnore]
        public User User { get; set; }

    }
}
