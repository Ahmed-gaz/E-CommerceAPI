using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;



namespace E_CommerceAPI.CQRS.Handelers
{
    public class InsertProductHandler : IRequestHandler<InsertProductCommand , Product?>
    {
        private ApplicationDbContext _context;
        private IProductRepo _repo;
        public InsertProductHandler(ApplicationDbContext context, IProductRepo repo)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<Product?> Handle(InsertProductCommand request, CancellationToken cancellationToken)
        {
            var oldProduct = await _context.Products.AnyAsync(p => p.Name == request.productDto.Name);

            if (oldProduct)
                return null;

            var product = await _repo.InsertProduct(request.productDto);



            return product;
        }
    }
}
