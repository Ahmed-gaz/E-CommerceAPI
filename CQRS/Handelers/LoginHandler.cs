using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public LoginHandler(ApplicationDbContext context , TokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.loginDto.Email);
            
            if (user == null || user.Password != request.loginDto.Password)
                return null;

            var token = _tokenService.GenerateToken(user.Id.ToString(), user.Email, user.Role);
            return token;
        }
    }
}
