using System.Text.Json.Serialization;

namespace E_CommerceAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Type { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }


    }
}
