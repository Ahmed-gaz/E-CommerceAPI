using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product?>
    {
        private IProductRepo _repo;
        public UpdateProductHandler(IProductRepo repo)
        {
            _repo = repo;
        }
        public async Task<Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repo.EditProduct(request.productId, request.productDto);

            if (product == null)
               return null;
           
            return product;
        }
    }
}
