using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;



namespace E_CommerceAPI.CQRS.Handelers
{
    public class InsertProductHandler : IRequestHandler<InsertProductCommand , Product>
    {
        private ApplicationDbContext _context;
        public InsertProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(InsertProductCommand request, CancellationToken cancellationToken)
        {
            var oldProduct = await _context.Products.AnyAsync(p => p.Name == request.productDto.Name);

            if (oldProduct)
                return null;

            var product = new Product
            {
                Name = request.productDto.Name,
                Price = request.productDto.Price,
                CategoryId = request.productDto.CategoryId,
                Description = request.productDto.Description,
                QuantityInStock = request.productDto.QuantityInStock
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
