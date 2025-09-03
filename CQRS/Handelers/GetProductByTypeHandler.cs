using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetProductByTypeHandler : IRequestHandler<GetProductByTypeQuery, List<Product>>
    {
        private IProductRepo _repo;

        public GetProductByTypeHandler(IProductRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<Product>> Handle(GetProductByTypeQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByCategory(request.CategoryId);
        }
    }
}
