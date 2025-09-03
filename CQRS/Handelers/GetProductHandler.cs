using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, List<Product>>
    {
        private IProductRepo _repo;

        public GetProductHandler(IProductRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetProducts();
        }
    }
}
