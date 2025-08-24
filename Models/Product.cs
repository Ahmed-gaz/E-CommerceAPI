using System.Text.Json.Serialization;

namespace E_CommerceAPI.Models
{
   
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set;}
        public int QuantityInStock { get; set; }
        public int CategoryId { get; set; }
        //[JsonIgnore]
        public Category Category { get; set; }
    }
}
