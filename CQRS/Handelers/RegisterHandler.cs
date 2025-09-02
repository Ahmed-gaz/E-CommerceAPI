using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, User>
    {
        private readonly ApplicationDbContext _context;
        public RegisterHandler(ApplicationDbContext context)
        {
         _context = context;
        }
        public async Task<User> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.registerDto.Email))
                return null;

            using var hmac = new HMACSHA256();
            var user = new User
            {
                Name = request.registerDto.Name,
                Email = request.registerDto.Email,
                Password = request.registerDto.Password,
                Role = "user"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
