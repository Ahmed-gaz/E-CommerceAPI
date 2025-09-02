using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;

namespace E_CommerceAPI.Repos
{
    public interface IProductRepo
    {
        public List<Product> GetProducts();
        public List<Product> GetByCategory(int categoryId);
        public int InsertProduct(ProductDto productDto);
        public int EditProduct(int productId,ProductDto productDto);
        public int Deleteproduct(int productId);
    }
}
