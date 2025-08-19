namespace E_CommerceAPI.Models
{
    public enum ProductType
    {
        Electronics,
        Clothing,
        Food,
        Books
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set;}
        public int QuantityInStock { get; set; }
        public ProductType Type { get; set; } 
        //public ICollection<CartItems> CartItems { get; set; }
    }
}
