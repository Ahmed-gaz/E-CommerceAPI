using E_CommerceAPI.Models;

namespace E_CommerceAPI.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int QuantityInStock { get; set; }
        public int CategoryId { get; set; }

    }
}
