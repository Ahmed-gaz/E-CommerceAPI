using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace E_CommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var products = await _context.Products.ToListAsync();

            if (products.Count == 0 || products is null)
            {
                return NotFound("yok");
            }

            return Ok(products);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("type")]
        public async Task<IActionResult> GetByType([FromQuery] ProductType type)
        {
            var products = await _context.Products.Where(t => t.Type == type).ToListAsync();

            if (products.Count == 0 || products is null)
            {
                return NotFound("yok");
            }

            return Ok(products);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var oldProduct = await _context.Products.AnyAsync(p => p.Name == productDto.Name);

            if (oldProduct)
                return BadRequest("product already exist");


            var newProduct = new Product
            {
                Price = productDto.Price,
                Description = productDto.Description,
                Name = productDto.Name,
                QuantityInStock = productDto.QuantityInStock,
                
                
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok(newProduct);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id , [FromBody] ProductDto productDto)
        {
            var updateProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (updateProduct == null)
                return NotFound("product not found");


            updateProduct.Name = productDto.Name;
            updateProduct.Price = productDto.Price;
            updateProduct.Description = productDto.Description;
            updateProduct.QuantityInStock = productDto.QuantityInStock;

            await _context.SaveChangesAsync();

            return Ok(updateProduct);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleteProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            _context.Products.Remove(deleteProduct);
            await _context.SaveChangesAsync();

            return Ok(deleteProduct);
        }
    }
}
