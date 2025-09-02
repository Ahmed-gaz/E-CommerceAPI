using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Stripe;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand,Models.Product>
    {
        private ApplicationDbContext _context;

        public DeleteProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Product> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deletedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.productId);
            
            if (deletedProduct == null)
                return null;

            _context.Products.Remove(deletedProduct);
            await _context.SaveChangesAsync(cancellationToken);
            return deletedProduct;
        }

       
    }
}

