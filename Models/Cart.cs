using System.Text.Json.Serialization;

namespace E_CommerceAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }

        public User User { get; set; }
        [JsonIgnore]
        public ICollection<CartItems> CartItems { get; set; }
    }
}
