using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class GetProductInCartHandler : IRequestHandler<GetProductInCartQuery, List<CartItems>>
    {
        private readonly ApplicationDbContext _context;
        public GetProductInCartHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CartItems>> Handle(GetProductInCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(c => c.Product).ThenInclude(c => c.Category).FirstOrDefaultAsync(c => c.UserId.ToString() == request.userId);
            if (cart is null)
                return null;

            return cart.CartItems.ToList();
        }
    }
}
