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

        public async Task<List<Product>> GetProducts()
        {
          return await _context.Products.ToListAsync();
        }
        public async Task<List<Product>> GetByCategory(int categoryId)
        {
            var product = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();

            return product;

        }

        public async Task<Product> InsertProduct(ProductDto productDto)
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
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<Product> EditProduct(int productId , ProductDto productDto)
        {
            var product = await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.QuantityInStock = productDto.QuantityInStock;
            product.CategoryId = productDto.CategoryId;

           await _context.SaveChangesAsync();

            return product; 
        }

        public async Task<Product?> Deleteproduct(int productId)
        {
            var product = await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
            if (product != null)
                _context.Products.Remove(product);
            
           await _context.SaveChangesAsync();

            return product;
        }

      
    }
}
