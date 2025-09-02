using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetProductByTypeHandler : IRequestHandler<GetProductByTypeQuery, List<Product>>
    {
        private ApplicationDbContext _context;

        public GetProductByTypeHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> Handle(GetProductByTypeQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                            .Where(p => p.CategoryId == request.CategoryId)
                            .ToListAsync(cancellationToken);
        }
    }
}
