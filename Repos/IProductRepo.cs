using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;

namespace E_CommerceAPI.Repos
{
    public interface IProductRepo
    {
        public Task<List<Product>> GetProducts();
        public Task<List<Product>> GetByCategory(int categoryId);
        public Task<Product> InsertProduct(ProductDto productDto);
        public Task<Product> EditProduct(int productId,ProductDto productDto);
        public Task<Product?> Deleteproduct(int productId);
    }
}
