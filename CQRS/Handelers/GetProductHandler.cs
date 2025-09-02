using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using MediatR;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, List<Product>>
    {
        private ApplicationDbContext _context;

        public GetProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_context.Products.ToList());
        }
    }
}
