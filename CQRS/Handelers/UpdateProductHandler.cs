using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private ApplicationDbContext _context;
        public UpdateProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.productId);

            if (product == null)
               return null;

            product.Name = request.productDto.Name;
            product.Description = request.productDto.Description;
            product.Price = request.productDto.Price;
            product.CategoryId = request.productDto.CategoryId;
            product.QuantityInStock = request.productDto.QuantityInStock;

            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }
    }
}
