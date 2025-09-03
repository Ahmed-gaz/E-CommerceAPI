using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using E_CommerceAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class LoginHandler : IRequestHandler<LoginCommand, string?>
    {
        private readonly IAuthenticationRepo _repo;
        private readonly TokenService _tokenService;


        public LoginHandler(IAuthenticationRepo repo, TokenService tokenService)
        {
            _tokenService = tokenService;
            _repo = repo;
        }
        public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.Login(request.loginDto);
            
            if (user == null || user.Password != request.loginDto.Password)
                return null;

            var token = _tokenService.GenerateToken(user.Id.ToString(), user.Email, user.Role);
            return token;
        }
    }
}
