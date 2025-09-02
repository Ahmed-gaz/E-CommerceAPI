using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceAPI.Repos
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
          return _context.Products.ToList();
        }
        public List<Product> GetByCategory(int categoryId)
        {
            var product = _context.Products.Where(p => p.CategoryId == categoryId).ToList();

            return product;

        }

        public int InsertProduct(ProductDto productDto)
        {

            var newProduct = new Product
            {
                Price = productDto.Price,
                Description = productDto.Description,
                Name = productDto.Name,
                QuantityInStock = productDto.QuantityInStock,
                CategoryId = productDto.CategoryId

            };
            _context.Products.Add(newProduct);
            return _context.SaveChanges();
        }

        public int EditProduct(int productId , ProductDto productDto)
        {
            var product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.QuantityInStock = productDto.QuantityInStock;
            product.CategoryId = productDto.CategoryId;

            return _context.SaveChanges();
        }

        public int Deleteproduct(int productId)
        {
            var product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
            if (product != null)
                _context.Products.Remove(product);
            return _context.SaveChanges();
        }

      
    }
}
